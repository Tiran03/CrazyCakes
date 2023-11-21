using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuickSort<T> where T : System.IComparable<T>
{
    void Sort(T[] array);
}
