public interface IBattler
{
    public void Damage(int amount, int elementIndex, ControlBox controlBox);
    public bool CheckDeath();
    public string GetBattlerName();
    public int GetAttack();
    public int GetSpeed();
}
