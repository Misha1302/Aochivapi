namespace Aochivapi;

using Iced.Intel;

public class FunctionCompiler
{
    private readonly StackManager _stackManager = new();

    public readonly RLabel Label;
    public readonly FunctionGenerator Generator;

    public FunctionCompiler(Assembler assembler, int localsCount, string name = "")
    {
        Label = new RLabel(assembler);
        Generator = new FunctionGenerator(assembler, Label, localsCount, name);
    }


    public void Push(long value)
    {
        var oneOf = _stackManager.GetToPush();
        if (oneOf.Is<reg>()) Generator.mov(oneOf.Item1, AsData(value));
        else Thrower.Throw(new Ioe());
    }

    private mem AsData(long value) => __[Generator.dq(value).Label];

    public void Imul()
    {
        Generator.mov(rax, _stackManager.GetToPop().Item1);
        Generator.imul(rax, _stackManager.GetToPop().Item1);
        Generator.mov(_stackManager.GetToPush().Item1, rax);
    }

    public void RetVoid() => Generator.ret();

    public void RetValue()
    {
        Generator.mov(rax, _stackManager.GetToPop().Item1);
        Generator.ret();
    }

    public void Call(RLabel label)
    {
        Generator.call(label);
        Generator.mov(_stackManager.GetToPush().Item1, rax);
    }
}