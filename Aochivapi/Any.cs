namespace Aochivapi;

public class Any(object obj)
{
    public T Get<T>() => (T)obj;

    public static implicit operator Any(int i) => new(i);
    public static implicit operator Any(long i) => new(i);
    public static implicit operator Any(string i) => new(i);
}