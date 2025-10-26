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
    private int[] elementWeakness = new int[4];
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

        characterBox.UpdateLabels(this);
        return damage;
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

    public void PlayCameraAnimation(StringName animName)
    {
        cameraAnimations.Play(animName);
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
