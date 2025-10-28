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
                enemy.Connect("EnemySelected", new Callable(playerMargin, "ExecuteAction"));
                enemy.Connect("ActionFinished", new Callable(this, "NextAction"));
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
            controlBox.Connect("DialogBoxFinished", new Callable(this, "BattlerChoice"), 4);
        }
    }

    private Battler[] SortActions()
    {
        Battler[] actionOrganizer;
        actionOrganizer = Global.Instance.GetParty();
        actionOrganizer = actionOrganizer.Concat(enemyTroop).ToArray();
        actionOrganizer = actionOrganizer.Where(b => b != null).ToArray();

        actionOrganizer = actionOrganizer.OrderByDescending(b => b.GetSpeed()).ToArray();

        return actionOrganizer;
    }

    private void BattlerChoice()
    {
        actionOrder = SortActions();

        if (actionOrder[currentAction] is Character)
        {
            Character character = actionOrder[currentAction] as Character;
            character.Action(controlBox, playerMargin);
            playerMargin.PassUIAnimations(uiAnimations);
        }

        if (actionOrder[currentAction] is Enemy)
        {
            Enemy enemy = actionOrder[currentAction] as Enemy;
            enemy.Action(controlBox);
        }
    }

    private void NextAction()
    {
        if (currentAction < actionOrder.Length - 1)
        {
            currentAction++;
        }
        else
        {
            turn++;
            currentAction = 0;
        }
        controlBox.Connect("DialogBoxFinished", new Callable(this, "BattlerChoice"), 4);
    }
}
