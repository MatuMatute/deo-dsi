using Godot;

public partial class PlayerMargin : MarginContainer
{
    [Signal]
    public delegate void ActionFinishedEventHandler();
    enum Transition { none, back, commands }; 
    [Export]
    private VBoxContainer playerContainer;
    [Export]
    private Button backButton;
    [Export]
    private AnimationPlayer animationPlayer;
    private readonly PackedScene characterBox = ResourceLoader.Load<PackedScene>("res://Scenes/characterbox.tscn");
    private Character currentCharacter;
    private Skill currentSkill;
    private Transition transition;
    private ControlBox controlBox;
    private AnimationPlayer cameraAnimation;

    public override void _Ready()
    {
        foreach (Character character in Global.Instance.GetParty())
        {
            if (character != null)
            {
                CharacterBox currentBox = characterBox.Instantiate() as CharacterBox;
                character.Connect("ActionFinished", new Callable(GetParent(), "NextAction"));
                character.AssignBox(currentBox);
                playerContainer.AddChild(currentBox);
            }
        }
    }

    public void ShowCommands()
    {
        backButton.Hide();
        GetTree().CallGroup("MainCommands", MethodName.Show);
        animationPlayer.Play("ShowCommands");
    }

    public void ShowBack()
    {
        backButton.Show();
        GetTree().CallGroup("MainCommands", MethodName.Hide);
        animationPlayer.Play("ShowCommands");
    }

    public void SlideBack(int transition)
    {
        animationPlayer.Play("HideCommands");
        this.transition = (Transition)transition;
    }
    
    private void ExecuteAction(Enemy enemy)
    {
        SlideBack(0);
        GetTree().CallGroup("Enemies", "CannotBeSelected");
        currentSkill.Effect(controlBox, currentCharacter, enemy);
    }

    private void animationFinished(StringName animName)
    {
        if (animName == "HideCommands")
        {
            if (transition == Transition.commands)
            {
                ShowCommands();
            }

            if (transition == Transition.back)
            {
                ShowBack();
            }
        }
    }

    private void attackButtonPressed()
    {
        SlideBack(1);
        GetTree().CallGroup("Enemies", "CanBeSelected");
        currentSkill = currentCharacter.GetSkill(0);
        controlBox.AddDialog("To which enemy?", true);
    }

    private void skillButtonPressed()
    {
        SlideBack(1);
        
    }

    private void backButtonPressed()
    {
        SlideBack(2);
        currentSkill = null;
        GetTree().CallGroup("Enemies", "CannotBeSelected");
    }

    public void SetCurrentCharacter(Character character) { currentCharacter = character; }
    public void PassControlBox(ControlBox controlBox) { this.controlBox = controlBox; }
    public void PassUIAnimations(AnimationPlayer uiAnimations)
    {
        currentCharacter.SetUIAnimations(uiAnimations); 
    }
}
