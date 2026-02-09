using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.SB;

public sealed class Seek : SteeringBehaviour
{
    public Seek(Entity target) : base(target)
    {
    }
		
    public override Vector2 Calculate(Entity agent) => 
        (Target.Position - agent.Position)
            .Normalized() - agent.InputDirection;
}