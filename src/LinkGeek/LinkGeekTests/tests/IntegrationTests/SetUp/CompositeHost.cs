using Microsoft.Extensions.Hosting;

namespace LinkGeek.tests.Pages.Shared.SetUp;

// Relay the call to both test host and kestrel host.
public class CompositeHost : IHost
{
    private readonly IHost testHost;
    private readonly IHost kestrelHost;
    public CompositeHost(IHost testHost, IHost kestrelHost)
    {
        this.testHost = testHost;
        this.kestrelHost = kestrelHost;
    }
    public IServiceProvider Services => this.testHost.Services;
    public void Dispose()
    {
        this.testHost.Dispose();
        this.kestrelHost.Dispose();
    }
    public async Task StartAsync(
        CancellationToken cancellationToken = default)
    {
        await this.testHost.StartAsync(cancellationToken);
        await this.kestrelHost.StartAsync(cancellationToken);
    }
    public async Task StopAsync(
        CancellationToken cancellationToken = default)
    {
        await this.testHost.StopAsync(cancellationToken);
        await this.kestrelHost.StopAsync(cancellationToken);
    }
}