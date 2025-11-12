using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Example.Api.Features.Organizations.Registration;
using Example.Api.Features.Projects.CreateProject;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.Registration;

namespace Example.IntegrationTests;

public class ProjectsTenantIsolationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private readonly string _password = "Password123!";

    [Fact]
    public async Task CreateProject_WithAuthentication_ShouldSucceed()
    {
        string organizationName = RandomId;

        string token = await RegisterUserAndGetTokenAsync(BuildEmail("0"), _password);
        await SendRegisterOrganizationRequestAsync(token, organizationName);

        string projectName = RandomId;

        CreateProjectRequest createProjectRequest = new(projectName, string.Empty);
        Client.DefaultRequestHeaders.Authorization = new(Constants.Bearer, token);
        
        HttpResponseMessage createResponse = await Client.PostAsJsonAsync(Routes.Projects, createProjectRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        CreateProjectResponse? createdProject = await createResponse.Content.ReadFromJsonAsync<CreateProjectResponse>();

        createdProject.Should().NotBeNull();
        createdProject!.Name.Should().Be(projectName);
    }

    [Fact]
    public async Task CreateProject_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        CreateProjectRequest createProjectRequest = new(RandomId, string.Empty);

        Client.DefaultRequestHeaders.Authorization = null;
        HttpResponseMessage response = await Client.PostAsJsonAsync(Routes.Projects, createProjectRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateProject_DifferentTenants_SameName_ShouldBothSucceed()
    {
        string email1 = BuildEmail("1");
        string email2 = BuildEmail("2");

        string token1 = await RegisterUserAndGetTokenAsync(email1, _password);
        string token2 = await RegisterUserAndGetTokenAsync(email2, _password);
        
        await SendRegisterOrganizationRequestAsync(token1, RandomId);
        await SendRegisterOrganizationRequestAsync(token2, RandomId);

        string newToken1 = await SendLoginRequestAsync(email1, _password);
        string newToken2 = await SendLoginRequestAsync(email2, _password);

        CreateProjectRequest request = new(RandomId, string.Empty);
        
        Client.DefaultRequestHeaders.Authorization = new(Constants.Bearer, newToken1);
        HttpResponseMessage response1 = await Client.PostAsJsonAsync(Routes.Projects, request);

        Client.DefaultRequestHeaders.Authorization = new(Constants.Bearer, newToken2);
        HttpResponseMessage response2 = await Client.PostAsJsonAsync(Routes.Projects, request);

        response1.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.Created);
        
        CreateProjectResponse? project1 = await response1.Content.ReadFromJsonAsync<CreateProjectResponse>();
        CreateProjectResponse? project2 = await response2.Content.ReadFromJsonAsync<CreateProjectResponse>();
        
        project1!.Name.Should().Be(project2!.Name);
        project1.Id.Should().NotBe(project2.Id);
    }

    private async Task<string> RegisterUserAndGetTokenAsync(string email, string password)
    {
        RegisterUserRequest registerRequest = new(email, password, string.Empty, string.Empty);
        HttpResponseMessage registerResponse = await Client.PostAsJsonAsync(Routes.RegisterUser, registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        return await SendLoginRequestAsync(email, password);
    }
    
    private async Task SendRegisterOrganizationRequestAsync(string token, string orgName)
    {
        Client.DefaultRequestHeaders.Authorization = new(Constants.Bearer, token);
        RegisterOrganizationRequest request = new(orgName);
        HttpResponseMessage response = await Client.PostAsJsonAsync(Routes.RegisterOrganization, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<string> SendLoginRequestAsync(string email, string password)
    {
        LoginUserRequest loginRequest = new(email, password);
        HttpResponseMessage loginResponse = await Client.PostAsJsonAsync(Routes.LoginUser, loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        LoginUserResponse? loginData = await loginResponse.Content.ReadFromJsonAsync<LoginUserResponse>();
        return loginData!.AccessToken;
    }

    private static string RandomId => Guid.NewGuid().ToString("N")[..8];

    private static string BuildEmail(string organizationName) => $"{RandomId}@{organizationName}.com";
}
