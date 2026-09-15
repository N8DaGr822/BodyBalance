using System.Net.Http.Json;
using System.Text.Json;
using BodyBalancingApp.Models;

namespace BodyBalancingApp.Services;

public class BodyBalancingDataService
{
    private readonly HttpClient _http;
    private List<TreeNode>? _categories;

    public BodyBalancingDataService(HttpClient http)
    {
        _http = http;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Loads (and caches) the Body Balancing category tree from wwwroot/data/body-balancing.json.</summary>
    public async Task<List<TreeNode>> GetCategoriesAsync()
    {
        if (_categories is not null)
        {
            return _categories;
        }

        var data = await _http.GetFromJsonAsync<List<TreeNode>>("data/body-balancing.json", JsonOptions);
        _categories = data ?? new List<TreeNode>();
        return _categories;
    }

    /// <summary>
    /// Walks the tree following the given slug segments, returning the ordered chain of nodes
    /// visited (the "flow" trail) plus the list of choices available at the final step.
    /// Returns null if any segment along the path could not be resolved.
    /// </summary>
    public async Task<(List<TreeNode> Trail, List<TreeNode> Choices)?> ResolvePathAsync(IEnumerable<string> segments)
    {
        var categories = await GetCategoriesAsync();
        var trail = new List<TreeNode>();
        var currentLevel = categories;

        foreach (var segment in segments)
        {
            var match = currentLevel.FirstOrDefault(n => n.Id == segment);
            if (match is null)
            {
                return null;
            }

            trail.Add(match);
            currentLevel = match.Children;
        }

        return (trail, currentLevel);
    }
}
