using System;
using Godot;
using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

internal sealed class WanderState<T> : State<T> where T : Entity
{
    private readonly Wander _wander;
    
    public WanderState(T agent, Entity target) : base(agent, target)
    {
        _wander = new Wander();
    }

    public override bool ToBeActivated()
    {
        return true;
    }

    public override Result Go(double delta)
    {
        var desiredVelocity=
            _wander.Go(Agent.Transform.X, Agent.Position);
        
        // Calculate angle to face the wander point
        var angleToWander = Mathf.LerpAngle(
            Agent.Rotation,
            desiredVelocity.Angle(),
             Agent.RotateSpeed * (float)delta);
        
        // Apply to agent
        return new Result
        {
            Velocity = desiredVelocity
        };
        
        //Agent.InputDirection = desiredVelocity;
        //Agent.Rotation = angleToWander;
    }
}