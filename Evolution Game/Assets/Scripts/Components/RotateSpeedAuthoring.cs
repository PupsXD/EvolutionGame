using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotateSpeedAuthoring : MonoBehaviour
{
    public float rotateSpeedValue;
    
    private class RotateSpeedBaker : Baker<RotateSpeedAuthoring>
    {
        public override void Bake(RotateSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); 
            AddComponent(entity, new RotateSpeed
            {
                RotateSpeedValue = authoring.rotateSpeedValue
            });
        }
    }
}
