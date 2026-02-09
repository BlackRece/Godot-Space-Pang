using System.Collections.Generic;
using Godot;
using SpacePang.Scripts.FSM;
using SpacePang.Scripts.Types;

namespace SpacePang.Scripts.SB;

public class Flocking<T> : State<T> where T : Entity
{
    private readonly Separate _separate;// = new Separate();

    private float _separationWeight = 1f;
    private float _alignmentWeight = 1f;
    private float _cohesionWeight = 1f;
    
    public Flocking(T agent, Entity target) : base (agent, target)
    {
        _separate = new Separate(target);
    }

    public override bool ToBeActivated() => true;

    public override Result Go(double delta)
    {
        var result = new Result();

        if (Agent is BasicFighter fighter)
        {
            var neighbourPos = new List<Vector2>();
            var neighbourVec = new List<Vector2>();

            foreach (var neighbour in fighter.Neighbours)
            {
                neighbourPos.Add(neighbour.Position);
                neighbourVec.Add(neighbour.InputDirection);
            }
            
            result.Velocity = Flock( Agent, neighbourPos, neighbourVec );
        }

        return result;
    }

    public Vector2 Flock(Entity agent, List<Vector2> pos, List<Vector2> vec)
    {

        _separate.Neighbours = pos;
        var separation = _separate.Calculate( agent );
        
        var alignment = Align(agent, vec);
        var cohesion = Cohere(agent, pos);

        // Weight and combine the three behaviors
        separation *= _separationWeight;
        alignment *= _alignmentWeight;
        cohesion *= _cohesionWeight;

        return separation + alignment + cohesion;
    }

    private Vector2 Align(Entity agent, List<Vector2> neighbors)
    {
        var sum = Vector2.Zero;
        int count = 0;

        foreach (var other in neighbors)
        {
            sum += other;
            count++;
        }

        if (count > 0)
        {
            sum /= count;
            sum = sum.Normalized() * agent.MaxSpeed;
            return sum - agent.InputDirection;
        }

        return sum;
    }

    private Vector2 Cohere(Entity agent, List<Vector2> neighbors)
    {
        var sum = Vector2.Zero;
        int count = 0;

        foreach (var other in neighbors)
        {
            sum += other;
            count++;
        }

        if (count > 0)
            sum /= count;
            //return Seek(agent, sum); // Steer toward average position

        return sum;
    }
}