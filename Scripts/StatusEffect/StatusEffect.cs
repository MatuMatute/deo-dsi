using Godot;

public partial class StatusEffect : PanelContainer
{
    [Export]
    private byte priority;
    [Export]
    private byte duration;
    [Export]
    private int power;

    virtual public void Effect()
    {

    }
    
    public int GetPriority() { return priority; }
}
