using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class GameController : MonoBehaviour
{
    private IGraph<WayPoint> graph;
    public WayPoint startNode; // Nodo de inicio del jugador
    public WayPoint endNode;   // Nodo de salida del laberinto
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

        // Aquí puedes hacer lo que necesites con el camino encontrado

        // Por ejemplo, puedes mover al jugador al nodo de inicio
        player.MoveToNode(startNode);
    }

    void Update()
    {
        // Detectar clics del mouse en los nodos
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit hit;
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);  // Cambiado a Physics2D

            Debug.Log("Click");

            if (hit.collider != null && hit.collider.CompareTag("WayPoint"))
            {
                WayPoint selectedNode = hit.collider.GetComponent<WayPoint>();
                player.MoveToNode(selectedNode);

                if (selectedNode.activatesDefeat)
                {
                    // Implementa aquí la lógica de derrota, por ejemplo, mostrando un mensaje o cargando una escena de derrota.
                    Debug.Log("¡Derrota activada!");
                    // Ejemplo de carga de una nueva escena después de la derrota
                    LoadingManager.Instance.LoadScene(8, 10);
                }

                else if (selectedNode == endNode)
                {
                    // ¡Ganaste! Puedes implementar aquí la lógica de victoria, como cargar una nueva escena.
                    Debug.Log("¡Ganaste!");
                    // Ejemplo de carga de una nueva escena después de ganar
                    LoadingManager.Instance.LoadScene(8, 9);
                }
            }
        }
    }




    // Implementación del algoritmo de Dijkstra

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


}