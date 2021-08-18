using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Graph Class for use during battle. Used to find things in range of a unit, shortest paths, etc
public class Graph
{
    
    public List<Node> graphNodes = new List<Node>();                    //The list of nodes that makeup the graph
    private int maxNodes;                                               //Max number of nodes. Will be equal to the number of tiles on map
    public Node startNode;                                              //Where to start building the graph from.

    public int[,] adjacencyMatrix;                                      //Adjacency Matrix

    private int currentNodeIndex;                                   //Each node and tile will be assigned an index, starting from 0

    public Graph()
    {
    }

    //Initialize and start building the graph
    public void BuildGraph(GameObject startingTile)
    {
        maxNodes = GameObject.Find("MapInfo").GetComponentsInChildren<TileInfo>().Length;       //GetNumber of tiles on map

        adjacencyMatrix = new int[maxNodes, maxNodes];
        graphNodes.Clear();                                                                     //Clear out any nodes if they exsist
        currentNodeIndex = 0;


        startNode = AddNode(startingTile.GetComponent<TileInfo>(), 9999, 0, 0);
    }

    //This method may not be used at any point anymore
    public void BuildGraph(GameObject startingTile, int moveableDistance, int minRange, int maxRange)
    {
        maxNodes = GameObject.Find("MapInfo").GetComponentsInChildren<TileInfo>().Length;

        adjacencyMatrix = new int[maxNodes, maxNodes];
        graphNodes.Clear();
        currentNodeIndex = 0;


        startNode = AddNode(startingTile.GetComponent<TileInfo>(), moveableDistance, minRange, maxRange);
    }

    //Method to add node. Is called recursively until all nodes are visited
    private Node AddNode(TileInfo thisTile, int moveableDistance, int minRange, int maxRange)
    {
        //Creates a new node and marks it as visited, then adds it to the node list and increments the Node index
        Node newTile = new Node(thisTile, currentNodeIndex);
        newTile.MarkVisited();
        newTile.colorType = COLOR_TYPE.NONE;
        //AddRangeTiles(newTile, thisTile, minRange, maxRange, 0);
        graphNodes.Add(newTile);

        currentNodeIndex++;

        
        //Next, check each neighboring tile to see if there is a tile to visit, then evaluate whether it needs to be added to the graph list.
        //See EvaluateNextTile for more information
        if (newTile.tile.eastTile != null)
        {
            //    TileInfo nextTile = newTile.tile.eastTile.GetComponent<TileInfo>();
            //    Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            //    int adjacencyWeight = nextTile.movementCost;
            //    if(adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            //    {
            //        graphNodes.Remove(adjacentNode);
            //        adjacentNode = null;
            //    }
            //    if (adjacentNode == null)
            //    {
            //        if (adjacencyWeight <= moveableDistance)
            //        {
            //            adjacentNode = AddNode(nextTile.gameObject, moveableDistance - adjacencyWeight, minRange, maxRange);
            //        }
            //        else if (moveableDistance < adjacencyWeight)
            //        {
            //            adjacentNode = AddRangeColorNode(nextTile.gameObject, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
            //            adjacencyWeight = 1;
            //        }
            //    }
            Node adjacentNode = EvaluateNextTile(newTile.tile.eastTile.GetComponent<TileInfo>(), moveableDistance, minRange, maxRange);


            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacentNode.tile.movementCost);

        }
        if (newTile.tile.GetComponent<TileInfo>().westTile != null )
        {
            //GameObject nextTile = newTile.tile.GetComponent<TileInfo>().westTile;
            //Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            //int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            //if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            //{
            //    graphNodes.Remove(adjacentNode);
            //    adjacentNode = null;
            //}
            //if (adjacentNode == null)
            //{
            //    if (adjacencyWeight <= moveableDistance)
            //    {
            //        adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
            //    }
            //    else
            //    {
            //        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
            //        adjacencyWeight = 1;
            //    }
            //}

            Node adjacentNode = EvaluateNextTile(newTile.tile.westTile.GetComponent<TileInfo>(), moveableDistance, minRange, maxRange);

            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacentNode.tile.movementCost);

        }
        if (newTile.tile.GetComponent<TileInfo>().northTile != null)
        {
            //GameObject nextTile = newTile.tile.GetComponent<TileInfo>().northTile;
            //Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            //int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            //if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            //{
            //    graphNodes.Remove(adjacentNode);
            //    adjacentNode = null;
            //}
            //if (adjacentNode == null)
            //{
            //    if (adjacencyWeight <= moveableDistance)
            //    {
            //        adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
            //    }
            //    else
            //    {
            //        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
            //        adjacencyWeight = 1;
            //    }
            //}
            Node adjacentNode = EvaluateNextTile(newTile.tile.northTile.GetComponent<TileInfo>(), moveableDistance, minRange, maxRange);

            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacentNode.tile.movementCost);

        }
        if (newTile.tile.GetComponent<TileInfo>().southTile != null)
        {
            //GameObject nextTile = newTile.tile.GetComponent<TileInfo>().southTile;
            //Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
            //int adjacencyWeight = nextTile.GetComponent<TileInfo>().movementCost;
            //if (adjacentNode != null && adjacentNode.colorType == COLOR_TYPE.ATTACK)
            //{
            //    graphNodes.Remove(adjacentNode);
            //    adjacentNode = null;
            //}
            //if (adjacentNode == null || adjacentNode.colorType == COLOR_TYPE.ATTACK)
            //{
            //    if (adjacencyWeight <= moveableDistance)
            //    {
            //        adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
            //    }
            //    else
            //    {
            //        adjacentNode = AddRangeColorNode(nextTile, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
            //        adjacencyWeight = 1;
            //    }
            //}
            Node adjacentNode = EvaluateNextTile(newTile.tile.southTile.GetComponent<TileInfo>(), moveableDistance, minRange, maxRange);
            if (adjacentNode != null)
                AddEdge(newTile.nodeIndex, adjacentNode.nodeIndex, adjacentNode.tile.movementCost);

        }

        return newTile;
    }

    //Evaluates a tile to see if it needs to be added to the node list. First, we check to see if the current tile is in the list of nodes already.
    //Then, if it it is not, we call AddNode to add it to the list, and return it to our adjacent Node. Finally we return adjacentNode so it may be added to the adjacencyMatrix.
    //If it the node is already in the graph, we simply return it so it may be added to the adjacency Matrix
    private Node EvaluateNextTile(TileInfo nextTile, int moveableDistance, int minRange, int maxRange)
    {
        Node adjacentNode = graphNodes.Find(x => x.tile == nextTile);
        int adjacencyWeight = nextTile.movementCost;
        if (adjacentNode == null)
        {
            if (adjacencyWeight <= moveableDistance)
            {
                adjacentNode = AddNode(nextTile, moveableDistance - adjacencyWeight, minRange, maxRange);
            }
            else if (moveableDistance < adjacencyWeight)
            {
                adjacentNode = AddRangeColorNode(nextTile.gameObject, minRange, maxRange, 1, COLOR_TYPE.ATTACK);
                adjacencyWeight = 1;
            }
        }
        return adjacentNode;
    }

    private void AddEdge(int vertexA, int vertexB, int distance)
    {
        if (vertexA >= 0 && vertexB >= 0 && vertexA < maxNodes && vertexB < maxNodes)
        {
            adjacencyMatrix[vertexA, vertexB] = distance;
        }
    }

    //No longer in use
    private Node AddRangeColorNode(GameObject thisTile, int minRange, int maxRange, int currentRange, COLOR_TYPE color)
    {
        Node newTile = new Node(thisTile.GetComponent<TileInfo>(), currentNodeIndex);
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

    //Builds a subGraph based off of the adjacency matrix using Breadth-first search
    public Graph BuildSubGraph(int root, int moveableDistance, int minRange, int maxRange, CharacterInfo currentCharacter)
    {
        Graph subGraph = new Graph();                                                   //New graph object
        Queue<Node> queue = new Queue<Node>();                                          //Queue for BFS
        bool[] visited = new bool[maxNodes];                                            //Markvisited for BFS
        int[] distanceFromStart = new int[maxNodes];                                    //Distance from start keeps track of how far a node is away from the start(ie. the current tile of the unit), so that we know when to stop searching
        COLOR_TYPE[] nodeColor = new COLOR_TYPE[maxNodes];                              //Keeps track of how to color nodes based on distance from start

        bool[] markAsInRange = new bool[maxNodes];

        Node currentNodeBase = graphNodes.Find(x => x.nodeIndex == root);               //Find the node in the main graph that corresponds with the root tileIndex, and set it as the first node

        Node currentNode;                                                               //Designates the current Node being evaluated to add to subgraph                  

        //Set max nodes and intialize adjacency matrix for subgraph
        subGraph.maxNodes = maxNodes;                                                   
        subGraph.adjacencyMatrix = new int[maxNodes, maxNodes];

        //Initialize various arrays
        for(int i = 0; i < maxNodes; i++)
        {
            visited[i] = false;
            nodeColor[i] = COLOR_TYPE.NONE;
            distanceFromStart[i] = -1;
        }

        //Queue up the root node, then set it to visited with a distance of 0
        queue.Enqueue(currentNodeBase);
        visited[root] = true;
        distanceFromStart[root] = 0;
        nodeColor[root] = COLOR_TYPE.MOVEMENT;

        //While loop for BFS
        while(queue.Count > 0){
            currentNodeBase = queue.Dequeue();                                  //Dequeue to get the next node to evaluate (Note: redundant for first loop)

            int currentIndex = currentNodeBase.nodeIndex;                       //Get the index for the x value in AdjacencyMatrix[x,y]

            currentNode = new Node(currentNodeBase.tile, currentIndex);         //Create a new node using information from currentNodeBase


            //If the current Index has not previously been determined to be outside of the characters range (defined by moveableDistance + maxRange), then
            //proceed to evaluate its neighboring nodes, add them to the queue if needed, and finally add it to the subgraph
            if (distanceFromStart[currentIndex] <= moveableDistance + maxRange)
            {
                for (int v = 0; v < maxNodes; v++)
                {
                    if (adjacencyMatrix[currentIndex, v] != 0)
                    {
                        subGraph.adjacencyMatrix[currentIndex, v] = adjacencyMatrix[currentIndex, v];
                        //If tile of index v has not previously been visited, add it to the queue
                        if (!visited[v])
                            queue.Enqueue(graphNodes.Find(x => x.nodeIndex == v));
                        visited[v] = true;

                        TileInfo tileV = graphNodes.Find(x => x.nodeIndex == v).tile;

                        if (currentIndex == 12 && v == 11)
                        {
                            Debug.Log("");
                        }

                        //Due to priority of Node colors, we need to check and see if the current distance of tile of index v is greater than the current node + its distance to the next node

                        distanceFromStart[v] = FindMinDistance(distanceFromStart[v], distanceFromStart[currentIndex] + adjacencyMatrix[currentIndex, v]);


                        if (tileV.occupant != null && nodeColor[v] == COLOR_TYPE.MOVEMENT && EvaluateIsEnemy(currentCharacter, tileV.occupant))
                        {
                            distanceFromStart[v] = moveableDistance + 1;
                        }

                        int addRange = 0;

                        if (graphNodes[currentIndex].tile.isOccupied && currentIndex !=root && !currentCharacter.EvaluateIsEnemy(graphNodes[currentIndex].tile.occupant) && distanceFromStart[currentIndex] <= moveableDistance && nodeColor[v] == COLOR_TYPE.NONE)
                        {
                            addRange = 1;
                        }
                        
                        //If the tile of index v is evaluated as farther from the root than the unit can move, we need to then check to see if it is within the units max range
                        //by finding the minimum distance between itself and either 1 away from the max moveable distance if the currentNode has a distance <= the movement of the unit,
                        //or one tile further from the current Index if the current Node is farther away from the moveable distance.
                        //This ensures that all tiles are properly evaluated as being in or out of units range
                        if (distanceFromStart[v]> moveableDistance + addRange && distanceFromStart[currentIndex] + addRange < moveableDistance + maxRange && !tileV.isOccupied /*&& (!graphNodes[currentIndex].tile.isOccupied || currentIndex == root)*/)
                        {
                            if (distanceFromStart[v] <= moveableDistance)
                            {
                                nodeColor[v] = COLOR_TYPE.MOVEMENT;
                            }
                            if (distanceFromStart[currentIndex] + addRange <= moveableDistance)
                            {
                                distanceFromStart[v] = FindMinDistanceInRange(distanceFromStart[v] + addRange, moveableDistance + 1 + addRange, moveableDistance + minRange, moveableDistance + maxRange);
                                if (distanceFromStart[v] <= moveableDistance + maxRange && (nodeColor[currentIndex] == COLOR_TYPE.MOVEMENT || markAsInRange[currentIndex]))
                                    markAsInRange[v] = true;
                            }
                            else if (distanceFromStart[currentIndex] + addRange > moveableDistance)
                            {
                                distanceFromStart[v] = FindMinDistanceInRange(distanceFromStart[v] + addRange, distanceFromStart[currentIndex] + 1 + addRange, moveableDistance + minRange, moveableDistance + maxRange);
                                if(distanceFromStart[v] <= moveableDistance + maxRange && (nodeColor[currentIndex] == COLOR_TYPE.MOVEMENT || markAsInRange[currentIndex]))
                                    markAsInRange[v] = true;
                            }

                            
                        }


                        //Check to see if a node is in range of another node and color it as attack

                        if (markAsInRange[v] && nodeColor[v] != COLOR_TYPE.MOVEMENT)
                        {
                            nodeColor[v] = COLOR_TYPE.ATTACK;
                        }

                        //If tile is in moveable range, color it as movement. Movement color overrides attack color                   
                        if (distanceFromStart[v] <= moveableDistance)
                        {
                            nodeColor[v] = COLOR_TYPE.MOVEMENT;
                            
                        }
                        
                    }

                }
                subGraph.graphNodes.Add(currentNode);
            }
            
        }
        //Correct color for each node
        foreach(var node in subGraph.graphNodes)
        {
            node.colorType = nodeColor[node.nodeIndex];
            node.weight = distanceFromStart[node.nodeIndex];
        }


        return subGraph;
    }

    private int FindMinDistance(int currentDistance, int nextDistance)
    {
        int minDistance = currentDistance;
        if(minDistance == -1 || minDistance >= nextDistance)
        {
            minDistance = nextDistance;
        }

        return minDistance;
    }

    private int FindMinDistanceInRange(int currentDistance, int nextDistance, int minRange, int maxRange)
    {
        int minDistance = currentDistance;
        if(((minDistance == -1 || minDistance >= nextDistance) && (nextDistance >= minRange) || minDistance < minRange))
        {
            minDistance = nextDistance;
        }

        return minDistance;
    }    

    public void RecolorGraph(int moveableDistance, int minRange, int maxRange, CharacterInfo currentCharacter)
    {
        bool[] markAsInRange = new bool[maxNodes];

        for(int i = 0; i < maxNodes; i++)
        {
            markAsInRange[i] = false;
        }

        foreach(var node in graphNodes)
        {
                for (int v = 0; v < graphNodes.Count; v++)
                {
                if (graphNodes[v].nodeIndex == 5)
                    Debug.Log(node.nodeIndex + " ");
                if ((graphNodes[v].weight > moveableDistance) && adjacencyMatrix[node.nodeIndex, graphNodes[v].nodeIndex] != 0 && (node.colorType == COLOR_TYPE.MOVEMENT || markAsInRange[graphNodes[v].nodeIndex]) && (!node.tile.isOccupied || node.tile.occupant == currentCharacter))
                    {
                        
                        markAsInRange[graphNodes[v].nodeIndex] = true;
                    }
                }
        }

        for(int i = 0; i < graphNodes.Count; i++)
        {
            if (markAsInRange[graphNodes[i].nodeIndex])
                graphNodes[i].colorType = COLOR_TYPE.ATTACK;
            else if (graphNodes[i].weight <= moveableDistance)
                graphNodes[i].colorType = COLOR_TYPE.MOVEMENT;
            else
                graphNodes[i].colorType = COLOR_TYPE.NONE;
        }

    }

    //Returns true if toEvaluate is considered an "enemy" of thisCharacter
    private bool EvaluateIsEnemy(CharacterInfo thisCharacter, CharacterInfo toEvaluate)
    {
        switch (thisCharacter.characterData.characterType)
        {
            case CHARACTER_TYPE.PLAYER:
                if (toEvaluate.characterData.characterType == CHARACTER_TYPE.PLAYER)
                    return false;
                else
                    return true;
            case CHARACTER_TYPE.ENEMY:
                if (toEvaluate.characterData.characterType == CHARACTER_TYPE.ENEMY)
                    return false;
                else
                    return true;
            default:
                return true;
        }
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
