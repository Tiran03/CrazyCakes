using System.Collections.Generic;

//using System.Collections.Generic;

public class QueueTDA<T> : IQueueTDA<T>
{
    private List<T> elements = new List<T>();
    private int front = 0;

    public int Count => elements.Count; // Propiedad para obtener el número de elementos en la cola

    public void Enqueue(T element)
    {
        elements.Add(element);
    }

    public T Dequeue()
    {
        if (IsEmpty())
            throw new System.InvalidOperationException("The queue is empty");

        T value = elements[front];
        front++;
        if (front >= elements.Count)
        {
            front = 0;
            elements.Clear();
        }
        return value;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new System.InvalidOperationException("The queue is empty");

        return elements[front];
    }

    public bool IsEmpty()
    {
        return elements.Count == 0;
    }

    public T ElementAt(int index)
    {
        if (index < 0 || index >= elements.Count)
            throw new System.ArgumentOutOfRangeException("index", "Index is out of range");

        return elements[(front + index) % elements.Count];
    }
    public void Clear()
    {
        elements.Clear();
        front = 0;
    }
}