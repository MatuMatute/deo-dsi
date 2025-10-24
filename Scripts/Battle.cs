using Godot;
using System.Linq;

public partial class Battle : CanvasLayer
{
    [Export]
    private ControlBox controlBox;
    [Export]
    private HBoxContainer enemyContainer;
    [Export]
    private VBoxContainer playerContainer;
    [Export]
    private AnimationPlayer uiAnimations;
    [Export]
    private Troop Troop;
    private readonly PackedScene characterBox = ResourceLoader.Load<PackedScene>("res://Scenes/characterbox.tscn");
    private Character[] Party;
    private Enemy[] enemyTroop;
    private Battler[] actionOrder;
    private ushort turn;
    private byte currentAction = 0;

    public override void _Ready()
    {
        Party = Global.Instance.GetParty();
        PackedScene[] enemyScenes = Troop.GetEnemyTroop();
        enemyTroop = new Enemy[enemyScenes.Length];

        for (int i = 0; i < enemyScenes.Length; i++)
        {
            if (enemyScenes[i] != null)
            {
                Enemy enemy = enemyScenes[i].Instantiate() as Enemy;
                enemyTroop[i] = enemy;
                enemyContainer.AddChild(enemy);
            }
        }

        foreach (Character character in Party)
        {
            if (character != null)
            {
                CharacterBox currentBox = characterBox.Instantiate() as CharacterBox;
                currentBox.SetCharacter(character);
                playerContainer.AddChild(currentBox);
            }
        }

        uiAnimations.Play("Start");
    }

    private void UIAnimationFinished(StringName animName)
    {
        if (animName == "Start")
        {
            uiAnimations.Play("CharacterShow");
            controlBox.Grow();
        }
    }

    private Battler[] SortActions()
    {
        Battler[] actionOrganizer;
        actionOrganizer = Party;
        actionOrganizer = actionOrganizer.Concat(enemyTroop).ToArray();
        actionOrganizer = actionOrganizer.Where(b => b != null).ToArray();

        for (int i = 0; i < actionOrganizer.Length - 1; i++)
        {
            if (actionOrganizer[i].GetSpeed() < actionOrganizer[i + 1].GetSpeed())
            {
                actionOrganizer = actionOrganizer.Append(actionOrganizer[i]).ToArray();
                actionOrganizer[i] = null;
            }
        }
        
        actionOrganizer = actionOrganizer.Where(b => b != null).ToArray();
        return actionOrganizer;
    }
    
    private void Action()
    {
        actionOrder = SortActions();

        if (actionOrder[currentAction] is Character)
        {
            GD.Print("Turno jugador");
            uiAnimations.Play("CommandShow");
            actionOrder[currentAction].Action(controlBox);
        }

        if (actionOrder[currentAction] is Enemy)
        {
            GD.Print("Turno enemigo");
        }
    }
}
