using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Components.PlayerAnimal
{
    public partial struct ChangeColorSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerAnimalVisuals>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (visuals, entity) in SystemAPI.Query<RefRW<PlayerAnimalVisuals>>().WithEntityAccess())
            {
                var currentColor = visuals.ValueRW.Color;
            }

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}