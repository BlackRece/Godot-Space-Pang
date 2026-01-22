using System.Collections.Generic;

namespace SpacePang.Scripts.FSM;

public sealed class FuzzyStateMachine
{
    public readonly struct Behaviours
    {
        public static readonly string SEEK = "SEEK";
        public static readonly string WANDER = "WANDER";
        public static readonly string FLEE = "FLEE";
    }
    
    public Dictionary<string, State> States { get; private set; }
}