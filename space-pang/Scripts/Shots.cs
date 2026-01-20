using Godot;

namespace SpacePang.Scripts;

public partial class Shots : Node2D
{
	private Vector2 Velocity => Vector2.Up * Speed;
	
	[Export] public int Speed { get; set; } = 300;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Position.Y > 0)
			Position += Velocity * (float)delta;
		else
			QueueFree();
	}
}