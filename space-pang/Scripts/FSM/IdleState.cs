using Godot;

namespace SpacePang.Scripts.FSM;

public class IdleState : State
{
    private Vector2 lastPos = new Vector2(); 
    public IdleState(Area2D agent, Area2D target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        var hasNotMoved = lastPos.DistanceSquaredTo(_agent.Position) <= 0f;
        lastPos = _agent.Position;
        return hasNotMoved;
    }

    public override void Go(double delta)
    {
        _agent.Rotate(1 * (float)delta);
        return;
    }
}