using System;
using Godot;

namespace SpacePang.Scripts;

public partial class Player : Node2D
{
	[Export] public PackedScene BulletScene { get; set; }
	private Timer _bulletTimer;
	[Export] public double ShotDelay { get; set; } = 0.1f;
	[Export] public Vector2 Pos { get; set; } = new(200, 400);
	[Export] public int Speed { get; set; } = 500;
	[Export] public float Drag { get; set; } =  0.1f;

	[Export] public Vector2 AreaBounds { get; set; } = new(1980, 1080);
	
	private Vector2 InputAxis => Input.GetVector(
		negativeX: "kb_left",
		positiveX: "kb_right",
		negativeY: "kb_up",
		positiveY: "kb_down");

	private Vector2 InputVelocity
	{
		get
		{
			_currentVelocity.X = Math.Clamp(
				_currentVelocity.X + InputAxis.X,
				-MaxVelocity.X,
				MaxVelocity.X);
			_currentVelocity.Y = Math.Clamp(
				_currentVelocity.Y + InputAxis.Y,
				-MaxVelocity.Y,
				MaxVelocity.Y);
			return _currentVelocity;
		}
	}
	
	[Export] public Vector2 MaxVelocity { get; set; } = new(10, 10);
	private Vector2 _currentVelocity = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bulletTimer = GetNode<Timer>("ShotClock");
		_bulletTimer.Stop();
		
		Position = Pos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Apply velocity
		var pos = Position + (InputVelocity * Speed) * (float)delta;
		pos.X = Math.Clamp(pos.X, 0, AreaBounds.X);
		pos.Y = Math.Clamp(pos.Y, 0, AreaBounds.Y);
		Position = pos;

		ApplyDrag();

		if (_bulletTimer.IsStopped() &&
		    Input.IsActionPressed("kb_fire"))
		{
			// Reset ShotClock
			_bulletTimer.Start(ShotDelay);

			SpawnBullet();
		}
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

	private void SpawnBullet()
	{
		var shot = BulletScene.Instantiate<Shots>();
		shot.Position = Position;
		GetParent().AddChild(shot);
	}
}