using System.Numerics;

namespace SpacePang.Scripts.Types;

public sealed record MinMaxValue<T> where T : INumber<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
    public T Mid { get; set; }

    private T _current;

    public T Current
    {
        get => _current;
        set => _current = value > Max ? Max : value < Min ? Min : value;
    }

    public MinMaxValue(T min, T max, T mid)
    {
        Min = min;
        Max = max;
        Mid = mid;
        Current = Mid;
    }
    
    public bool IsAbove => Current > Mid;
    public bool IsBelow => Current < Mid;
}