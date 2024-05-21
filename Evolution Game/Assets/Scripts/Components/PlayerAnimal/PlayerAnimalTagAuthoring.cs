using Unity.Entities;
using UnityEngine;

public class PlayerAnimalTagAuthoring : MonoBehaviour
{
    public class PlayerAnimalTagBaker : Baker<PlayerAnimalTagAuthoring>
    {
        public override void Bake(PlayerAnimalTagAuthoring authoring)
        {
            AddComponent<PlayerAnimalTag>();
        }
    }
}