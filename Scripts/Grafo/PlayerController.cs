using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private WayPoint currentNode; // Nodo en el que se encuentra el jugador
    public float rotationSpeed = 5f;

    void Update()
    {
        // Obtener la posici�n del mouse en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcular la direcci�n hacia la posici�n del mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calcular el �ngulo en radianes
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotar el jugador hacia el �ngulo calculado
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
    }

    public WayPoint GetCurrentNode()
    {
        return currentNode;
    }

    // M�todo para mover al jugador a un nodo
    public void MoveToNode(WayPoint node)
    {
        // Mover al jugador al nodo deseado en 2D
        transform.position = new Vector2(node.transform.position.x, node.transform.position.y);
        currentNode = node;
    }

    // M�todo para manejar colisiones con otros colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Golpe");
            // Considera c�mo deseas manejar la colisi�n, por ejemplo, cargar una escena de derrota
        }
    }
}

