using System;
using IDE.QueryParser;

namespace IDE.PQLParser;

public class QueryTree
{
    public string NodeType { get; }
    public string Name { get; }
    public List<QueryTree> Children { get; } = new();
    
    public QueryTree(string name, string nodeType)
    {
        this.Name = name;
        this.NodeType = nodeType;
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
