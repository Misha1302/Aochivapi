namespace Aochivapi;

using Iced.Intel;

public class Compiler
{
    private readonly List<FunctionsOps> _functionCompilers = [];

    private readonly Assembler _assembler = new(64);

    public FunctionCombiner GetCombiner() =>
        new(_assembler, _functionCompilers.Select(x => x.FunctionCompiler.Generator).ToList());

    public void NewFunction(Op[] ops, string name)
    {
        var localsCount = ops.Count(x => x.OpType == OpType.Push);
        var function = new FunctionsOps(new FunctionCompiler(_assembler, localsCount, name), ops);
        _functionCompilers.Add(function);
    }

    public void Compile()
    {
        foreach (var (function, ops) in _functionCompilers)
        {
            function.Generator.prolog();

            foreach (var op in ops)
                switch (op.OpType)
                {
                    case OpType.Push:
                        function.Push(op.Arg1.Get<long>());
                        break;
                    case OpType.IAdd:
                        function.IAdd();
                        break;
                    case OpType.ISub:
                        function.ISub();
                        break;
                    case OpType.IMul:
                        function.Imul();
                        break;
                    case OpType.IDiv:
                        function.IDiv();
                        break;
                    case OpType.RetValue:
                        function.RetValue();
                        break;
                    case OpType.RetVoid:
                        function.RetVoid();
                        break;
                    case OpType.CallFunction:
                        var name = op.Arg1.Get<string>();

                        var label =
                            (_functionCompilers.Find(x => x.FunctionCompiler.Generator.Name == name)
                             ?? Thrower.Throw<FunctionsOps>(new Ioe($"Cannot found function {name}")))
                            .FunctionCompiler.Label;

                        function.Call(label);
                        break;
                    default:
                        Thrower.Throw(new Aofre($"Unknown op {op}"));
                        break;
                }

            function.Generator.DefineData();
        }
    }
}