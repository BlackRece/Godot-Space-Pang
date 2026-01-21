using Godot;

namespace SpacePang.Scripts;

public partial class BasicFighter : Area2D
{
	private int _hitPoints = 10;

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
		GD.Print($"hit by {other.Name}");
	}
}