using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public sealed class ChaseState : State
{
    public ChaseState(Entity agent, Entity target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        //return base.ToBeActivated();
        var distance = _agent.Position.DistanceSquaredTo(_target.Position);
        return distance > 1f;
    }

    public override void Go(double delta)
    {
        var seek = new Seek(_agent.Position);
        _agent.InputDirection = seek.Go(_target.Position);
    }
}