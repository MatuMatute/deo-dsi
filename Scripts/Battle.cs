using Godot;

public partial class Battle : CanvasLayer
{
    [Export]
    private HBoxContainer enemyContainer;
    [Export]
    private VBoxContainer playerContainer;
    [Export]
    private Troop Troop;
    private readonly PackedScene characterBox = ResourceLoader.Load<PackedScene>("res://Scenes/characterbox.tscn");
    private Enemy[] enemyTroop;
    private int turn;
    private int currentAction;

    public override void _Ready()
    {
        PackedScene[] enemyScenes = Troop.GetEnemyTroop();
        enemyTroop = new Enemy[enemyScenes.Length];

        for (int i = 0; i < enemyScenes.Length; i++)
        {
            Enemy enemy = enemyScenes[i].Instantiate() as Enemy;
            enemyTroop[i] = enemy;
            enemyContainer.AddChild(enemy);
        }

        foreach (Character character in Global.Instance.GetParty())
        {
            if (character != null)
            {
                CharacterBox currentBox = characterBox.Instantiate() as CharacterBox;
                currentBox.SetCharacter(character);
                playerContainer.AddChild(currentBox);
            }
        }
    }
    
    private void Accion()
    {
        
    }
}
