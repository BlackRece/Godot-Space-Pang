using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

internal class IdleState<T> : State<T> where T : Entity
{
    private Vector2 _lastPos;
    
    public IdleState(T agent, Entity target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        var hasNotMoved = _lastPos.DistanceSquaredTo(Agent.Position) <= 0f;
        _lastPos = Agent.Position;
        return hasNotMoved;
    }

    public override Result Go(double delta) => 
        new() { Rotation = Agent.Rotation + 0.1f };
            //Agent.Rotate(1 * (float)delta);
}