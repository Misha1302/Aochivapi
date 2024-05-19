namespace Aochivapi;

public class StackManager
{
    private int _index;

    public RegIntMem GetToPush()
    {
        var result = GetCurrent();
        _index++;
        return result;
    }

    public RegIntMem GetToPop()
    {
        _index--;
        return GetCurrent();
    }

    public RegIntMem Peek() => GetCurrent();

    private RegIntMem GetCurrent()
    {
        var result = _index switch
        {
            0 => new RegIntMem(r12),
            1 => new RegIntMem(r13),
            2 => new RegIntMem(r14),
            3 => new RegIntMem(r15),
            _ => new RegIntMem((_index - 4) * 8)
        };
        return result;
    }
}