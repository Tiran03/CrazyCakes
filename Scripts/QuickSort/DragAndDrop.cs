using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    public string flavor;  // Sabor del caramelo
    public bool isCorrect = false;  // Indica si el caramelo está en la posición correcta
    public CandySortingGame candySortingGame;
    public Transform[] candyPositions;

    private bool isAlreadyPlaced = false;  // Flag para evitar sumar múltiples veces

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            // Verifica si el caramelo está cerca de una posición de destino y si coincide con el sabor correcto
            foreach (Transform position in candyPositions)
            {
                float distance = Vector3.Distance(transform.position, position.position);
                if (distance < 0.5f)
                {
                    if (flavor == position.GetComponent<PositionScript>().expectedFlavor && !isAlreadyPlaced)
                    {
                        isCorrect = true;
                        isAlreadyPlaced = true;
                        candySortingGame.CandyPlacedCorrectly(this); // Notifica al script principal
                    }
                }
            }
        }
    }
}
