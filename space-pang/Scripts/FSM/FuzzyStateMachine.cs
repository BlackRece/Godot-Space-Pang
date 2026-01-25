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

    public enum States
    {
        Idle = 0,
        Chase
    }

    private Area2D _agent;
    private Area2D _target;
    
    public Dictionary<States, State> AllStates { get; private set; }
    private Dictionary<States, State> ActiveStates { get; set; }
    private Dictionary<States, State> InactiveStates { get; set; }
    
    public FuzzyStateMachine(Area2D agent, Area2D target, States[] states)
    {
        _agent = agent;
        _target = target;

        foreach(var state in states)
            AddState(state);
    }

    public void AddStates(Dictionary<States, float> states)
    {
        foreach (var state in states)
            AddState(state.Key, state.Value);
    }

    public void AddState(States state, float activationRange = 10f)
    {
        AllStates.Add(
            state,
            state switch
            {
                States.Chase => new ChaseState(_agent, _target),
                _ => new IdleState(_agent, _target)
            }
        );
    }

    public void Update(float delta)
    {
        
    }
}