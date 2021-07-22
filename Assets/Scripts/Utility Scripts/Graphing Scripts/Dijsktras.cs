using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Performs Dijkstras algorithm to find the shortest path for a graph. Used for unit movement primaraly
public class Dijsktras
{

    public int[] dist;
    public bool[] visited;
    public int[] parent;        //used to find path of shortest distance

    private Graph _graph;
    private int graphSize;

    //Constructor. The graph that it akes
    public Dijsktras(Graph graph)
    {
        _graph = graph;
        graphSize = _graph.graphNodes.Count;
    }

    private int FindMinDistance()
    {
        int min = int.MaxValue;
        int minIndex = -1;

        for(int i = 0; i < graphSize; i++)
        {
            if(!visited[i] && dist[i] <= min)
            {
                min = dist[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public void ReplaceGraph(Graph newGraph)
    {
        _graph = newGraph;
        graphSize = newGraph.graphNodes.Count;
    }

    public void DijsktrasAlogrithm(int source)
    {
        dist = new int[graphSize];
        visited = new bool[graphSize];
        parent = new int[graphSize];

        for(int i = 0; i< graphSize; i++)
        {
            dist[i] = int.MaxValue;
            visited[i] = false;
        }

        dist[source] = 0;
        parent[source] = -1;

        for(int currentNode = 0; currentNode < graphSize - 1; currentNode++)
        {
            int u = FindMinDistance();

            visited[u] = true;

            for(int v = 0; v < graphSize; v++)
            {
                if (!visited[v] && _graph.adjacencyMatrix[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + _graph.adjacencyMatrix[u,v] < dist[v])
                {
                    dist[v] = dist[u] + _graph.adjacencyMatrix[u, v];
                    parent[v] = u;
                }
            }
        }
    }
}
