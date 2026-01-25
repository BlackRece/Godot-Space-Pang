using System;
using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public abstract class State
{
    protected Area2D _agent; // Reference to the enemy using this state
    protected readonly Area2D _target;

    private MinMaxValue<float> _activation;
    private float activationLevel = 0f;
    private float range = 10f;
    
    public State(Area2D agent, Area2D target)
    {
        _agent = agent;
        _target = target;
    }
    
    public void SetActivation(MinMaxValue<float> activation) => 
        _activation = activation;

    public virtual bool ToBeActivated() => 
        _activation.IsAbove;

    // Called when entering the state
    public virtual void Enter()
    {
    }
    
    // Called each frame while in this state
    public abstract void Go(double delta);
    
    // Called when exiting the state
    public virtual void Exit()
    {
    }
}