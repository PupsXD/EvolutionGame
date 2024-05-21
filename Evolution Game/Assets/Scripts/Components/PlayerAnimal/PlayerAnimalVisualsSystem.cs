using Unity.Entities;
using UnityEngine;

    public partial class PlayerAnimalVisualsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var entityManager = EntityManager;
            Entities.WithAll<PlayerAnimalVisuals, PlayerAnimalTag>().
                ForEach((Entity entity, int entityInQueryIndex, ref PlayerAnimalVisuals visuals) =>
                {
                    var spriteRenderer = entityManager.GetComponentObject<SpriteRenderer>(entity);
                    Color newColor = new Color(visuals.Color.x, visuals.Color.y, visuals.Color.z, visuals.Color.w);
                    spriteRenderer.color = newColor;
                }).WithoutBurst().Run();
        }
    }   
