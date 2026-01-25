using Godot;

namespace SpacePang.Scripts.FSM;

public abstract class State
{
    protected Area2D _agent; // Reference to the enemy using this state
    protected readonly Area2D _target;
    
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