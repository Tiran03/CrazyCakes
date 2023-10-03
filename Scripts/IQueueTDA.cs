using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueueTDA<T>
{
    void Enqueue(T element);
    T Dequeue();
    T Peek();
    bool IsEmpty();
    void Clear();
}
