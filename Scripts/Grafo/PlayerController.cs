using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private WayPoint currentNode; // Nodo en el que se encuentra el jugador

    // M�todo para mover al jugador a un nodo
    public void MoveToNode(WayPoint node)
    {
        //if (currentNode != null)
        //{
        //    // Desactivar el resaltado del nodo actual si lo hay
        //    // Esto depender� de c�mo implementes la visualizaci�n de los nodos seleccionados.
        //    // Puedes cambiar esto seg�n tus necesidades.
        //    currentNode.DeactivateHighlight();
        //}

        // Mover al jugador al nodo deseado en 2D
        transform.position = new Vector2(node.transform.position.x, node.transform.position.y);
        currentNode = node;


        // Puedes activar el resaltado del nodo actual si lo deseas.
        // currentNode.ActivateHighlight();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Golpe");
            LoadingManager.Instance.LoadScene(8, 9);

        }

    }
}

