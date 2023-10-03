using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPilaTDA<T>
{
    int Count { get; }  // Propiedad para obtener la cantidad de elementos en la pila
    void Push(T item); // M�todo para agregar un elemento a la pila
    T Pop();           // M�todo para eliminar y devolver el elemento superior de la pila
    T Peek();          // M�todo para obtener el elemento superior de la pila sin eliminarlo
    bool Any(Func<T, bool> p);
    IEnumerator<T> GetEnumerator();
}
