using System.Collections.Generic;
using SpacePang.Scripts.SB;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public enum FuzzyStates
{
    Idle = 0,
    Chase,
    Wander,
    Flock
}

public sealed class FuzzyStateMachine<T> where T : Entity
{

    private readonly T _agent;
    private readonly Entity _target;

    private Dictionary<FuzzyStates, State<T>> AllStates { get; set; } = [];

    private Dictionary<FuzzyStates, State<T>> ActiveStates { get; set; } = [];
    
    public FuzzyStateMachine(
        T agent,
        Entity target,
        Dictionary<FuzzyStates, float>? states = null)
    {
        _agent = agent;
        _target = target;

        if(states is not null)
            AddStates(states);
    }

    public void AddStates(Dictionary<FuzzyStates, float> states)
    {
        foreach (var state in states)
            AddState(state.Key, state.Value);
    }

    public void AddState(FuzzyStates stateType, float activationRange = 10f)
    {
        var state = CreateState(stateType);
        state.SetActivation(new MinMaxValue<float>(
            min: -activationRange,
            max: activationRange,
            mid: 0)
        );
        
        AllStates.Add(stateType, state);
    }

    private State<T> CreateState(FuzzyStates stateType) => stateType switch
    {
        FuzzyStates.Chase => new ChaseState<T>(_agent, _target),
        FuzzyStates.Wander => new WanderState<T>(_agent, _target),
        FuzzyStates.Flock => new Flocking<T>(_agent, _target),
        _ => new IdleState<T>(_agent, _target)
    };

    public State<T>.Result? Update(double delta)
    {
        State<T>.Result? result = null; 
        var wasActive = new List<FuzzyStates>();
        foreach (var key in ActiveStates.Keys)
            wasActive.Add(key);
                    
        ActiveStates.Clear();
        
        foreach (var state in AllStates)
        {
            State<T>.Result? temp = null;
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