using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlCatalog.Pages;

#pragma warning disable CS1591
public static class TreeEx
{
    public static IEnumerable<TNode> Dfs<TNode>(TNode root, Func<TNode, IEnumerable<TNode>> getChildren)
    {
        yield return root;

        var children = getChildren(root);
        if (children == null) 
            yield break;
        
        foreach (var child in children)
            foreach (var descendant in Dfs(child, getChildren))
                yield return descendant;
    }

    public static IEnumerable<TNode> Bfs<TNode>(TNode root, Func<TNode, IEnumerable<TNode>> getChildren)
    {
        return Bfs(new[] { root }, getChildren);
    }

    private static IEnumerable<TNode> Bfs<TNode>(IEnumerable<TNode> levelNodes, Func<TNode, IEnumerable<TNode>> getChildren)
    {
        if (!levelNodes.Any())
            yield break;

        foreach (var node in levelNodes)
            yield return node;

        var nextLevelNodes = levelNodes.SelectMany(getChildren);
        foreach (var node in Bfs(nextLevelNodes, getChildren))
            yield return node;
    }
}
