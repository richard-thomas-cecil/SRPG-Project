using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private Queue<Node> queue = new Queue<Node>();
    public List<Node> graphNodes = new List<Node>();
    public List<GameObject> targetsInRange = new List<GameObject>();
    private int maxNodes;
    public Node startNode;

    public int[,] adjacencyMatrix;

    private int currentNodeIndex = 0;

    public Graph()
    {
    }

    public void BuildGraph(GameObject startingTile)
    {
        maxNodes = GameObject.Find("MapInfo").GetComponentsInChildren<TileInfo>().Length;

        adjacencyMatrix = new int[maxNodes, maxNodes];
        graphNodes.Clear();
        currentNodeIndex = 0;


        startNode = AddNode(startingTile, 9999, 0, 0);
    }


    public void BuildGraph(GameObject startingTile, int moveableDistance, int minRange, int maxRange)
    {
        maxNodes = GameObject.Find("MapInfo").GetComponentsInChildren<TileInfo>().Length;

        adjacencyMatrix = new int[maxNodes, maxNodes];
        graphNodes.Clear();
        currentNodeIndex = 0;


        startNode = AddNode(startingTile, moveableDistance, minRange, maxRange);
    }

    private Node AddNode(GameObject thisTile, int moveableDistance, int minRange, int maxRange)
    {
        Node newTile = new Node(thisTile, currentNodeIndex);
        newTile.MarkVisited();
        newTile.colorType = COLOR_TYPE.MOVEMENT;
        //AddRangeTiles(newTile, thisTile, minRange, maxRange, 0);
        graphNodes.Add(newTile);

        currentNodeIndex++;

        

        if (newTile.tile.GetComponent<TileInfo>().eastTile != null)
        {
            GameObject nextTile = newTile.tile.GetComponent<TileInfo>().eastTile;
            Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            if(adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            {
                graphNodes.Remove(adjacentNode);
                adjacentNode = null;
            }
            if (adjacentNode == null)
            {
                if (adjacencyWeight <= moveableDistance)
                {
                    adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
                }
                else if (moveableDistance < adjacencyWeight)
                {
                    adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
                    adjacencyWeight = 1;
                }
            }
            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacencyWeight);

        }
        if (newTile.tile.GetComponent<TileInfo>().westTile != null )
        {
            GameObject nextTile = newTile.tile.GetComponent<TileInfo>().westTile;
            Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            {
                graphNodes.Remove(adjacentNode);
                adjacentNode = null;
            }
            if (adjacentNode == null)
            {
                if (adjacencyWeight <= moveableDistance)
                {
                    adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
                }
                else
                {
                    adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
                    adjacencyWeight = 1;
                }
            }
            
            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacencyWeight);

        }
        if (newTile.tile.GetComponent<TileInfo>().northTile != null)
        {
            GameObject nextTile = newTile.tile.GetComponent<TileInfo>().northTile;
            Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            {
                graphNodes.Remove(adjacentNode);
                adjacentNode = null;
            }
            if (adjacentNode == null)
            {
                if (adjacencyWeight <= moveableDistance)
                {
                    adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
                }
                else
                {
                    adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
                    adjacencyWeight = 1;
                }
            }
            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacencyWeight);

        }
        if (newTile.tile.GetComponent<TileInfo>().southTile != null)
        {
            GameObject nextTile = newTile.tile.GetComponent<TileInfo>().southTile;
            Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            {
                graphNodes.Remove(adjacentNode);
                adjacentNode = null;
            }
            if (adjacentNode == null || adjacentNode.colorType == COLOR_TYPE.ATTACK)
            {
                if (adjacencyWeight <= moveableDistance)
                {
                    adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
                }
                else
                {
                    adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
                    adjacencyWeight = 1;
                }
            }
            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacencyWeight);

        }

        return newTile;
    }

    private void AddEdge(int vertexA, int vertexB, int distance)
    {
        if (vertexA >= 0 && vertexB >= 0 && vertexA < maxNodes && vertexB < maxNodes)
        {
            adjacencyMatrix[vertexA, vertexB] = distance;
        }
    }

    private Node AddRangeColorNode(GameObject thisTile, int minRange, int maxRange, int currentRange, COLOR_TYPE color)
    {
        Node newTile = new Node(thisTile, currentNodeIndex);
        newTile.MarkVisited();
        newTile.colorType = COLOR_TYPE.ATTACK;
        graphNodes.Add(newTile);

        if(currentRange >= minRange && currentRange < maxRange)
        {
                if (newTile.tile.GetComponent<TileInfo>().eastTile != null)
                {
                    GameObject nextTile = newTile.tile.GetComponent<TileInfo>().eastTile;
                    Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
                    if (adjacentNode == null)
                    {
                        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, currentRange + 1, COLOR_TYPE.ATTACK);
                    }
                    if (adjacentNode != null)
                        AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, 1);

                }
                if (newTile.tile.GetComponent<TileInfo>().westTile != null)
                {
                    GameObject nextTile = newTile.tile.GetComponent<TileInfo>().westTile;
                    Node adjacentNode = graphNodes.Find(x => x.tile == newTile.tile.GetComponent<TileInfo>().westTile);
                    if (adjacentNode == null)
                        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, currentRange + 1, COLOR_TYPE.ATTACK);
                    if (adjacentNode != null)
                        AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, 1);

                }
                if (newTile.tile.GetComponent<TileInfo>().northTile != null)
                {
                    GameObject nextTile = newTile.tile.GetComponent<TileInfo>().northTile;
                    Node adjacentNode = graphNodes.Find(x => x.tile == newTile.tile.GetComponent<TileInfo>().northTile);
                    if (adjacentNode == null)
                        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, currentRange + 1, COLOR_TYPE.ATTACK);
                    if (adjacentNode != null)
                        AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, 1);

                }
                if (newTile.tile.GetComponent<TileInfo>().southTile != null)
                {
                    GameObject nextTile = newTile.tile.GetComponent<TileInfo>().southTile;
                    Node adjacentNode = graphNodes.Find(x => x.tile == newTile.tile.GetComponent<TileInfo>().southTile);
                    if (adjacentNode == null)
                        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, currentRange + 1, COLOR_TYPE.ATTACK);
                    if (adjacentNode != null)
                        AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, 1);

                }
        }

        return newTile;
    }

    public Graph BuildSubGraph(int root, int moveableDistance, int minRange, int maxRange)
    {
        Graph subGraph = new Graph();
        Queue<Node> queue = new Queue<Node>();
        bool[] visited = new bool[maxNodes];
        int[] distanceFromStart = new int[maxNodes];
        COLOR_TYPE[] nodeColor = new COLOR_TYPE[maxNodes];

        Node currentNodeBase = graphNodes.Find(x => x.nodeIndex == root);

        Node currentNode = new Node(currentNodeBase.tile, currentNodeBase.nodeIndex);

        for(int i = 0; i < maxNodes; i++)
        {
            visited[i] = false;
            nodeColor[i] = COLOR_TYPE.NONE;
            distanceFromStart[i] = 0;
        }

        queue.Enqueue(currentNode);
        visited[root] = true;
        distanceFromStart[root] = 0;

        while(queue.Count > 0){
            currentNodeBase = queue.Dequeue();

            int currentIndex = currentNodeBase.nodeIndex;

            currentNode = new Node(currentNodeBase.tile, currentIndex);

            if (distanceFromStart[currentIndex] <= moveableDistance + maxRange)
            {
                for (int v = 0; v < maxNodes; v++)
                {
                    if (adjacencyMatrix[currentIndex, v] != 0)
                    {
                        if (!visited[v])
                            queue.Enqueue(graphNodes.Find(x => x.nodeIndex == v));
                        visited[v] = true;
                        distanceFromStart[v] = FindMinDistance(distanceFromStart[v], distanceFromStart[currentIndex] + adjacencyMatrix[currentIndex, v]);
                        if (distanceFromStart[v] > moveableDistance)
                        {
                            if (distanceFromStart[currentIndex] <= moveableDistance)
                            {
                                distanceFromStart[v] = FindMinDistance(distanceFromStart[v], moveableDistance + 1);
                            }
                            else if (distanceFromStart[currentIndex] > moveableDistance)
                            {
                                distanceFromStart[v] = FindMinDistance(distanceFromStart[v], distanceFromStart[currentIndex] + 1);
                            }
                            
                        }
                        
                        if (distanceFromStart[v] >= minRange + moveableDistance && distanceFromStart[v] <= maxRange + moveableDistance && nodeColor[v] != COLOR_TYPE.MOVEMENT)
                        {
                            nodeColor[v] = COLOR_TYPE.ATTACK;
                        }
                        else if(distanceFromStart[v] <= moveableDistance)
                            nodeColor[v] = COLOR_TYPE.MOVEMENT;

                    }

                }
                subGraph.graphNodes.Add(currentNode);
            }
            
        }
        foreach(var node in subGraph.graphNodes)
        {
            node.colorType = nodeColor[node.nodeIndex];
        }

        return subGraph;
    }

    private int FindMinDistance(int currentDistance, int nextDistance)
    {
        int minDistance = currentDistance;
        if(minDistance == 0 || minDistance >= nextDistance)
        {
            minDistance = nextDistance;
        }

        return minDistance;
    }
    public void Display()
    {
        Debug.Log("***********Adjacency Matrix Representation***********");
        Debug.Log("Number of nodes: {0}\n" + " " + (maxNodes - 1));
        foreach (Node n in graphNodes)
        {
            Debug.Log("{0}\t" +  n.tile);
        }
        Debug.Log("");//newline for the graph display
        for (int i = 0; i < maxNodes; i++)
        {
            Debug.Log("{0}\t" + " " + graphNodes[adjacencyMatrix[i, 0]].tile);
            for (int j = 0; j < maxNodes; j++)
            {
                Debug.Log(graphNodes[i].tile + " distance to  "+ graphNodes[j].tile + ": " + adjacencyMatrix[i, j]);
            }
            Debug.Log("");
            Debug.Log("");
        }
        Console.WriteLine("Read the graph from left to right");
    }
}
