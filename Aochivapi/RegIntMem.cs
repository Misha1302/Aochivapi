namespace Aochivapi;

public class RegIntMem(reg? reg, int? @int, mem? mem, RegIntMem.RegIntMemType type)
{
    public enum RegIntMemType
    {
        RegType,
        IntType,
        MemType
    }

    public RegIntMem(reg a) : this(a, null, null, RegType)
    {
    }

    public RegIntMem(int a) : this(null, a, null, IntType)
    {
    }

    public RegIntMem(mem a) : this(null, null, a, MemType)
    {
    }

    public bool IsReg => Type == RegType;
    public bool IsInt => Type == IntType;
    public bool IsMem => Type == MemType;

    public reg Reg => reg ?? Thrower.Throw<reg>(new Ioe("Not a reg"));
    public int Int => @int ?? Thrower.Throw<int>(new Ioe("Not a int"));
    public mem Mem => mem ?? Thrower.Throw<mem>(new Ioe("Not a mem"));

    public RegIntMemType Type => type;


    public static implicit operator RegIntMem(reg a) => new(a);
    public static implicit operator RegIntMem(int a) => new(a);
    public static implicit operator RegIntMem(mem a) => new(a);

    public void Deconstruct(out reg reg, out int @int, out mem mem, out RegIntMemType type)
    {
        reg = Reg;
        @int = Int;
        mem = Mem;
        type = Type;
    }
}