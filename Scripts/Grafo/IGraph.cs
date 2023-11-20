using System.Collections.Generic;

public interface IGraph<T>
{
    // Agregar un nodo al grafo
    void AddNode(T node);

    // Agregar una arista entre dos nodos en el grafo
    void AddEdge(T fromNode, T toNode);

    // Obtener todos los nodos del grafo
    List<T> GetNodes();

    // Obtener los nodos vecinos de un nodo específico
    List<T> GetNeighbors(T node);

    // Verificar si existe una arista entre dos nodos
    bool HasEdge(T fromNode, T toNode);

    // Realizar una búsqueda en profundidad (DFS) desde un nodo
    List<T> DepthFirstSearch(T startNode);

    // Realizar una búsqueda en amplitud (BFS) desde un nodo
    List<T> BreadthFirstSearch(T startNode);
}
