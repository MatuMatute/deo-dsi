using Godot;

public partial class StatusEffect : Resource
{
    [Export]
    private int priority;
    [Export]
    private int duration;
    [Export]
    private int power;

    virtual public void Effect()
    {
        
    }
}
