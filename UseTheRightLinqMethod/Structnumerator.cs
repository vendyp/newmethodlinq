using System.Collections;

namespace UseTheRightLinqMethod;

public class MyList : IStructnumerable<List<int>.Enumerator, int>
{
    private List<int> _items = new();

    public void Add(int value) => _items.Add(value);
    
    public int Count => _items.Count;

    public int this[int index] => _items[index];
    
    public Structnumerator<List<int>.Enumerator, int> GetEnumerator()
    {
        return new Structnumerator<List<int>.Enumerator, int>(_items.GetEnumerator());
    }
}

public struct Structnumerator<TEnumerator, TValue> : IEnumerator<TValue>
    where TEnumerator : IEnumerator<TValue>
{
    private TEnumerator _enumerator;
    public TValue Current => _enumerator.Current;
    object IEnumerator.Current => _enumerator.Current;

    public Structnumerator(TEnumerator enumerator)
    {
        _enumerator = enumerator;
    }

    public bool MoveNext() => _enumerator.MoveNext();

    public void Dispose()
    {
        _enumerator.Dispose();
    }

    public void Reset()
    {
        _enumerator.Reset();
    }
}

public interface IStructnumerable<TEnumerator, TValue>
    where TEnumerator : IEnumerator<TValue>
{
    Structnumerator<TEnumerator, TValue> GetEnumerator();
}
