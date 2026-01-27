using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public class IdleState : State
{
    private Vector2 _lastPos;
    
    public IdleState(Entity agent, Entity target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        var hasNotMoved = _lastPos.DistanceSquaredTo(_agent.Position) <= 0f;
        _lastPos = _agent.Position;
        return hasNotMoved;
    }

    public override void Go(double delta) => 
        _agent.Rotate(1 * (float)delta);
}