using System;
using IDE.QueryParser;

namespace IDE.PQLParser;

public class QueryTree
{
    public string Name { get; }
    public string NodeType { get; }
    public List<QueryTree> Children { get; } = new();
    
    public QueryTree(string name, string nodeType)
    {
        this.Name = name;
        this.NodeType = nodeType;
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
