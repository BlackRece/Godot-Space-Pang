using System.Collections.Generic;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public sealed class FuzzyStateMachine
{
    public enum States
    {
        Idle = 0,
        Chase,
        Wander
    }

    private readonly Entity _agent;
    private readonly Entity _target;

    private Dictionary<States, State> AllStates { get; set; } = [];

    private Dictionary<States, State> ActiveStates { get; set; } = [];
    
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
        States.Wander => new WanderState(_agent, _target),
        _ => new IdleState(_agent, _target)
    };

    public State.Result? Update(double delta)
    {
        State.Result result = null; 
        var wasActive = new List<States>();
        foreach (var key in ActiveStates.Keys)
            wasActive.Add(key);
                    
        ActiveStates.Clear();
        
        foreach (var state in AllStates)
        {
            State.Result temp = null;
            if (state.Value.ToBeActivated())
            {
                if (wasActive.Contains(state.Key))
                    temp = state.Value.Go(delta);
                else
                    state.Value.Enter();

                ActiveStates[state.Key] = state.Value;
            }
            else
            {
                if (wasActive.Contains(state.Key))
                    state.Value.Exit();
            }
            
            if(temp is null)
                continue;

            if (result is null)
            {
                result = temp;
                continue;
            }
            
            result.Velocity += temp.Velocity;
            if (result.Rotation.HasValue)
                result.Rotation += temp.Rotation ?? 0f;
            else
                result.Rotation = temp.Rotation ?? null;
        }

        return result;
    }
}