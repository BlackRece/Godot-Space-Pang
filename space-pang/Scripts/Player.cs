using Godot;

namespace SpacePang.Scripts;

public partial class Player : Node2D
{
	[Export] public Vector2 Pos { get; set; } = new Vector2(200, 400);
	[Export] public int Speed { get; set; } = 500;

	private Node2D player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//player = GetNode().tra
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var axis = Input.GetVector(
			negativeX: "kb_left",
			positiveX: "kb_right",
			negativeY: "kb_up",
			positiveY: "kb_down");
		
		Position = Position + ((axis * Speed) * (float)delta);
	}
}