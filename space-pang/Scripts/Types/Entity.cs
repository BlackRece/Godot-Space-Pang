using System;
using System.Collections.Generic;
using Godot;

namespace SpacePang.Scripts.Types;

public partial class Entity : Area2D
{
    [Export] public Vector2 StartingPos { get; set; }

    // radius in pixels?
    [Export] public float DetectionRadius { get; set; } = 50;
    
    [Export] public float RotateSpeed = 1f;
    [Export] public float Accel = 10f;
    [Export] public float Decel = 5f;
    [Export] public float MaxSpeed = 50f;

    private Detector _detector;

    public List<Entity> GetNeighbours() =>
        _detector.GetNeighbours<Entity>();
    
    public Vector2 InputDirection = Vector2.Zero;
    private Vector2 _currentVelocity = Vector2.Zero;

    protected Vector2 Area => GetViewportRect().Size;
    
    public override void _Ready()
    {
        Position = StartingPos;
        _detector = new Detector(this, DetectionRadius);
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
        
        Position = BoundaryWrap(pos);
        //Position = BoundaryLock(pos);
    }
    
    private Vector2 MoveTowards(
        Vector2 current,
        Vector2 target, 
        float maxDistance)
    {
        var direction = target - current;
        var distance = direction.Length();
    
        if (distance <= maxDistance)
            return target; // Reached target
    
        return current + (direction.Normalized() * maxDistance);
    }

    private Vector2 BoundaryLock(Vector2 pos)
    {
        // Check boundaries and wrap velocity component if needed
        if (pos.X <= 1 || pos.X >= Area.X - 1)
            // Reset X velocity if hitting vertical boundaries
            _currentVelocity = _currentVelocity with { X = 0 };

        if (pos.Y <= 1 || pos.Y >= Area.Y - 1)
            // Reset Y velocity if hitting horizontal boundaries
            _currentVelocity = _currentVelocity with { Y = 0 };

        pos.X = Math.Clamp(pos.X, 0, Area.X);
        pos.Y = Math.Clamp(pos.Y, 0, Area.Y);

        return pos;
    }

    private Vector2 BoundaryWrap(Vector2 pos)
    {
        var result = pos;
        
        if (pos.X <= 0)
            result.X = Area.X;
        else if (pos.X >= Area.X)
            result.X = 0;

        if (pos.Y <= 0)
            result.Y = Area.Y;
        else if (pos.Y >= Area.Y)
            result.Y = 0;
        
        return result;
    }
    
    
}

internal sealed class Detector
{
    private readonly CollisionShape2D _shape2d;
    private readonly CircleShape2D _circle2d;

    public float Radius
    {
        get => _circle2d?.Radius ?? 0;
        set => _circle2d.Radius = _circle2d.Radius > value 
            ? _circle2d.Radius : value;
    }
    
    private readonly List<Entity> _neighbors = [];

    public Detector(Entity owner, float radius = 10)
    {
        _circle2d = new CircleShape2D();
        _circle2d.Radius = radius;

        _shape2d = new CollisionShape2D();
        _shape2d.Shape = _circle2d;
        
        owner.AddChild(_shape2d);
        owner.AreaEntered += OnAreaEntered;
        owner.AreaExited += OnAreaExited;
    }

    public List<T> GetNeighbours<T>() where T : Entity
    {
        var neighbours = new List<T>();

        foreach (var neighbor in _neighbors)
        {
            if (neighbor is T)
                neighbours.Add((T)neighbor);
        }
            
        return neighbours;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Entity entity)
            return;
        
        if (!_neighbors.Contains(entity))
            _neighbors.Add(entity);
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is not Entity entity)
            return;

        _neighbors.Remove(entity);
    }
}