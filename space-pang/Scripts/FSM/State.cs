using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.FSM;

public abstract class State
{
    protected readonly Entity Agent; // Reference to the enemy using this state
    protected readonly Entity Target;

    private MinMaxValue<float> _activation = MinMaxValue<float>.Empty();

    protected State(Entity agent, Entity target)
    {
        Agent = agent;
        Target = target;
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
    public abstract Result Go(double delta);
    
    // Called when exiting the state
    public virtual void Exit()
    {
    }

    public record Result
    {
        /// <summary>
        /// Desired Rotation
        /// </summary>
        internal float? Rotation { get; set; } = null;
        
        /// <summary>
        /// Desired Velocity
        /// </summary>
        internal Vector2 Velocity { get; set; } = new();
    }
}