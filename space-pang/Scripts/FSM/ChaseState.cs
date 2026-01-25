using Godot;

namespace SpacePang.Scripts.FSM;

public sealed class ChaseState : State
{
    public ChaseState(Area2D agent, Area2D target) : base(agent, target)
    {
    }

    public override void Go(double delta)
    {
        throw new System.NotImplementedException();
    }
}