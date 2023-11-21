using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort<T> : IQuickSort<T> where T : System.IComparable<T>
{
    public void Sort(T[] array)
    {
        QuickSortRecursive(array, 0, array.Length - 1);
    }

    private void QuickSortRecursive(T[] array, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high);

            QuickSortRecursive(array, low, pivotIndex - 1);
            QuickSortRecursive(array, pivotIndex + 1, high);
        }
    }

    private int Partition(T[] array, int low, int high)
    {
        T pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (array[j].CompareTo(pivot) < 0)
            {
                i++;
                Swap(array, i, j);
            }
        }

        Swap(array, i + 1, high);
        return i + 1;
    }

    private void Swap(T[] array, int indexA, int indexB)
    {
        T temp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = temp;
    }
}

