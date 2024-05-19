namespace Aochivapi;

public class OneOf<T1, T2>
{
    public readonly T1? Item1;
    public readonly T2? Item2;
    public readonly Type Type;

    public OneOf(T1? item1) : this(item1, default, typeof(T1))
    {
    }

    public OneOf(T2? item2) : this(default, item2, typeof(T2))
    {
    }

    private OneOf(T1? a, T2? b, Type type)
    {
        Item1 = a;
        Item2 = b;
        Type = type;
    }

    public bool Is<T>() => Type == typeof(T);
}