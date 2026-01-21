using System.Collections.Generic;
using Godot;

namespace SpacePang.Scripts
{
	public partial class EnemyManager : Node
	{
		private Timer _timer;

		private EnemyLibrary _lib;
		//private Dictionary<EnemyTypes, Node> _enemyPool { get; set; } = [];
		private List<Node> _enemies = [];
		private Rect2 _area;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_lib = GetNode<EnemyLibrary>("EnemyLibrary");
			
			_timer = GetNode<Timer>("EnemyTimer");
			_timer.Start(0.1);

			_area = GetParent().GetViewport().GetVisibleRect();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_enemies.Count <= 1)
			{
				var enemy = _lib.GetEnemy(EnemyLibrary.EnemyTypes.Basic);
				enemy.Position = new Vector2(_area.Size.X / 2, 50.0f);
				_enemies.Add(enemy);
				AddChild(enemy);
			}
		}
	}
}