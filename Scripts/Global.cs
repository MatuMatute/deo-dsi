using Godot;

public partial class Global : Node
{
    public static Global Instance { get; private set; }
    private Character[] Party = new Character[3];
    private Character deo;

    public override void _Ready()
    {
        deo = new Character("DEO", Color.Color8(0, 0, 255, 255), 10, 10, 2, 0, 2, 0, 20);
        Party[0] = deo;
        Instance = this;
    }

    public Character[] GetParty() { return Party; }
}
