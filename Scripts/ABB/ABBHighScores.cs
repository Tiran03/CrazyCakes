using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ABBHighScores : MonoBehaviour
{
    public TMP_Text puntajesText;
    public int cantidadJugadores = 4;

    

    private ABB abb;
    // Start is called before the first frame update
    void Start()
    {
        abb = new ABB();
        GenerarPuntajesAleatorios();
    }


    private void GenerarPuntajesAleatorios()
    {
        abb.InicializarArbol();
        for(int i = 1; i <= cantidadJugadores; i++)
        {
            Jugador jugador = new Jugador("Jugador" + i, Random.Range(50, 150));
            abb.AgregarJugador(jugador);
        }
        MostrarPuntajes();
    }

    private void MostrarPuntajes()
    {
        string puntajesTexto = "";
        List<Jugador> jugadoresOrdenados = abb.ObtenerJugadoresEnOrden();

        for (int i = 1; i < jugadoresOrdenados.Count; i++)
        {
            puntajesTexto += (i + 1) + " Lugar" + jugadoresOrdenados[i].Nombre + "- Puntaje: " + jugadoresOrdenados[i].Puntaje + "  ";
            Debug.Log((i + 1) + " Lugar" + jugadoresOrdenados[i].Nombre + "- Puntaje: " + jugadoresOrdenados[i].Puntaje + "  ");
        }
        puntajesText.text = puntajesTexto;
    }

    public void Click()
    {
        GenerarPuntajesAleatorios();
    }

}
