using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public readonly partial struct EnemyAspect : IAspect
{
    public readonly Entity Self;

    public readonly RefRW<LocalTransform> Transform;
    public readonly RefRW<PhysicsVelocity> Rigidbody;

    public float3 Position => Transform.ValueRO.Position;

    public quaternion Rotation
    {
        get => Transform.ValueRO.Rotation;
        set => Transform.ValueRW.Rotation = value;
    }

    public float3 Velocity
    {
        get => Rigidbody.ValueRO.Linear;
        set => Rigidbody.ValueRW.Linear = value;
    }
}
