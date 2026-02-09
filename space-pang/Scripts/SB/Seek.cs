using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.SB;

public sealed class Seek : SteeringBehaviour
{
    private readonly Entity _target;

    public Seek(Entity target)
    {
        _target = target;
    }
		
    public override Vector2 Calculate(Entity agent) => 
        (_target.Position - agent.Position)
            .Normalized() - agent.InputDirection;
}