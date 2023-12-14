using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    private IGraph<WayPoint> graph;
    public WayPoint startNode; 
    public WayPoint endNode;  
    public PlayerController player;

    void Start()
    {
        graph = new Graph<WayPoint>();

        WayPoint[] allWayPoints = FindObjectsOfType<WayPoint>();

        foreach (WayPoint node in allWayPoints)
        {
            graph.AddNode(node);
        }

        foreach (WayPoint node in allWayPoints)
        {
            foreach (WayPoint connectedNode in node.GetWaypoints())
            {
                graph.AddEdge(node, connectedNode);
            }
        }

        // Usar el algoritmo de Dijkstra para encontrar el camino más corto
        List<WayPoint> shortestPath = Dijkstra(graph, startNode, endNode);

        

        for (int i = 1; i < shortestPath.Count; i++)
        {
            player.MoveToNode(shortestPath[i]);
        }
        
        //player.MoveToNode(startNode);
    }

    void Update()
    {
        // Detectar clics del mouse en los nodos
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            Debug.Log("Click");

            if (hit.collider != null && hit.collider.CompareTag("WayPoint"))
            {
                WayPoint selectedNode = hit.collider.GetComponent<WayPoint>();

                // Verificar si el nodo seleccionado activa la derrota y no se puede volver atrás
                if (selectedNode.activatesDefeat && !CanGoBackward(selectedNode))
                {
                    Debug.Log("¡Derrota activada!");
                    LoadingManager.Instance.LoadScene(8, 10);
                }

                // Verificar si el nodo seleccionado es el nodo final (endNode)
                else if (selectedNode == endNode)
                {
                    Debug.Log("¡Ganaste!");
                    LoadingManager.Instance.LoadScene(8, 9);
                }

                // Mover al jugador al nodo seleccionado
                player.MoveToNode(selectedNode);
            }
        }
    }




   

    public List<WayPoint> Dijkstra(IGraph<WayPoint> graph, WayPoint startNode, WayPoint endNode)

    {
        Dictionary<WayPoint, float> distance = new Dictionary<WayPoint, float>();
        Dictionary<WayPoint, WayPoint> previous = new Dictionary<WayPoint, WayPoint>();
        List<WayPoint> unvisited = new List<WayPoint>();

        // Obtener todos los nodos del grafo
        List<WayPoint> nodes = graph.GetNodes();

        foreach (WayPoint node in nodes)
        {
            distance[node] = float.MaxValue;
            previous[node] = null;
            unvisited.Add(node);
        }


        distance[startNode] = 0;

        while (unvisited.Count > 0)
        {
            WayPoint currentNode = null;

            foreach (WayPoint node in unvisited)
            {
                if (currentNode == null || distance[node] < distance[currentNode])
                {
                    currentNode = node;
                }
            }

            unvisited.Remove(currentNode);

            if (currentNode == endNode)
            {
                break;
            }

            foreach (WayPoint neighbor in currentNode.GetWaypoints())
            {
                float alt = distance[currentNode] + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                if (alt < distance[neighbor])
                {
                    distance[neighbor] = alt;
                    previous[neighbor] = currentNode;
                }
            }

            foreach (WayPoint backwardNeighbor in currentNode.GetBackwardWaypoints())
            {
                float alt = distance[currentNode] + Vector2.Distance(currentNode.transform.position, backwardNeighbor.transform.position);

                if (alt < distance[backwardNeighbor])
                {
                    distance[backwardNeighbor] = alt;
                    previous[backwardNeighbor] = currentNode;
                }
            }
        }

        List<WayPoint> path = new List<WayPoint>();
        WayPoint current = endNode;

        while (current != null)
        {
            path.Insert(0, current);
            current = previous[current];
        }

        player.MoveToNode(startNode);
        return path;
    }

    private bool CanGoBackward(WayPoint waypoint)
    {
        List<WayPoint> backwardWaypoints = waypoint.GetBackwardWaypoints();
        foreach (WayPoint backwardWaypoint in backwardWaypoints)
        {
            if (backwardWaypoint == startNode)
            {
                return true; // Hay una conexión hacia atrás al nodo de inicio
            }
        }
        return false; // No hay conexión hacia atrás al nodo de inicio
    }


}