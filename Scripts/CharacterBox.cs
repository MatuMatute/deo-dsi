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
    [Export]
    private Button selectButton;

    public void UpdateLabels(Character character)
    {
        currentHP.Set("text", character.GetHP().ToString());
        maximumHP.Set("text", character.GetMaxHP().ToString());
        currentSP.Set("text", character.GetSP().ToString());
        maximumSP.Set("text", character.GetMaxSP().ToString());
    }

    public void ShowButton() { selectButton.Set("visible", true); }
    public void HideButton() { selectButton.Set("visible", false); }
}
