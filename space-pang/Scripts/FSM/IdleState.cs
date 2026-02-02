using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

internal class IdleState : State
{
    private Vector2 _lastPos;
    
    public IdleState(Entity agent, Entity target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        var hasNotMoved = _lastPos.DistanceSquaredTo(Agent.Position) <= 0f;
        _lastPos = Agent.Position;
        return hasNotMoved;
    }

    public override void Go(double delta) => 
        Agent.Rotate(1 * (float)delta);
}