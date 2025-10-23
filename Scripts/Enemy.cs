using Godot;

public partial class Enemy : Battler
{
    [Export]
    private string name;
    [ExportCategory("😈 Stats 😈")]
    private int hp;
    [Export]
    private int maxHP { get; set; }
    private int sp;
    [Export]
    private int maxSP { get; set; }
    [Export]
    private int attack { get; set; }
    [Export]
    private int defense { get; set; }
    [Export]
    private int speed { get; set; }

    private StatusEffect[] statusEffects = new StatusEffect[4];

    private ProgressBar HealthBar;

    public override void _Ready()
    {
        hp = maxHP;
        sp = maxSP;
        HealthBar = GetNode<ProgressBar>("HealthBar");
        HealthBar.Set("max_value", maxHP);
        HealthBar.Set("value", hp);
    }

    public void Damage(int amount)
    {
        int damage = amount - defense;

        if (hp > damage)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
        }
    }

    public string GetBattlerName() { return name; }
    public override int GetSpeed() { return speed; }
}
