using Godot;

namespace SpacePang.Scripts.FSM;

public class IdleState : State
{
    public IdleState(Area2D agent, Area2D target) : base(agent, target)
    {
    }

    public override void Go(double delta)
    {
        _agent.Rotate(1 * (float)delta);
        return;
    }
}