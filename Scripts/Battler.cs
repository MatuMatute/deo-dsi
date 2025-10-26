using Godot;

abstract public partial class Battler : Node
{
    // Clase base para los combatientes. Estos dos están relacionados de alguna manera, es imposible organizar los turnos si no.
    
    public virtual string GetBattlerName() { return ""; }
    public virtual int GetAttack() { return 0; }
    public virtual int GetSpeed() { return 0; }
}
