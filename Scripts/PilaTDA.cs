using System;
using System.Collections.Generic;
using System.Linq;

public class PilaTDA<T>
{
    private List<T> items = new List<T>();

    public int Count
    {
        get { return items.Count; }
    }

    public void Push(T item)
    {
        items.Add(item);
    }

    public T Pop()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("La pila está vacía.");
        }

        T item = items[Count - 1];
        items.RemoveAt(Count - 1);
        return item;
    }

    public T Peek()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("La pila está vacía.");
        }

        return items[Count - 1];
    }

    public bool Any(Func<T, bool> predicate)
    {
        return items.Any(predicate);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }
}
