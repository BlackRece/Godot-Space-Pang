using System;
using Godot;

namespace SpacePang.Scripts.SB;

public sealed class Wander
{
    // Parameters
    private readonly float _radius;		// Distance from agent to wander circle
    private readonly float _distance;	// Distance along forward axis to circle center
		
    private readonly Random _rnd = new();
    private float Rnd => (float)(_rnd.NextSingle() * 2 * Math.PI);

    public Wander(float radius = 100f, float distance = 50f)
    {
        _radius = radius;
        _distance = distance;
    }

    public Vector2 Go(Vector2 forward, Vector2 position)
    {
        // Calculate circle center (in front of agent)
        var circleCenter = position + forward * _distance;
        
        // Generate random point on circle
        var randomAngle = Rnd;
        var wanderPoint = circleCenter + new Vector2(
            (float)Math.Cos(randomAngle) * _radius,
            (float)Math.Sin(randomAngle) * _radius
        );
        
        // Calculate desired velocity
        return (wanderPoint - position).Normalized();
    }
}