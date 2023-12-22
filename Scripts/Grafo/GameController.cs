using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
//using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour
{
    private IGraph<WayPoint> graph;
    public WayPoint startNode;
    public WayPoint endNode;
    public PlayerController player;
    private List<WayPoint> currentPath;
    private List<WayPoint> shortestPath;
    private List<WayPoint> NodosDerrota = new List<WayPoint>();
    private WayPoint currentNode; // Nuevo: Nodo actual del jugador

    public void Start()
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

        // Invocar el método Dijkstra con un retraso de 1 segundo
        Invoke("RunDijkstra", 1f);
    }

    void RunDijkstra()
    {
        // Usar el algoritmo de Dijkstra para encontrar el camino más corto
        shortestPath = Dijkstra(graph, startNode, endNode);
        currentNode = startNode; // Nuevo: Inicializar el nodo actual al inicio

        // Mover al jugador al nodo de inicio
        player.MoveToNode(startNode);
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

                if (!selectedNode.IsVisited())
                {
                    selectedNode.MarkVisited();

                    // Verificar si el nodo clickeado está en la lista de NodosDerrota
                    if (NodosDerrota.Contains(selectedNode))
                    {
                        player.MoveToNode(selectedNode);
                        Debug.Log("¡Derrota activada!");
                        LoadingManager.Instance.LoadScene(8, 10);
                    }
                    // Verificar si el nodo seleccionado activa la derrota y no se puede volver atrás
                    else if (selectedNode.activatesDefeat && !CanGoBackward(selectedNode))
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
                    else
                    {
                        currentNode = selectedNode; // Nuevo: Actualizar el nodo actual al seleccionado

                        currentPath = Dijkstra(graph, startNode, endNode);

                        player.MoveToNode(currentNode);


                        // Verificar si el jugador está yendo por el recorrido creado por Dijkstra y Graph
                        if (!IsPlayerOnPath())
                        {
                            Debug.Log("Oh, no! te saliste del camino y no puedes volver atras. Tendras que perder para reiniciar");
                        }

                        // Verificar si el nodo actual no tiene conexiones salientes
                        if (currentNode != startNode && (currentNode.GetWaypoints().Count == 0 && currentNode.GetOneWaypoints().Count == 0))
                        {
                            Debug.Log("¡Derrota activada! Este nodo es malo.");
                            LoadingManager.Instance.LoadScene(8, 10);
                        }
                    }
                }
                else
                {
                    Debug.Log("Ya has visitado este nodo.");
                }
            }
        }
    }




    public List<WayPoint> Dijkstra(IGraph<WayPoint> graph, WayPoint startNode, WayPoint endNode)
    {
        Dictionary<WayPoint, float> distance = new Dictionary<WayPoint, float>();
        Dictionary<WayPoint, WayPoint> previous = new Dictionary<WayPoint, WayPoint>();
        List<WayPoint> unvisited = new List<WayPoint>();

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

            if (currentNode == null)
            {
               
                Debug.LogError("Error: No se encontró ningún nodo en el bucle Dijkstra.");
                break;
            }

            unvisited.Remove(currentNode);

            if (currentNode == endNode)
            {
                break;
            }

            
            if (currentNode.GetWaypoints().Count == 0 && currentNode.GetOneWaypoints().Count == 0)
            {
                Debug.Log($"¡Cuidado! El nodo {currentNode.gameObject.name} es malvado.");
                NodosDerrota.Add(currentNode);  
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

            
            foreach (WayPoint backwardNeighbor in currentNode.GetOneWaypoints())
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

        // Mover al jugador al nodo de inicio al final del algoritmo
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

    private bool IsConnectedToCurrentNode(WayPoint targetNode)
    {
        if (currentNode != null)
        {
            return currentNode.Contains(targetNode);
        }
        return false;
    }

    private bool IsPlayerOnPath()
    {
        // Verificar si el jugador está yendo por el recorrido creado por Dijkstra y Graph
        return currentPath != null && currentPath.Count > 0 && currentPath.Contains(player.GetCurrentNode());
    }


}