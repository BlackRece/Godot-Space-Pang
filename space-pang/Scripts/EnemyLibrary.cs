using Godot;
using Godot.Collections;

namespace SpacePang.Scripts;

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
}