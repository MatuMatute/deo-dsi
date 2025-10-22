using Godot;

public partial class CharacterBox : PanelContainer
{
    [Export]
    private Label currentHP;
    [Export]
    private Label maximumHP;
    [Export]
    private Label currentSP;
    [Export]
    private Label maximumSP;
    private Character character;

    public override void _Ready()
    {
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        currentHP.Set("text", character.GetHP().ToString());
        maximumHP.Set("text", character.GetMaxHP().ToString());
        currentSP.Set("text", character.GetSP().ToString());
        maximumSP.Set("text", character.GetMaxSP().ToString());
    }

    public void SetCharacter(Character character) { this.character = character; }
}
