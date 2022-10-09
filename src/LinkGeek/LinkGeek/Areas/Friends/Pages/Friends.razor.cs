using LinkGeek.Areas.ProfileCard.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;

namespace LinkGeek.Areas.Friends.Pages;

public class SearchModel
{
    [Required]
    public string? Search { get; set; }
}

[Authorize]
public partial class Friends
{
    public SearchModel search = new SearchModel();
    public ICollection<ProfilePageModel> results { get; set; } = new List<ProfilePageModel>();
    
    public async Task Search()
    {
        // results = ;

    }
}