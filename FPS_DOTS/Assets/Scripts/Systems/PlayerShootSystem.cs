using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public partial struct PlayerShootSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<PlayerShoot>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var playerShootEntity = SystemAPI.GetSingletonEntity<PlayerShoot>();
        var playerShoot = SystemAPI.GetComponent<PlayerShoot>(playerShootEntity);
        var playerCamera = CameraController.Instance.Camera;

        var askToShoot = Input.GetButton("Fire1");

        if (!askToShoot || playerShoot.ShootCooldown > 0)
        {
            playerShoot.ShootCooldown -= SystemAPI.Time.DeltaTime;
            state.EntityManager.SetComponentData(playerShootEntity, playerShoot);
            return;
        }

        playerShoot.ShootCooldown = 0.05f;
        state.EntityManager.SetComponentData(playerShootEntity, playerShoot);

        var physicWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        var collisionWorld = physicWorld.CollisionWorld;
        uint enemyMask = (uint)LayerMask.GetMask("Enemy");

        RaycastInput input = new()
        {
            Start = playerCamera.transform.position + playerCamera.transform.forward,
            End = playerCamera.transform.position + 200 * playerCamera.transform.forward,
            Filter = new CollisionFilter()
            {
                BelongsTo = enemyMask,
                CollidesWith = enemyMask,
                GroupIndex = 0
            }
        };

        bool haveHit = collisionWorld.CastRay(input, out var hit);
        if (haveHit)
        {
            //state.EntityManager.GetComponentData<CollisionFilter>()
            var enemyController = state.EntityManager.GetComponentObject<EnemyAnimatedGO>(hit.Entity);
            GameObject.Destroy(enemyController.EnemyController.gameObject);
            state.EntityManager.DestroyEntity(hit.Entity);
            EnemySpawnSystem.EnemyCount--;
        }
    }
}