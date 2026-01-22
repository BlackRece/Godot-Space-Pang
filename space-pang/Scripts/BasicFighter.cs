using System;
using Godot;

namespace SpacePang.Scripts;

public partial class BasicFighter : Area2D
{
	private FuzzyStateMachine _fsm;
	
	private int _hitPoints = 10;

	private int HitPoints
	{
		get => _hitPoints;
		set
		{
			_hitPoints -= value;
			if (_hitPoints <= 0)
			{
				// send signal/event to say i'm dead!
			}
		}
	}

	[Export] public int MaxHitPoints { get; set; } = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fsm = new();
		_hitPoints = MaxHitPoints;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnAreaEntered(Area2D other)
	{
		if (other is Shots shot)
		{
			TakeDamage(shot.Damage);
			GD.Print($"HP: {_hitPoints}");
			return;
		}
		
		GD.Print($"hit by {other.Name}");
	}

	private void TakeDamage(int damage) => _hitPoints -= Math.Abs(damage);
	
	private Vector2 CalculateWander()
    {
        // Parameters
        var wanderRadius = 100f; // Distance from agent to wander circle
        var wanderDistance = 50f; // Distance along forward axis to circle center
        var wanderStrength = 0.5f; // Controls how much the wander direction changes
        var maxForce = 50f; // Maximum steering force
        
        // Get current forward direction
        var forward = Transform.Y; // Direction agent is facing
        
        // Calculate circle center (in front of agent)
        var circleCenter = Position + forward * wanderDistance;
        
        // Generate random point on circle
        /*
        var randomAngle = (float)(Math.Random * 2 * Math.PI);
        var wanderPoint = circleCenter + new Vector2(
            (float)Math.Cos(randomAngle) * wanderRadius,
            (float)Math.Sin(randomAngle) * wanderRadius
        );
        
        // Calculate desired velocity (seek wander point)
        var desiredVelocity = (wanderPoint - Position).Normalized() * MaxSpeed;
        
        // Calculate steering force
        var steeringForce = desiredVelocity - Velocity;
        // Clamp to max force
        steeringForce = steeringForce.LimitLength(maxForce);
        
        return steeringForce;
        */
        return Vector2.Zero;
    }

	public sealed class Seek
	{
		private Vector2 _origin;

		public Vector2 Origin
		{
			get => _origin;
			set => _origin = value;
		}

		public Seek(Vector2 origin)
		{
			_origin = origin;
		}
		
		public Vector2 Go(Vector2 target) =>
			(target - _origin).Normalized();
	}

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
}