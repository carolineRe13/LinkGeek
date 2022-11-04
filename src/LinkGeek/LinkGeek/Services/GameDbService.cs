using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LinkGeek.Data;
using LinkGeek.Models;

namespace LinkGeek.Services;

public class AuthResponse
{
    public string access_token { get; set; }
    public long expires_in { get; set; }
    public string token_type { get; set; }
}

public class GameEndpointResponse
{
    public class Cover
    {
        public long id { get; set; }
        public string url { get; set; }
    }

    public long id { get; set; }
    public string name { get; set; }

    public Cover? cover { get; set; }
}

public class GameDbService
{
    private const string Api = "https://api.igdb.com/v4/";
    private const int PageSize = 49;
    private const int MinRatingCount = 100;

    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string? _token;
    private DateTimeOffset _tokenExpiration = DateTimeOffset.MinValue;

    private readonly IContextProvider contextProvider;

    public GameDbService(IConfiguration configuration, IContextProvider contextProvider)
    {
        _clientId = configuration["igdb_clientId"];
        _clientSecret = configuration["igdb_clientSecret"];
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Client-ID", _clientId);
        this.contextProvider = contextProvider;
    }

    public async Task<ICollection<Game>> GetPopularGames(int page = 0)
    {
        var query = $"fields name, cover.url; where rating_count > {MinRatingCount}; sort rating desc; limit {PageSize}; offset {PageSize * page};";

        return await this.CallGameEndpoint(query);
    }
    
    public async Task<ICollection<Game>> SearchGames(string search, int page = 0)
    {
        var query = $"search \"{search}\"; fields name, cover.url; limit {PageSize}; offset {PageSize * page};";

        return await this.CallGameEndpoint(query);
    }

    private async Task<ICollection<Game>> CallGameEndpoint(string query)
    {
        var cache = this.GetGameSearchCache(query);
        if (cache != null)
        {
            return cache;
        }

        var gamesResponse = await this.CallEndpoint<ICollection<GameEndpointResponse>>("games", query);
        var games = gamesResponse.Select(g => new Game()
                { Id = $"{g.id}", Name = g.name, Logo = g.cover != null ? new Uri($"https:{g.cover.url}") : null })
            .ToList();
        
        // this.StoreGameSearchCache(query, games);
        
        return games;
    }

    private ICollection<Game>? GetGameSearchCache(string query)
    {
        using (var context = contextProvider.GetContext())
        {
            var games = context.GameSearchCache.AsQueryable()
                .Where(c => c.Query == query)
                .Where(c => c.LastUpdated > DateTimeOffset.UtcNow.AddHours(-12)) // TODO make configurable
                .OrderBy(c => c.Rank)
                .Select(c => c.Game)
                .ToList();

            if (games.Count > 0)
            {
                return games;
            }
        }

        return null;
    }

    private void StoreGameSearchCache(string query, ICollection<Game> searchResult)
    {
        if (searchResult.Count == 0)
        {
            return;
        }

        using (var context = contextProvider.GetContext())
        {
            var gamesDict = GetOrCreateGames(context, searchResult)
                .ToDictionary(g => g.Id, g => g);
            
            var gameSearchCache = searchResult.Select((g, i) => new GameSearchCacheItem()
            {
                Query = query,
                Rank = i,
                Game = gamesDict[g.Id],
                LastUpdated = DateTimeOffset.UtcNow
            }).ToList();
            
            context.GameSearchCache.UpdateRange(gameSearchCache);
            context.SaveChanges();
        }
    }

    private IEnumerable<Game> GetOrCreateGames(ApplicationDbContext context, ICollection<Game> searchResult)
    {
        var games = new List<Game>
        {
            Capacity = searchResult.Count
        };
        
        var existingGames = GetExistingGames(context, searchResult);
        games.AddRange(existingGames);
            
        var newGames = GetNewGames(existingGames, searchResult);
        games.AddRange(newGames);
            
        context.Game.AddRange(newGames);
        context.SaveChanges();

        return games;
    }

    private ICollection<Game> GetExistingGames(ApplicationDbContext context, ICollection<Game> games)
    {
        var gameIds = games.Select(g => g.Id).ToList();
        return context.Game.AsQueryable()
            .Where(g => gameIds.Contains(g.Id))
            .ToList();
    }
    
    private ICollection<Game> GetNewGames(ICollection<Game> existingGames, ICollection<Game> games)
    {
        var existingGameIds = existingGames.Select(g => g.Id).ToList();
        return games.Where(g => !existingGameIds.Contains(g.Id)).ToList();
    }

    private async Task<T> CallEndpoint<T>(string endpoint, string query)
    {
        await UpdateToken();

        var response = await _httpClient.PostAsync(Api + endpoint, new StringContent(query, Encoding.UTF8));
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content.Replace("t_thumb", "t_cover_big"));
    }

    private async Task UpdateToken()
    {
        if (_token == null || _tokenExpiration < DateTimeOffset.UtcNow)
        {
            var response = await _httpClient.PostAsync("https://id.twitch.tv/oauth2/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "grant_type", "client_credentials" }
                }));
            var authResponse =
                await JsonSerializer.DeserializeAsync<AuthResponse>(await response.Content.ReadAsStreamAsync());

            _token = authResponse.access_token;
            _tokenExpiration = DateTimeOffset.UtcNow.AddSeconds(authResponse.expires_in);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}