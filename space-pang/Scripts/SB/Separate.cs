using System.Collections.Generic;
using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.SB;

public class Separate : SteeringBehaviour
{
    public float Gap { get; set; } = 1f;

    public List<Entity> Neighbors{ get; set; } = [];
    
    public Separate(Entity target) : base (target)
    {
    }

    public override Vector2 Calculate(Entity agent)
    {
        var steer = Vector2.Zero;
        int count = 0;

        foreach (var other in Neighbors)
        {
            var distance = agent.Position.DistanceTo(other.Position);
            if (distance > 0 && distance < Gap)
            {
                var diff = (agent.Position - other.Position).Normalized();
                diff /= distance; // Weight by distance
                steer += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steer /= count;
            steer = steer.Normalized() * agent.MaxSpeed;
            return steer - agent.InputDirection;
        }

        return steer;
    }
}