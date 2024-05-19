namespace Aochivapi;

using System.Diagnostics;

public static class Program
{
    private static void Main()
    {
        var combiner = MakeProgram().GetCombiner();
        Console.WriteLine(combiner);
        ExecuteFunc(BytesToFunc(combiner.ToBytes()));
    }

    private static Compiler MakeProgram()
    {
        var c = new Compiler();
        c.NewFunction([
            new Op(OpType.CallFunction, "NotMain"),
            new Op(OpType.RetValue)
        ], "Main");

        // 5 * (100 / 7 - 8) -> 5 100 7 / 8 - * -> 30
        c.NewFunction([
            new Op(OpType.Push, 5L),
            new Op(OpType.Push, 100L),
            new Op(OpType.Push, 7L),
            new Op(OpType.IDiv),
            new Op(OpType.Push, 8L),
            new Op(OpType.ISub),
            new Op(OpType.IMul),
            new Op(OpType.RetValue)
        ], "NotMain");

        c.Compile();
        return c;
    }


    private static NativeFunction BytesToFunc(byte[] shellcode) => AsmFunctionMaker.ToFunction(shellcode);

    private static void ExecuteFunc(NativeFunction func)
    {
        var sw = Stopwatch.StartNew();
        Console.WriteLine($"Result: {func.Call<long>()}");
        Console.WriteLine($"Elapsed ms: {sw.ElapsedMilliseconds}");
    }
}