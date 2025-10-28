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
            enemyTarget.Damage(damage, element, controlBox);
        }

        if (target is Character)
        {
            Character characterTarget = target as Character;
            AnimationPlayer cameraAnimation = characterTarget.PlayCameraAnimation("Damage");
            await ToSignal(cameraAnimation, "animation_finished");
            characterTarget.Damage(damage, element, controlBox);
        }
        
        user.EmitSignal("ActionFinished");
    }
}
