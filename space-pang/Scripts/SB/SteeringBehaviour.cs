using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.SB;

public abstract class SteeringBehaviour
{
    public Entity Target { get; set; }

    public abstract Vector2 Calculate(Entity agent);

    public SteeringBehaviour(Entity target)
    {
        Target = target;
    }
}