using System;

public class Lazy<T>
{
    private Func<T> generator;
    private T cache;

    public Lazy(Func<T> generator)
    {
        this.generator = generator;
    }

    internal T Get()
    {
        if (cache == null)
        {
            cache = generator();
        }
        return cache;
    }
}