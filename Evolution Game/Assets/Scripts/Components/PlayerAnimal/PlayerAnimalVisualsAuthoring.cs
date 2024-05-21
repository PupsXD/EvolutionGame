using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimalVisualsAuthoring : MonoBehaviour
{
    public Color color;
   // public SpriteRenderer SpriteRenderer;
    //public Color newColor;
    public class PlayerAnimalVisualsBaker : Baker<PlayerAnimalVisualsAuthoring>
    {
        public override void Bake(PlayerAnimalVisualsAuthoring authoring)
        {
            AddComponent(new PlayerAnimalVisuals
            {
                Color = new float4(authoring.color.r, authoring.color.g, authoring.color.b, authoring.color.a),
                OriginalColor = new float4(authoring.color.r, authoring.color.g, authoring.color.b, authoring.color.a)
            });
            
        }
    }
}