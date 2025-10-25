using System;
using Godot;

public partial class PlayerMargin : MarginContainer
{
    enum Transition { none, back, commands }; 
    [Export]
    private VBoxContainer playerContainer;
    [Export]
    private Button backButton;
    [Export]
    private AnimationPlayer animationPlayer;
    private readonly PackedScene characterBox = ResourceLoader.Load<PackedScene>("res://Scenes/characterbox.tscn");
    private Character[] Party;
    private Transition transition;
    private ControlBox controlBox;

    public override void _Ready()
    {
        Party = Global.Instance.GetParty();

        foreach (Character character in Party)
        {
            if (character != null)
            {
                CharacterBox currentBox = characterBox.Instantiate() as CharacterBox;
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
        controlBox.AddDialog("To which enemy?", true);
    }

    private void backButtonPressed()
    {
        SlideBack(2);
        GetTree().CallGroup("Enemies", "CannotBeSelected");
    }
    
    public void PassControlBox(ControlBox controlBox) { this.controlBox = controlBox; }
}
