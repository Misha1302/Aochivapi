namespace Aochivapi;

using System.Text;
using Iced.Intel;

// NOTE: use RBX for some operations
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

    public void mov(RegIntMem a, RegIntMem b)
    {
        BinOp(a, b, assembler.mov, assembler.mov, assembler.mov, (mem1, mem2) =>
        {
            assembler.mov(rbx, mem2);
            assembler.mov(mem1, rbx);
        });
    }

    public void imul(RegIntMem a, RegIntMem b)
    {
        BinOp(a, b,
            assembler.imul,
            assembler.imul,
            (m, r) => regsAndOp(assembler.imul, m, r),
            (m1, m2) => moveAndOp(assembler.imul, m1, m2)
        );
    }

    public void idiv(RegIntMem a, RegIntMem b)
    {
        assembler.push(rax);
        assembler.push(rdx);

        assembler.xor(rdx, rdx);

        BinOp(a, b,
            (r, r2) =>
            {
                mov(rax, r);
                assembler.idiv(r2);
                mov(r, rax);
            },
            (r, m) =>
            {
                mov(rax, r);
                assembler.idiv(m);
                mov(r, rax);
            },
            (m, r) =>
            {
                mov(rax, m);
                assembler.idiv(r);
                mov(m, rax);
            },
            (m, m2) =>
            {
                mov(rax, m);
                assembler.idiv(m2);
                mov(m, rax);
            }
        );

        assembler.pop(rdx);
        assembler.pop(rax);
    }


    public void iadd(RegIntMem a, RegIntMem b)
    {
        BinOp(a, b,
            assembler.add,
            assembler.add,
            assembler.add,
            (m1, m2) => moveAndOp(assembler.add, m1, m2)
        );
    }


    public void isub(RegIntMem a, RegIntMem b)
    {
        BinOp(a, b,
            assembler.sub,
            assembler.sub,
            assembler.sub,
            (m1, m2) => moveAndOp(assembler.sub, m1, m2)
        );
    }

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


    private static void BinOp(RegIntMem a,
        RegIntMem b,
        Action<reg, reg> regReg,
        Action<reg, mem> regMem,
        Action<mem, reg> memReg,
        Action<mem, mem> memMem)
    {
        if (a.IsReg && b.IsReg) regReg(a.Reg, b.Reg);
        else if (a.IsReg && b.IsInt) regMem(a.Reg, ToMem(b.Int));
        else if (a.IsReg && b.IsMem) regMem(a.Reg, b.Mem);

        else if (a.IsMem && b.IsReg) memReg(a.Mem, b.Reg);
        else if (a.IsMem && b.IsInt) memMem(a.Mem, ToMem(b.Int));
        else if (a.IsMem && b.IsMem) memMem(a.Mem, b.Mem);

        else if (a.IsInt && b.IsReg) memReg(ToMem(a.Int), b.Reg);
        else if (a.IsInt && b.IsInt) memMem(ToMem(a.Int), ToMem(b.Int));
        else if (a.IsInt && b.IsMem) memMem(ToMem(a.Int), b.Mem);

        else Thrower.Throw(new Unreachable());
    }

    private static mem ToMem(int @int) => __[rbp + @int];

    private void regsAndOp(Action<reg, reg> op, mem m, reg r)
    {
        mov(rbx, m);
        op(rbx, r);
    }

    private void moveAndOp(Action<reg, mem> op, mem mem1, mem mem2)
    {
        mov(rbx, mem1);
        op(rbx, mem2);
    }
}