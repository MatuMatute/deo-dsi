using Godot;

public partial class CharacterBox : HBoxContainer
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
    [Export]
    private GridContainer statusEffectContainer;

    public void UpdateLabels(Character character)
    {
        currentHP.Set("text", character.GetHP().ToString());
        maximumHP.Set("text", character.GetMaxHP().ToString());
        currentSP.Set("text", character.GetSP().ToString());
        maximumSP.Set("text", character.GetMaxSP().ToString());
    }

    public void UpdateStatusEffects(StatusEffect[] statusEffects)
    {
        
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect != null)
            {
                statusEffectContainer.AddChild(statusEffect);
            }
        }
    }

    public void ShowButton() { selectButton.Set("visible", true); }
    public void HideButton() { selectButton.Set("visible", false); }
}
