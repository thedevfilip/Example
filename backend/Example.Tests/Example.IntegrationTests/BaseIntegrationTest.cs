using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory) : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client { get; } = factory.CreateClient();

    protected async Task ExecuteScopedAsync(Func<IServiceProvider, Task> action)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task<TResult> ExecuteScopedAsync<TResult>(Func<IServiceProvider, Task<TResult>> action)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        return await action(scope.ServiceProvider);
    }
}
