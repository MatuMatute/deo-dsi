using Godot;

public partial class Global : Node
{
    public static Global Instance { get; private set; }
    private Character[] Party = new Character[3];
    private Character deo;

    public override void _Ready()
    {
        deo = new Character("DEO", Color.Color8(0, 0, 255, 255), 100, 100, 20, 0, 10, 0, 20, [1, 2, 1, 1, 1]);
        Party[0] = deo;
        Instance = this;
    }

    public int CharactersInParty()
    {
        int Amount = 0;

        for (int i = 0; i < Party.GetLength(0); i++)
        {
            if (Party[i] != null)
                Amount++;
        }
        return Amount;
    }

    public bool IsPartyDefeated()
    {
        int DefeatedCharacters = 0;

        for (int i = 0; i < CharactersInParty(); i++)
        {
            if (Party[i].CheckDeath())
                DefeatedCharacters++;
        }

        if (DefeatedCharacters == CharactersInParty())
            return true;
        return false;
    }

    public Character[] GetParty() { return Party; }
}
