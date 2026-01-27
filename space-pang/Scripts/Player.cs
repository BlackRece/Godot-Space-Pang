using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts;

public partial class Player : Entity
{
	[Export] public float BottomOffset { get; set; } = 50;

	[Export] public double ShotDelay { get; set; } = 0.1f;
	[Export] public PackedScene BulletScene { get; set; }
	private Timer _bulletTimer;
	
	private Vector2 InputAxis => Input.GetVector(
		negativeX: "kb_left",
		positiveX: "kb_right",
		negativeY: "kb_up",
		positiveY: "kb_down");
	
	public override void _Ready()
	{
		_bulletTimer = GetNode<Timer>("ShotClock");
		_bulletTimer.Stop();
		
		StartingPos = new(
			Area.X / 2,
			Area.Y - BottomOffset);
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
		base._Process(delta);
	}
	
	private void SpawnBullet()
	{
		var shot = BulletScene.Instantiate<Shots>();
		shot.Position = Position;
		GetParent().AddChild(shot);
	}
}