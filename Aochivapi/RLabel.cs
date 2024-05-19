namespace Aochivapi;

using Iced.Intel;

public class RLabel(Assembler assembler, string? name = null)
{
    private Label _label = assembler.CreateLabel(name);

    public Label Label => _label;
    public bool IsEmitted => _label.InstructionIndex >= 0;

    public void Emit() => assembler.Label(ref _label);

    public override string ToString() => name;
}