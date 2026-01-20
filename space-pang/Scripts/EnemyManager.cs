//using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace SpacePang.Scripts
{
	public partial class EnemyManager : Node
	{
		[Export] public Dictionary<EnemyTypes, Node2D> EnemyLibrary { get; set; } = [];

		public enum EnemyTypes
		{
			Basic = 0,
			Elite,
			MiniBoss,
			Boss
		}
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}