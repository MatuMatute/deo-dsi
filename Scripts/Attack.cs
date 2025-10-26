using Godot;

public partial class Attack : Skill
{
    override public async void Effect(ControlBox controlBox, Battler user, Battler target)
    {
        int damage = user.GetAttack() + power;
        controlBox.AddDialog(user.GetBattlerName() + " attacks " + target.GetBattlerName(), true);

        if (target is Enemy)
        {
            Enemy enemyTarget = target as Enemy;
            AnimatedSprite2D effect = enemyTarget.PlayEffect(animation);
            await ToSignal(effect, "animation_finished");
            damage = enemyTarget.Damage(damage, element);
        }

        if (target is Character)
        {
            Character characterTarget = target as Character;
            damage = characterTarget.Damage(damage, element);
            characterTarget.PlayCameraAnimation("Damage");
        }

        EmitSignal("Finished");
        controlBox.AddDialog(target.GetBattlerName() + $" receives {damage} points of damage!", false);
    }
}
