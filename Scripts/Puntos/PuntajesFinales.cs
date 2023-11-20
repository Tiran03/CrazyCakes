using UnityEngine;
using TMPro;

public class PuntajesFinales : MonoBehaviour
{
    public TMP_Text puntajesText;

    private void Start()
    {
        // Calcular el puntaje total considerando el tiempo en todos los niveles
        int puntajeTotalFinal = ScriptPersistente.instance.puntajeTotal + ScriptPersistente.instance.puntajePorTiempo;

        // Mostrar el puntaje total en la escena de puntajes finales
        puntajesText.text = "Puntaje Total: " + puntajeTotalFinal;
    }
}

