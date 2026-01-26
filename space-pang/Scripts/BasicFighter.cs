using System;
using System.Collections.Generic;
using Godot;
using SpacePang.Scripts.FSM;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts;

public partial class BasicFighter : Entity
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

	private decimal _rotation;
	private decimal Rot => decimal.Round((decimal)RotationDegrees, 2, MidpointRounding.AwayFromZero);
	[Export] public int MaxHitPoints { get; set; } = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// TODO: pass states in from enemy manager
		var states = new Dictionary<FuzzyStateMachine.States, float>
		{
			[FuzzyStateMachine.States.Idle] = 1f,
			[FuzzyStateMachine.States.Chase] = 100f
		};

		// TODO: pass player in from signal/enemymanager
		var target = GetTree().Root.GetNode<Area2D>("GameArea/Player");
		
		_fsm = new(this, target, states);
		_hitPoints = MaxHitPoints;

		_rotation = Rot;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_fsm.Update(delta);
	}
	
	private void OnAreaEntered(Area2D other)
	{
		if (other is Shots shot)
		{
			TakeDamage(shot.Damage);
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
}