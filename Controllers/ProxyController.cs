using System.Web;
using Microsoft.AspNetCore.Mvc;
using AasCore.Aas3_0;
using hackathon.Utils;
using IO.Swagger.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using hackathon.Import;
using System.Text.Json.Nodes;

namespace hackathon.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProxyController : ControllerBase
{
    private readonly ImportService _importService;

    public ProxyController(ImportService importService)
    {
        _importService = importService;
    }

    [HttpGet]
    public async Task<object> Shells(string registryUrl)
    {
        var decodedUrl = HttpUtility.UrlDecode(registryUrl);

        var client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(decodedUrl + "/shells");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return await Task.FromResult(responseBody);
    }

    [HttpGet]
    public async Task<object> Registry(string registryUrl)
    {
        var decodedUrl = HttpUtility.UrlDecode(registryUrl);

        var client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(decodedUrl + "/shell-descriptors");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return await Task.FromResult(responseBody);
    }

    [HttpGet]
    public async Task<object> Shell(string registryUrl, string? aasId)
    {
        var decodedUrl = HttpUtility.UrlDecode(registryUrl);
        var decodedId = HttpUtility.UrlDecode(aasId) ?? string.Empty;

        var client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(decodedUrl  + $"/shells/{decodedId?.ToBase64()}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return await Task.FromResult(responseBody);
    }

    [HttpGet]
    public async Task<object> Discovery(string registryUrl, string? assetId)
    {
        var decodedUrl = HttpUtility.UrlDecode(registryUrl);

        var client = new HttpClient();
        var url = decodedUrl + $"/lookup/shells?assetIds={assetId?.ToBase64()}";
        using HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return await Task.FromResult(responseBody);
    }


    [HttpGet]
    public async Task<bool> Import(string localRegistryUrl, string remoteRegistryUrl, string id)
    {
        var decodedLocalUrl = HttpUtility.UrlDecode(localRegistryUrl);
        var decodedRemoteUrl = HttpUtility.UrlDecode(remoteRegistryUrl);
        var decodedId = HttpUtility.UrlDecode(id);

        _importService.ImportFromRepository(decodedLocalUrl, decodedRemoteUrl, decodedId);


        return await Task.FromResult(true);
    }



}
