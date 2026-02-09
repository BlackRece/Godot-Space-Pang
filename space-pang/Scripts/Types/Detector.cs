using System.Collections.Generic;
using Godot;

namespace SpacePang.Scripts.Types;

public partial class Detector<T> : Area2D where T : Entity
{
    public CollisionShape2D Shape2d { get; set; }
    private CircleShape2D _circle2d;

    public float Radius
    {
        get => _circle2d?.Radius ?? 0;
        set => _circle2d.Radius = _circle2d.Radius > value 
            ? _circle2d.Radius : value;
    }
    
    private readonly List<Entity> _neighbors = [];

    public static Detector<T> Register(T owner, float radius = 10)
    {
        var detector = new Detector<T>();
        
        var circle2d = new CircleShape2D();
        circle2d.Radius = radius;

        detector.Shape2d = new CollisionShape2D();
        detector.Shape2d.Shape = circle2d;

        detector.AddChild(detector.Shape2d);

        var id = owner.CollisionLayer;
        detector.CollisionLayer = id;
        detector.CollisionMask = id;
        
        detector.AreaEntered += detector.OnAreaEntered;
        detector.AreaExited += detector.OnAreaExited;
        
        return detector;
    }

    public List<Entity> GetNeighbours()
    {
        var neighbours = new List<Entity>();

        foreach (var neighbor in _neighbors)
        {
            if (neighbor is T)
                neighbours.Add(neighbor);
        }
            
        return neighbours;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not T entity)
            return;
        
        if (!_neighbors.Contains(entity))
            _neighbors.Add(entity);
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is T entity)
            _neighbors.Remove(entity);
    }
}