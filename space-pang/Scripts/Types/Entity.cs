using System;
using Godot;

namespace SpacePang.Scripts.Types;

public partial class Entity : Area2D
{
    [Export] public Vector2 MaxVelocity { get; set; } = new(10, 10);
    public Vector2 Velocity { get; set; }
    
    protected Vector2 InputDirection = Vector2.Zero;
    private Vector2 _currentVelocity = Vector2.Zero;
    [Export] public float Accel = 10f;
    [Export] public float Decel = 5f;
    [Export] public float MaxSpeed = 50f;
    
    [Export] public Vector2 AreaBounds { get; set; } = new(1980, 1080);
    [Export] public Vector2 StartingPos { get; set; }

    private Rect2 _area;
    
    public override void _Ready()
    {
        _area = GetViewportRect();
        Position = StartingPos;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        var targetVelocity = InputDirection * MaxSpeed;
        _currentVelocity = InputDirection.Length() > 0
            ? MoveTowards(
                _currentVelocity, 
                targetVelocity,
                (float)(Accel * delta))
            // Decelerate when no input
            : MoveTowards(
                _currentVelocity, 
                Vector2.Zero,
                (float)(Decel * delta));

        // Apply velocity
        var pos = Position + (_currentVelocity * MaxSpeed) * (float)delta;

        // Check boundaries and reset velocity component if needed
        if (pos.X <= 0 || pos.X >= AreaBounds.X)
            // Reset X velocity if hitting vertical boundaries
            _currentVelocity = _currentVelocity with { X = 0 };
    
        if (pos.Y <= 0 || pos.Y >= AreaBounds.Y)
            // Reset Y velocity if hitting horizontal boundaries
            _currentVelocity = _currentVelocity with { Y = 0 };
        
        pos.X = Math.Clamp(pos.X, 0, AreaBounds.X);
        pos.Y = Math.Clamp(pos.Y, 0, AreaBounds.Y);
        Position = pos;
    }
    
    private Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistance)
    {
        Vector2 direction = target - current;
        float distance = direction.Length();
    
        if (distance <= maxDistance)
            return target; // Reached target
    
        return current + (direction.Normalized() * maxDistance);
    }
}