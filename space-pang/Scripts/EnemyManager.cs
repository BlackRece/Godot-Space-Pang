using System.Collections.Generic;
using System.Text;
using Godot;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts
{
	public partial class EnemyManager : Node
	{
		private Timer _timer;
		private Label _score;
		private RichTextLabel _debug;
		private MinMaxValue<int> _counter = new MinMaxValue<int>(0, 10, 5);

		private EnemyLibrary _lib;
		//private Dictionary<EnemyTypes, Node> _enemyPool { get; set; } = [];
		private List<Node> _enemies = [];
		private int _enemiesSpawned = 0;
		private Vector2 _enemySpawnPos;
		private Rect2 _area;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_debug = GetNode<RichTextLabel>("DebugLabel");
			_score = GetNode<Label>("ScoreLabel");
			if (_score == null)
			{
				GD.PrintErr("ScoreLabel not found!");
				GD.Print("Available children: ");
				foreach (Node child in GetChildren())
				{
					GD.Print(child.Name);
				}
			}
			else
			{
				_score.Text = "Ready";
			}
			
			_lib = GetNode<EnemyLibrary>("EnemyLibrary");
			_enemySpawnPos = new Vector2(_area.Size.X / 2, 50.0f);
			
			_timer = GetNode<Timer>("EnemyTimer");
			_timer.Timeout += OnTimerTimeout;
			_timer.Start(0.1);

			_area = GetParent().GetViewport().GetVisibleRect();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			var enemyStats = new StringBuilder();
			foreach (Entity enemy in _enemies)
			{
				enemyStats.AppendLine($"{enemy.Position}");
			}
			_debug.Text = enemyStats.ToString();
			_score.Text = $"[Time: {_counter.Current}] " +
			              $"[Enemies: {_enemies.Count}/{_enemiesSpawned}] " +
			              $"[SpawnPos: {_enemySpawnPos}]";

			/*
			if (_enemies.Count <= 1)
			{
				var enemy = _lib.GetEnemy(EnemyLibrary.EnemyTypes.Basic);
				enemy.Position = new Vector2(_area.Size.X / 2, 50.0f);
				_enemies.Add(enemy);
				AddChild(enemy);
			}
			*/
		}

		public void OnTimerTimeout()
		{
			if (++_counter.Current > _counter.Max)
			{
				_counter.Current = _counter.Min;
				Spawn();
			}
		}

		private void Spawn()
		{
			if (_enemies.Count <= 10)
			{
				var enemy = _lib.GetEnemy(EnemyLibrary.EnemyTypes.Basic);
				enemy.Position = _enemySpawnPos;
				if (enemy is BasicFighter fighter)
					fighter.Death += OnDeathSignal;
				_enemies.Add(enemy);
				AddChild(enemy);

				_enemiesSpawned++;
			}
		}

		public void OnDeathSignal(Node agent)
		{
			_enemies.Remove(agent);
		}
	}
}