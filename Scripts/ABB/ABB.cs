using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Jugador
{
    public string Nombre { get; set; }
    public int Puntaje { get; set; }

    public Jugador(string nombre, int puntaje)
    {
        Nombre = nombre;
        Puntaje = puntaje;
    }
}



public class NodoABB
{
    public Jugador Jugador { get; set; }
    public NodoABB Izquierda { get; set; }
    public NodoABB Derecha { get; set; }

    public NodoABB(Jugador jugador)
    {
        Jugador = jugador;
        Izquierda = null;
        Derecha = null;
    }
}


public class ABB : ABBTDA
{
    private NodoABB raiz;

    public bool ArbolVacio()
    {
        return raiz == null;

    }

    public Jugador Raiz()
    {
        if (raiz == null)
        {
            throw new InvalidOperationException("El arbol esta vacio");

        }
        return raiz.Jugador;
    }

    public ABBTDA HijoDer()
    {
        if (raiz == null)
        {
            throw new InvalidOperationException("El arbol esta vacio");

        }
        ABB hijoDer = new ABB();
        hijoDer.raiz = raiz.Derecha;
        return hijoDer;
    }
    
    public ABBTDA HijoIzq()
    {
        if (raiz == null)
        {
            throw new InvalidOperationException("El arbol esta vacio");

        }
        ABB hijoIzq = new ABB();
        hijoIzq.raiz = raiz.Izquierda;
        return hijoIzq;
    }

    public void InicializarArbol()
    {
        raiz = null;
    }

    public void AgregarJugador(Jugador jugador)
    {
        raiz = AgregarJugador(raiz, jugador);
    }

    private NodoABB AgregarJugador(NodoABB nodo, Jugador jugador)
    {
        if(nodo == null)
        {
            return new NodoABB(jugador);
        }
        if(jugador.Puntaje < nodo.Jugador.Puntaje)
        {
            nodo.Izquierda = AgregarJugador(nodo.Izquierda, jugador);

        }
        else if (jugador.Puntaje > nodo.Jugador.Puntaje)
        {
            nodo.Derecha = AgregarJugador(nodo.Derecha, jugador);
        }
        return nodo;
    }

    public void EliminarJugador(int puntaje)
    {
        raiz = EliminarJugador(raiz, puntaje);
    }

    private NodoABB EliminarJugador(NodoABB nodo, int puntaje)
    {
        if (nodo == null)
        {
            return nodo;
        }
        if (puntaje < nodo.Jugador.Puntaje)
        {
            nodo.Izquierda = EliminarJugador(nodo.Izquierda, puntaje);

        }
        else if (puntaje > nodo.Jugador.Puntaje)
        {
            nodo.Derecha = EliminarJugador(nodo.Derecha, puntaje);
        }
        else
        {
            if(nodo.Izquierda == null)
            {
                return nodo.Derecha;
            }
            else if (nodo.Derecha == null)
            {
                return nodo.Izquierda;
            }
            nodo.Jugador = JugadorConMenorPuntaje(nodo.Derecha);
            nodo.Derecha = EliminarJugador(nodo.Derecha, nodo.Jugador.Puntaje);

        }
        return nodo;
    }

    public Jugador JugadorConMayorPuntaje()
    {
        if(raiz == null)
        {
            throw new InvalidOperationException("El arbol esta vacio");
        }
        return JugadorConMayorPuntaje(raiz);
    }

    private Jugador JugadorConMayorPuntaje(NodoABB nodo)
    {
        if(nodo.Derecha == null)
        {
            return nodo.Jugador;
        }
        return JugadorConMayorPuntaje(nodo.Derecha);
    }


    public Jugador JugadorConMenorPuntaje()
    {
        if (raiz == null)
        {
            throw new InvalidOperationException("El arbol esta vacio");
        }
        return JugadorConMenorPuntaje(raiz);
    }

    private Jugador JugadorConMenorPuntaje(NodoABB nodo)
    {
        if (nodo.Izquierda == null)
        {
            return nodo.Jugador;
        }
        return JugadorConMenorPuntaje(nodo.Izquierda);
    }

    public List<Jugador> ObtenerJugadoresEnOrden()
    {
        List<Jugador> jugadoresEnOrden = new List<Jugador>();
        ObtenerJugadoresEnOrden(raiz, jugadoresEnOrden);
        return jugadoresEnOrden;
    }

    private void ObtenerJugadoresEnOrden(NodoABB nodo, List<Jugador> jugadores)
    {
        if(nodo != null)
        {
            ObtenerJugadoresEnOrden(nodo.Derecha, jugadores);
            jugadores.Add(nodo.Jugador);
            ObtenerJugadoresEnOrden(nodo.Izquierda, jugadores);
        }
    }
}
