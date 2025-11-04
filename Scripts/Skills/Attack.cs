using Godot;

public partial class Attack : Skill
{
    override public async void Effect(ControlBox controlBox, IBattler user, IBattler target)
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

        if (user is Node)
        {
            Node nodeUser = user as Node;
            nodeUser.EmitSignal("ActionFinished");
        }
    }
}
