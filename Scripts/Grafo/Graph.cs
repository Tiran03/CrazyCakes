using System.Collections.Generic;
using UnityEngine;

public class Graph<T> : IGraph<T>
{
    private Dictionary<T, List<T>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<T, List<T>>();
    }

    public void AddNode(T node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<T>();
        }
    }

    public void AddEdge(T fromNode, T toNode)
    {
        if (!adjacencyList.ContainsKey(fromNode))
        {
            Debug.LogError($"Error: Node {fromNode} does not exist in the graph.");
            return;
        }

        if (!adjacencyList.ContainsKey(toNode))
        {
            Debug.LogError($"Error: Node {toNode} does not exist in the graph.");
            return;
        }

        adjacencyList[fromNode].Add(toNode);
        adjacencyList[toNode].Add(fromNode); // Para un grafo no dirigido
    }

    public List<T> GetNodes()
    {
        return new List<T>(adjacencyList.Keys);
    }

    public List<T> GetNeighbors(T node)
    {
        if (adjacencyList.ContainsKey(node))
        {
            return new List<T>(adjacencyList[node]);
        }
        return new List<T>();
    }

    public bool HasEdge(T fromNode, T toNode)
    {
        return adjacencyList.ContainsKey(fromNode) && adjacencyList[fromNode].Contains(toNode);
    }

    public List<T> DepthFirstSearch(T startNode)
    {
        HashSet<T> visited = new HashSet<T>();
        List<T> result = new List<T>();
        DFS(startNode, visited, result);
        return result;
    }

    public List<T> BreadthFirstSearch(T startNode)
    {
        HashSet<T> visited = new HashSet<T>();
        List<T> result = new List<T>();
        Queue<T> queue = new Queue<T>();

        visited.Add(startNode);
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            T currentNode = queue.Dequeue();
            result.Add(currentNode);

            foreach (T neighbor in adjacencyList[currentNode])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return result;
    }

    private void DFS(T currentNode, HashSet<T> visited, List<T> result)
    {
        visited.Add(currentNode);
        result.Add(currentNode);

        foreach (T neighbor in adjacencyList[currentNode])
        {
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, visited, result);
            }
        }
    }
}

