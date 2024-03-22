using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed = 1;
    public float LookSpeed = 1;

    public Camera PlayerCamera;

    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var camera = CameraController.Instance ? CameraController.Instance.Camera : Camera.main;

            var entity = GetEntity(authoring, TransformUsageFlags.WorldSpace);
            AddComponent<Player>(entity);
            AddComponent(entity, new PlayerMovement
            {
                MoveSpeed = authoring.MoveSpeed,
                LookSpeed = authoring.LookSpeed
            });
            AddComponent(entity, new PlayerShoot
            {
                AskToShoot = false,
                ShootCooldown = 0
            });
            AddComponentObject(entity, new PlayerCamera
            {
                Camera = camera,
                Offset = camera.transform.localPosition
            });
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

public struct Player : IComponentData { }

public struct PlayerMovement : IComponentData
{
    public float MoveSpeed;
    public float LookSpeed;
}

public struct PlayerShoot : IComponentData
{
    public bool AskToShoot;
    public float ShootCooldown;
}

public class PlayerCamera : IComponentData
{
    public float CameraAngle = 0;
    public Camera Camera;
    public float3 Offset;
}
