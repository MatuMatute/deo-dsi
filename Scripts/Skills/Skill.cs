using Godot;

public partial class Skill : Resource
{
    [Export]
    protected string name;
    [Export]
    protected string tier;
    [Export]
    protected int power;
    [Export]
    protected int element;
    [Export]
    protected int cost;
    [Export]
    protected SpriteFrames animation;
    virtual public async void Effect(ControlBox controlBox, Battler user, Battler target) {}
}
