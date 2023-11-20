using UnityEngine;

public class Nivel2 : MonoBehaviour
{
    private int puntajeNivel2 = 150; // Puntaje del nivel 2, calculado de alguna manera

    private void Start()
    {
        // Añadir el puntaje del nivel 2 al puntaje total en el script persistente
        ScriptPersistente.instance.puntajeTotal += puntajeNivel2;
    }
}
