using BaseTool;
using Unity.Entities;

public class PlayerController : MonoSingleton<PlayerController>
{
    public class Baker : Baker<PlayerController>
    {
        public override void Bake(PlayerController authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        }
    }
}
