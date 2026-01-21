using Godot;
using Godot.Collections;

namespace SpacePang.Scripts
{
    public partial class EnemyLibrary : Node
    {
        public enum EnemyTypes
        {
            Basic = 0,
            Elite,
            MiniBoss,
            Boss
        }

        [Export] public Dictionary<EnemyTypes, PackedScene> Enemies { get; set; } = [];

        public Node2D GetEnemy(EnemyTypes type) =>
            Enemies.TryGetValue(type, out PackedScene value)
                ? value.Instantiate<Node2D>()
                : new();
    }
}