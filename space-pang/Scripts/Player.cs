using System;
using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts;

public partial class Player : Entity
{
	[Export] public PackedScene BulletScene { get; set; }
	private Timer _bulletTimer;
	[Export] public double ShotDelay { get; set; } = 0.1f;

	[Export] public float BottomOffset { get; set; } = 50;
	
	private Vector2 InputAxis => Input.GetVector(
		negativeX: "kb_left",
		positiveX: "kb_right",
		negativeY: "kb_up",
		positiveY: "kb_down");
	
	private Vector2 InputVelocity =>
		new (
			Math.Clamp(
				Velocity.X + InputAxis.X,
				-MaxVelocity.X,
				MaxVelocity.X),
			Math.Clamp(
				Velocity.Y + InputAxis.Y,
				-MaxVelocity.Y,
				MaxVelocity.Y)
		);

	public override void _Ready()
	{
		_bulletTimer = GetNode<Timer>("ShotClock");
		_bulletTimer.Stop();
		
		var area = GetViewportRect().Size;
		area.Y -= BottomOffset;
		area.X /= 2;
		StartingPos = area;
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if (_bulletTimer.IsStopped() &&
		    Input.IsActionPressed("kb_fire"))
		{
			// Reset ShotClock
			_bulletTimer.Start(ShotDelay);

			SpawnBullet();
		}
		
		// Apply velocity
		InputDirection = InputAxis;
		//Velocity = InputVelocity;
		base._Process(delta);
	}
	
	private void SpawnBullet()
	{
		var shot = BulletScene.Instantiate<Shots>();
		shot.Position = Position;
		GetParent().AddChild(shot);
	}
}