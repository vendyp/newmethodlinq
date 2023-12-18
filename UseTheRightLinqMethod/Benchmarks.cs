using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.StackSources;

namespace UseTheRightLinqMethod;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    public List<int> Numbers { get; set; }

    public MyList MyNumbers { get; set; } = new();
    
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(420);
        Numbers = Enumerable
            .Range(0, 10_000)
            .Select(_ => random.Next(1, int.MaxValue))
            .ToList();
        
        Numbers.ForEach(x => MyNumbers.Add(x));
    }

    [Benchmark]
    public bool All()
    {
        return All(Numbers, x => x > 0);
    }

    [Benchmark]
    public bool TrueForAll()
    {
        return TrueForAll(x => x > 0);
    }
    
    [Benchmark]
    public bool AllMyList()
    {
        return AllMyList(MyNumbers, x => x > 0);
    }
    
    public static bool AllMyList(MyList source, Func<int, bool> predicate)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        foreach (int element in source)
        {
            if (!predicate(element))
            {
                return false;
            }
        }

        return true;
    }
    
    public static bool All<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        IEnumerator<TSource> enumerator = source.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                TSource current = enumerator.Current;
                if (!predicate(current))
                {
                    return false;
                }
            }
        }
        finally
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
            }
        }

        return true;
    }
    
    public bool TrueForAll(Predicate<int> match)
    {
        if (match == null)
        {
            throw new ArgumentNullException(nameof(match));
        }

        var size = Numbers.Count;
        for (int i = 0; i < size; i++)
        {
            if (!match(Numbers[i]))
            {
                return false;
            }
        }
        return true;
    }
}
