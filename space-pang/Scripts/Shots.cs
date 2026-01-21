using Godot;

namespace SpacePang.Scripts;

public partial class Shots : Area2D
{
	[Export] public int Speed { get; set; } = 300;

	[Signal] public delegate void HitEventHandler();
	
	private Vector2 Velocity => Vector2.Up * Speed;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Velocity * (float)delta;

		if(Position.Y < 0)
			QueueFree();
	}

	public void OnAreaEntered()
	{
		Hide();
		EmitSignal(SignalName.Hit);
	}
}