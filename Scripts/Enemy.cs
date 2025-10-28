using System;
using System.Linq;
using Godot;

public partial class Enemy : Battler
{
    [Signal]
    public delegate void EnemySelectedEventHandler(Enemy enemy);
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
    private int[] elementWeakness = new int[5];
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

    public void Damage(int amount, int elementIndex, ControlBox controlBox)
    {
        int damage = (amount * elementWeakness[elementIndex]) - defense;

        if (damage > 0) { controlBox.AddDialog(name + $" receives {damage} points of damage!", false); }
        else { controlBox.AddDialog(name + " received no damage!", false); }

        if (hp > damage)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
            controlBox.AddDialog(name + " has been defeated!", false);
            animations.Play("defeat");
        }

        HealthBar.Set("value", hp);
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (Array.Exists(statusEffects, s => s == null))
        {
            int index = Array.FindIndex(statusEffects, s => s == null);
            statusEffects[index] = statusEffect;
        }
        else
        {
            int index = Array.FindIndex(statusEffects, s => s == statusEffects.MinBy(s => s.GetPriority()));
            if (statusEffect.GetPriority() >= statusEffects[index].GetPriority())
            {
                statusEffects[index] = statusEffect;
            }
        }
    }

    public void Action(ControlBox controlBox)
    {
        Character[] Party = Global.Instance.GetParty().Where(b => b != null).ToArray();

        if (skills.Length > 1)
        {

        }
        else
        {
            Character target;
            if (Party.Length > 1)
            {
                target = Party.MinBy(b => b.GetHP());

            }
            else
            {
                target = Party[0];
            }
            
            skills[0].Effect(controlBox, this, target);
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

    private void AnimationFinished(StringName animName)
    {
        if (animName == "defeat")
        {
            selectButton.Set("disabled", true);
            ApplyStatusEffect(GD.Load<PackedScene>("res://Scenes/StatusEffects/death.tscn").Instantiate() as StatusEffect);
        }
    }

    private void IsSelected()
    {
        EmitSignal("EnemySelected", this);
    }

    public override string GetBattlerName() { return name; }
    public override int GetSpeed() { return speed; }
    public override int GetAttack() { return attack; }
}
