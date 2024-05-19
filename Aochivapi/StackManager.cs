namespace Aochivapi;

public class StackManager
{
    private int _index;

    public OneOf<reg, int> GetToPush()
    {
        var result = GetCurrent();
        _index++;
        return result;
    }

    public OneOf<reg, int> GetToPop()
    {
        _index--;
        return GetCurrent();
    }

    private OneOf<reg, int> GetCurrent()
    {
        var result = _index switch
        {
            0 => new OneOf<reg, int>(r12),
            1 => new OneOf<reg, int>(r13),
            2 => new OneOf<reg, int>(r14),
            3 => new OneOf<reg, int>(r15),
            _ => new OneOf<reg, int>((_index - 4) * 8)
        };
        return result;
    }
}