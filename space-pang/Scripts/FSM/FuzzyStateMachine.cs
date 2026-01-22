using System.Collections.Generic;
using Godot;

namespace SpacePang.Scripts.FSM;

public sealed class FuzzyStateMachine
{
    public readonly struct Behaviours
    {
        public static readonly string SEEK = "SEEK";
        public static readonly string WANDER = "WANDER";
        public static readonly string FLEE = "FLEE";
    }
    
    public Dictionary<string, State> States { get; private set; }
}

public abstract class State
{
    protected Area2D _agent; // Reference to the enemy using this state
    private Area2D _target;
    
    public State(Area2D agent, Area2D target)
    {
        _agent = agent;
        _target = target;
    }
    
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