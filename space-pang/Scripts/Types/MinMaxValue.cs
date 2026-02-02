using System.Numerics;

namespace SpacePang.Scripts.Types;

public sealed record MinMaxValue<T> where T : INumber<T>
{
    /// <summary>
    /// Minimum Value
    /// </summary>
    public T Min { get; set; }
    
    /// <summary>
    /// Maximum Value
    /// </summary>
    public T Max { get; set; }
    
    /// <summary>
    /// Threshold Value
    /// </summary>
    public T Mid { get; set; }

    private T _current;

    /// <summary>
    /// Current value that is clamped between Min and Max.
    /// </summary>
    public T Current
    {
        get => _current;
        set => _current = value > Max ? Max : value < Min ? Min : value;
    }

    /// <summary>
    /// Create a new type
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="mid">Threshold value used for Current's starting value</param>
    public MinMaxValue(T min, T max, T mid)
    {
        Min = min;
        Max = max;
        Mid = mid;
        Current = Mid;
    }
    
    public static MinMaxValue<T> Empty() => new (T.Zero, T.One, T.Zero);
    
    /// <summary>
    /// Returns true if Current more than Mid
    /// </summary>
    public bool IsAbove => Current > Mid;
    
    /// <summary>
    /// Returns true if Current less than Mid
    /// </summary>
    public bool IsBelow => Current < Mid;
}