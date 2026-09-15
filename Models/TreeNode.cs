namespace BodyBalancingApp.Models;

public class TreeNode
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Image { get; set; }
    public string? Description { get; set; }
    public bool ComingSoon { get; set; }
    public List<TreeNode> Children { get; set; } = new();

    public bool IsLeaf => Children.Count == 0;
}
