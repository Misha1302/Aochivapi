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
        Generator.mov(_stackManager.GetToPush(), AsData(value));
    }

    private mem AsData(long value) => __[Generator.dq(value).Label];

    public void Imul() => BinOp(Generator.imul);
    public void IAdd() => BinOp(Generator.iadd);
    public void ISub() => BinOp(Generator.isub);
    public void IDiv() => BinOp(Generator.idiv);

    public void RetVoid() => Generator.ret();

    public void RetValue()
    {
        Generator.mov(rax, _stackManager.GetToPop());
        Generator.ret();
    }

    public void Call(RLabel label)
    {
        Generator.call(label);
        Generator.mov(_stackManager.GetToPush(), rax);
    }

    private void BinOp(Action<RegIntMem, RegIntMem> op)
    {
        var b = _stackManager.GetToPop();
        var a = _stackManager.GetToPop();
        op(a, b);
        _stackManager.GetToPush();
    }
}