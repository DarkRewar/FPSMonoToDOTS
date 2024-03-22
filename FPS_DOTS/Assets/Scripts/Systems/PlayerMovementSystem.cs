using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<PlayerMovement>();
        state.RequireForUpdate<PlayerCamera>();
    }

    void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        var movement = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement = math.normalizesafe(movement);
        var look = new float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        foreach (var physicsMass in SystemAPI.Query<RefRW<PhysicsMass>>().WithAll<Player>())
        {
            physicsMass.ValueRW.InverseInertia = float3.zero;
        }

        foreach ((var playerMovement, var localTransform, var physicsVelocity) in
            SystemAPI.Query<PlayerMovement, RefRW<LocalTransform>, RefRW<PhysicsVelocity>>().WithAll<Player>())
        {
            var velocity = physicsVelocity.ValueRO.Linear;
            velocity.x = playerMovement.MoveSpeed * movement.x;
            velocity.z = playerMovement.MoveSpeed * movement.y;
            physicsVelocity.ValueRW.Linear = localTransform.ValueRO.TransformDirection(velocity);

            localTransform.ValueRW = localTransform.ValueRO.RotateY(dt * playerMovement.LookSpeed * math.radians(look.x));
        }

        foreach ((var playerMovement, var localTransform, var playerCamera) in
            SystemAPI.Query<PlayerMovement, LocalTransform, PlayerCamera>().WithAll<Player>())
        {
            playerCamera.Camera.transform.position = localTransform.Position + playerCamera.Offset;
            var playerRotation = math.degrees(math.Euler(localTransform.Rotation).y);

            var lookY = dt * playerMovement.LookSpeed * look.y;
            playerCamera.CameraAngle = math.clamp(playerCamera.CameraAngle - lookY, -90, 90);
            playerCamera.Camera.transform.localEulerAngles = new Vector2(playerCamera.CameraAngle, playerRotation);
        }
    }
}
