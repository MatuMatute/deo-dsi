using Godot;

public partial class IceBall : Skill
{
    override public async void Effect(ControlBox controlBox, IBattler user, IBattler target)
    {
        int damage = user.GetAttack() + power;
        controlBox.AddDialog(user.GetBattlerName() + " throws an ice ball to " + target.GetBattlerName(), true);

        if (target is Enemy)
        {
            Enemy enemyTarget = target as Enemy;
            AnimatedSprite2D effect = enemyTarget.PlayEffect(animation);
            await ToSignal(effect, "animation_finished");
        }

        if (target is Character)
        {
            Character characterTarget = target as Character;
            AnimationPlayer cameraAnimation = characterTarget.PlayCameraAnimation("Damage");
            await ToSignal(cameraAnimation, "animation_finished");
        }

        target.Damage(damage, element, controlBox);

        if (user is Node)
        {
            Node nodeUser = user as Node;
            nodeUser.EmitSignal("ActionFinished");
        }
    }
}
