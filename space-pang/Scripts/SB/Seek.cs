using Godot;

namespace SpacePang.Scripts.SB;

public sealed class Seek
{
    private Vector2 _origin;

    public Vector2 Origin
    {
        get => _origin;
        set => _origin = value;
    }

    public Seek(Vector2 origin)
    {
        _origin = origin;
    }
		
    public Vector2 Go(Vector2 target) =>
        (target - _origin).Normalized();
}