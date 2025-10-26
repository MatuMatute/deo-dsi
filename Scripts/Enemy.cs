using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class Enemy : Battler
{
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);
    [Signal]
    public delegate void ActionFinishedEventHandler();
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
    [Export]
    private int[] elementWeakness = new int[4];
    [Export]
    private Skill[] skills = new Skill[4];
    [Export]
    private AnimatedSprite2D effectSprite;
    [Export]
    private AnimationPlayer animations;

    private StatusEffect[] statusEffects = new StatusEffect[4];

    private ProgressBar HealthBar;
    private Button selectButton;

    public override void _Ready()
    {
        hp = maxHP;
        sp = maxSP;
        selectButton = GetNode<Button>("Select");
        CannotBeSelected();
        HealthBar = GetNode<ProgressBar>("HealthBar");
        HealthBar.Set("max_value", maxHP);
        HealthBar.Set("value", hp);
    }

    public int Damage(int amount, int elementIndex)
    {
        int damage = (amount * elementWeakness[elementIndex]) - defense;

        if (hp > damage)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
        }

        HealthBar.Set("value", hp);
        return damage;
    }

    public async void Action(ControlBox controlBox)
    {
        Character[] Party = Global.Instance.GetParty().Where(b => b != null).ToArray();;

        if (skills.Length > 1)
        {

        }
        else
        {
            if (Party.Length > 1)
            {
                Character target = Party.MinBy(b => b.GetHP());
                skills[0].Effect(controlBox, this, target);
            }
            else
            {
                skills[0].Effect(controlBox, this, Party[0]);
                await ToSignal(skills[0], "Finished");
                EmitSignal("ActionFinished");
            }
        }
    }

    public void CanBeSelected()
    {
        selectButton.Set("visible", true);
    }

    public void CannotBeSelected()
    {
        selectButton.Set("visible", false);
    }

    public AnimatedSprite2D PlayEffect(SpriteFrames spriteFrames)
    {
        effectSprite.Set("sprite_frames", spriteFrames);
        effectSprite.Play();
        return effectSprite;
    }

    private void EffectFinished()
    {
        effectSprite.SpriteFrames = null;
        animations.Play("damaged");
    }

    private void IsSelected()
    {
        EmitSignal("EnemySelected", this);
    }

    public override string GetBattlerName() { return name; }
    public override int GetSpeed() { return speed; }
}
