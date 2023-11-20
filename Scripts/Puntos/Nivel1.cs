using UnityEngine;

public class Nivel1 : MonoBehaviour
{
    private int puntajeNivel1 = 100; // Puntaje del nivel 1, calculado de alguna manera

    private void Start()
    {
        // Añadir el puntaje del nivel 1 al puntaje total en el script persistente
        ScriptPersistente.instance.puntajeTotal += puntajeNivel1;
    }
}
