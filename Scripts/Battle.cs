using Godot;
using System.Linq;

public partial class Battle : CanvasLayer
{
    [Export]
    private ControlBox controlBox;
    [Export]
    private HBoxContainer enemyContainer;
    [Export]
    private PlayerMargin playerMargin;
    [Export]
    private AnimationPlayer uiAnimations;
    [Export]
    private Troop Troop;
    private Enemy[] enemyTroop;
    private Battler[] actionOrder;
    private ushort turn;
    private byte currentAction = 0;

    public override void _Ready()
    {
        playerMargin.PassControlBox(controlBox);
        PackedScene[] enemyScenes = Troop.GetEnemyTroop();
        enemyTroop = new Enemy[enemyScenes.Length];

        for (int i = 0; i < enemyScenes.Length; i++)
        {
            if (enemyScenes[i] != null)
            {
                Enemy enemy = enemyScenes[i].Instantiate() as Enemy;
                enemy.AddToGroup("Enemies");
                enemyTroop[i] = enemy;
                enemyContainer.AddChild(enemy);
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
        actionOrganizer = Global.Instance.GetParty();
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

    private void BattlerChoice()
    {
        actionOrder = SortActions();

        if (actionOrder[currentAction] is Character)
        {
            GD.Print("Turno jugador");
            actionOrder[currentAction].Action(controlBox, playerMargin);
        }

        if (actionOrder[currentAction] is Enemy)
        {
            GD.Print("Turno enemigo");
        }
    }
}
