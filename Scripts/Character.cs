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

    public Character(string name, Color color, int hp, int sp, int attack, int defense, int speed, int experience, int maxExperience)
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
    public int GetHP() { return hp; }
    public int GetMaxHP() { return maxHP; }
    public int GetSP() { return sp; }
    public int GetMaxSP() { return maxSP; }
    public override int GetSpeed() { return speed; }
}
