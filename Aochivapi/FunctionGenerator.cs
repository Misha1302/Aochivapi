namespace Aochivapi;

using System.Text;
using Iced.Intel;

// ReSharper disable InconsistentNaming
public class FunctionGenerator(Assembler assembler, RLabel functionLabel, int localsCount, string name = "")
{
    private const int LocalSize = sizeof(long);

    private readonly List<(RLabel label, long value)> _defineQuad = [];

    public readonly string Name = name;
    private Range _instructionsRange;

    public void DefineData()
    {
        foreach (var pair in _defineQuad)
        {
            pair.label.Emit();
            assembler.dq(pair.value);
        }

        _instructionsRange = new Range(_instructionsRange.Start, assembler.Instructions.Count);
    }

    public override string ToString() => InstructionsToSb().ToString();

    private StringBuilder InstructionsToSb()
    {
        var sb = new StringBuilder();
        for (var i = _instructionsRange.Start.Value; i < _instructionsRange.End.Value; i++)
            sb.AppendLine(assembler.Instructions[i].ToString());

        return sb;
    }

    public void prolog()
    {
        _instructionsRange = new Range(assembler.Instructions.Count, 0);

        functionLabel.Emit();

        assembler.push(rbp);
        assembler.mov(rbp, rsp);
        assembler.sub(rsp, localsCount * LocalSize);
    }

    private void epilogue()
    {
        assembler.mov(rsp, rbp);
        assembler.pop(rbp);
    }

    public void mov(reg a, reg b) => assembler.mov(a, b);
    public void mov(reg a, mem b) => assembler.mov(a, b);
    public void imul(reg a, reg b) => assembler.imul(a, b);

    public void ret()
    {
        epilogue();
        assembler.ret();
    }

    public RLabel dq(long value)
    {
        var label = new RLabel(assembler, $"i64[{value.ToString()}]");
        _defineQuad.Add((label, value));
        return label;
    }

    public void call(RLabel label)
    {
        assembler.call(label.Label);
    }
}