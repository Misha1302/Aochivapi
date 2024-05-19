namespace Aochivapi;

using System.Text;using Iced.Intel;

public class FunctionCombiner(Assembler asm, List<FunctionGenerator> functions)
{
    public byte[] ToBytes()
    {
        var stream = new MemoryStream();
        asm.Assemble(new StreamCodeWriter(stream), 0);
        return stream.GetBuffer();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var f in functions)
        {
            sb.AppendLine($"----------- {f.Name} -----------");
            sb.AppendLine(f.ToString());
        }

        return sb.ToString();
    }
}