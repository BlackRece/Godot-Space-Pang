using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public sealed class WanderState : State
{
    private readonly Wander _wander;
    
    public WanderState(Entity agent, Entity target) : base(agent, target)
    {
        _wander = new Wander();
    }

    public override bool ToBeActivated()
    {
        return true;
    }

    public override void Go(double delta)
    {
        _agent.InputDirection =
            _wander.Go(_agent.Transform.X, _agent.Position);
        
    }
}