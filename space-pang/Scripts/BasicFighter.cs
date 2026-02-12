using System;
using System.Collections.Generic;
using Godot;
using SpacePang.Scripts.FSM;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts;

public partial class BasicFighter : Entity
{
	private FuzzyStateMachine<BasicFighter> _fsm;
	private Detector<BasicFighter> _detector;
	public List<Entity> Neighbours => _detector.GetNeighbours();
	
	// radius in pixels?
	[Export] public float DetectionRadius { get; set; } = 100;
	
    [Signal] public delegate void DeathEventHandler(BasicFighter agent);
	
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
				EmitSignal(nameof(DeathEventHandler), this);
				this.QueueFree();
			}
		}
	}

	[Export] public int MaxHitPoints { get; set; } = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// TODO: pass in stats from enemy manager
		Accel = 1f;
		Decel = 0.5f;
		MaxSpeed = 2.5f;

		StartingPos = new Vector2(Area.X / 2, 50f);

		/* Flocking: required setup */
		_detector = Detector<BasicFighter>.Register(this, DetectionRadius);
		AddChild(_detector);

		InputDirection = Vector2.Down;
		/* !Flocking */
		
		// TODO: pass states in from enemy manager
		var states = new Dictionary<FuzzyStates, float>
		{
			//[FuzzyStateMachine.States.Idle] = 1f,
			//[FuzzyStateMachine.States.Chase] = 100f
			//[FuzzyStates.Wander] = 1f
			[FuzzyStates.Flock] = 1f
		};

		// TODO: pass player in from signal/enemymanager
		var target = GetTree().Root.GetNode<Entity>("GameArea/Player");
		
		_fsm = new FuzzyStateMachine<BasicFighter>(this, target, states);
		_hitPoints = MaxHitPoints;

		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_hitPoints < 0)
			QueueFree();
		;
		var result = _fsm.Update(delta);

		if (result is not null)
		{
			var targetRotation = result.Rotation ?? result.Velocity.Angle();
			Rotation = Mathf.LerpAngle(Rotation, targetRotation, RotateSpeed * (float)delta);
			InputDirection = result.Velocity;
		}

		base._Process(delta);
	}
	
	private void OnAreaEntered(Area2D other)
	{
		if (other is Shots shot)
			TakeDamage(shot.Damage);
	}

	private void TakeDamage(int damage) => 
		_hitPoints -= Math.Abs(damage);
	
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