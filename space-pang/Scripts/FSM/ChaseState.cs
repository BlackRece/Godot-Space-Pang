using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public sealed class ChaseState : State
{
    private readonly Seek _seek;
    
    public ChaseState(Entity agent, Entity target) : base(agent, target)
    {
        _seek = new Seek(Agent.Position);
    }

    public override bool ToBeActivated() => 
        Agent.Position.DistanceSquaredTo(Target.Position) > 1f;

    public override void Go(double delta)
    {
        Agent.InputDirection =
            _seek.Go(Target.Position);
    }
}