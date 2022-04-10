using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.Kendra;
using Amazon.Kendra.Model;

namespace hello_kendra.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    const string INDEX_ID = "[Index-ID]";

    private RegionEndpoint region = RegionEndpoint.USWest2;

    private readonly ILogger<SearchController> _logger;
    private AmazonKendraClient client;

    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;
        client = new AmazonKendraClient(region);
    }

    [HttpGet]
    public async Task<IEnumerable<SearchResult>> Get([FromQuery] string term)
    {
        var request = new QueryRequest()
        {
            IndexId = INDEX_ID,
            QueryText = term
        };

        var response = await client.QueryAsync(request);

        return response.ResultItems.Select(result => new SearchResult
        {
            Title = result.DocumentTitle.Text,
            Document = result.DocumentURI,
            Excerpt = result.DocumentExcerpt.Text
        })
        .ToArray();
    }
}
