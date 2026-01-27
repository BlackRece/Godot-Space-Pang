using System.Collections.Generic;
using Godot;
using SpacePang.Scripts.Types;

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

    private Entity _agent;
    private Entity _target;

    private Dictionary<States, State> AllStates { get; set; } = [];

    private Dictionary<States, State> ActiveStates { get; set; } = [];
    //private Dictionary<States, State> InactiveStates { get; set; }
    
    public FuzzyStateMachine(Entity agent, Entity target, Dictionary<States, float> states = null)
    {
        _agent = agent;
        _target = target;

        if(states != null)
            AddStates(states);
    }

    public void AddStates(Dictionary<States, float> states)
    {
        foreach (var state in states)
            AddState(state.Key, state.Value);
    }

    public void AddState(States stateType, float activationRange = 10f)
    {
        var state = CreateState(stateType);
        state.SetActivation(new MinMaxValue<float>(
            min: -activationRange,
            max: activationRange,
            mid: 0)
        );
        
        AllStates.Add(stateType, state);
    }

    private State CreateState(States stateType) => stateType switch
    {
        States.Chase => new ChaseState(_agent, _target),
        _ => new IdleState(_agent, _target)
    };

    public void Update(double delta)
    {
        /*
         * Psuedo:
         * get all states
         * calculate state activation ( negative = not active, positive = active }
         * list active states
         * list inactive states
         * if an active state becomes inactive, then call state's exit()
         * if an inactive state becomes active, then call state's enter()
         * if was active is still active, then call Go()
         * otherwise do nuttin'
         * TODO:
         * - 
         */
        
        // allstates = all states assigned to this agent, in initial state?
        // activestates = all states with activation value greater than zero
        // inactivestates = all states from allstates not in activestates
        
        var wasActive = new List<States>();
        foreach (var key in ActiveStates.Keys)
            wasActive.Add(key);
                    
        ActiveStates.Clear();
        
        foreach (var state in AllStates)
        {
            if (state.Value.ToBeActivated())
            {
                if (wasActive.Contains(state.Key))
                    state.Value.Go(delta);
                else
                    state.Value.Enter();

                ActiveStates[state.Key] = state.Value;
            }
            else
            {
                if (wasActive.Contains(state.Key))
                    state.Value.Exit();
            }
        }
    }
}