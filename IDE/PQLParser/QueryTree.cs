namespace IDE.PQLParser;

public class QueryTree
{
    public string Name { get; }
    public string NodeType { get; }
    public string? Attribute { get; }
    public List<QueryTree> Children { get; } = new();

    public QueryTree(string name, string nodeType, string? attr = null)
    {
        this.Name = name;
        this.NodeType = nodeType;
        this.Attribute = attr;
    }

    override public String ToString() {
        return this.Name + ":" + this.NodeType;
    }
    public void AddNode(QueryTree childNode)
    {
        Children.Add(childNode);
    }
    public void AddNodes(List<QueryTree> nodes)
    {
        foreach (var node in nodes)
        {
            Children.Add(node);
        }
    }

}
