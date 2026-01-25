using System;
using Godot;

namespace SpacePang.Scripts.Types;

public partial class Entity : Area2D
{
    [Export] public Vector2 MaxVelocity { get; set; } = new(10, 10);
    public Vector2 Velocity { get; set; }
    private Vector2 _currentVelocity = Vector2.Zero;
    
    [Export] public int Speed { get; set; } = 500;
    [Export] public float Drag { get; set; } =  0.1f;
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
        // Apply velocity
        var pos = Position + (Velocity * Speed) * (float)delta;
        pos.X = Math.Clamp(pos.X, 0, AreaBounds.X);
        pos.Y = Math.Clamp(pos.Y, 0, AreaBounds.Y);
        Position = pos;

        ApplyDrag();
    }

    private void ApplyDrag()
    {
        // Zero velocity when less than drag
        if (_currentVelocity.X > -Drag &&
            _currentVelocity.X < Drag)
            _currentVelocity.X = 0;
        else
            _currentVelocity.X = (_currentVelocity.X > 0)
                ? _currentVelocity.X - Drag
                : _currentVelocity.X + Drag;

        if (_currentVelocity.Y > -Drag &&
            _currentVelocity.Y < Drag)
            _currentVelocity.Y = 0;
        else
            _currentVelocity.Y = (_currentVelocity.Y > 0)
                ? _currentVelocity.Y - Drag
                : _currentVelocity.Y + Drag;
    }
}