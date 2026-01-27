using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public sealed class WanderState : State
{
    public WanderState(Entity agent, Entity target) : base(agent, target)
    {
    }

    public override bool ToBeActivated()
    {
        return true;
    }

    public override void Go(double delta)
    {
        var wander = new Wander();
        _agent.InputDirection =
            wander.Go(_agent.Transform.X, _agent.Position);
        
    }
}