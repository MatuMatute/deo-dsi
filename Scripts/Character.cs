using System;
using System.Linq;
using Godot;

public partial class Character : Battler
{
    private string name;
    private Color color;
    private byte level;
    private int experience;
    private int maxExperience;
    private int hp;
    private int maxHP;
    private int sp;
    private int maxSP;
    private int attack;
    private int defense;
    private int speed;
    private StatusEffect[] statusEffects = new StatusEffect[4];
    private Skill[] skills = new Skill[10];
    // [No elemental, Fuego, Agua, Aire, Tierra]
    private int[] elementWeakness = new int[5];
    private CharacterBox characterBox;
    private AnimationPlayer cameraAnimations;
    

    public Character(string name, Color color, int hp, int sp, int attack, int defense, int speed, int experience, int maxExperience, int[] elementWeakness)
    {
        this.name = name;
        this.color = color;
        this.hp = hp;
        maxHP = hp;
        this.sp = sp;
        maxSP = sp;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        level = 1;
        this.experience = experience;
        this.maxExperience = maxExperience;
        this.elementWeakness = elementWeakness;
        skills[0] = ResourceLoader.Load<Skill>("res://Resources/Skills/Attack.tres");
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
            ApplyStatusEffect(GD.Load<PackedScene>("res://Scenes/StatusEffects/death.tscn").Instantiate() as StatusEffect);
        }

        characterBox.UpdateLabels(this);
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
        characterBox.UpdateStatusEffects(statusEffects);
    }

    public void Action(ControlBox controlBox, PlayerMargin playerMargin)
    {
        controlBox.AddDialog(name + " está pensando en qué hacer...", true);
        playerMargin.SetCurrentCharacter(this);
        playerMargin.ShowCommands();
    }

    public void AssignBox(CharacterBox characterBox)
    {
        this.characterBox = characterBox;
        this.characterBox.HideButton();
        this.characterBox.UpdateLabels(this);
    }

    public AnimationPlayer PlayCameraAnimation(StringName animName)
    {
        cameraAnimations.Play(animName);
        return cameraAnimations;
    }

    public void SetUIAnimations(AnimationPlayer uiAnimation) { cameraAnimations = uiAnimation; }

    public override string GetBattlerName() { return name; }
    public int GetHP() { return hp; }
    public int GetMaxHP() { return maxHP; }
    public int GetSP() { return sp; }
    public int GetMaxSP() { return maxSP; }
    public override int GetAttack() { return attack; }
    public override int GetSpeed() { return speed; }
    public Skill GetSkill(int index) { return skills[index]; }
}
