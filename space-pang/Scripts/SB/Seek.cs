using Godot;

namespace SpacePang.Scripts.SB;

public sealed class Seek
{
    private Vector2 _origin;

    public Seek(Vector2 origin)
    {
        _origin = origin;
    }
		
    public Vector2 Go(Vector2 target) =>
        (target - _origin).Normalized();
}