using System;
using Godot;

namespace SpacePang.Scripts;

public partial class BasicFighter : Area2D
{
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

	[Export] public int MaxHitPoints { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
}