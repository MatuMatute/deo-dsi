using Godot;

public partial class Troop : Resource
{
    [ExportCategory("Troop 😈😈😈")]
    [Export]
    private PackedScene[] EnemyTroop = new PackedScene[6];

    public PackedScene[] GetEnemyTroop() { return EnemyTroop; }
}
