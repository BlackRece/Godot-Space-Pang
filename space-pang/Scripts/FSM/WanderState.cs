using System;
using Godot;
using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

internal sealed class WanderState : State
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
        var desiredVelocity=
            _wander.Go(Agent.Transform.X, Agent.Position);
        
        // Calculate angle to face the wander point
        var angleToWander = Mathf.LerpAngle(
            Agent.Rotation,
            desiredVelocity.Angle(),
             Agent.RotateSpeed * (float)delta);
        
        // Apply to agent
        Agent.InputDirection = desiredVelocity;
        Agent.Rotation = angleToWander;
    }
}