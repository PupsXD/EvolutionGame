using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform,RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<RotateSpeed>>())
        {
            localTransform.ValueRW = localTransform.ValueRW.RotateX(rotateSpeed.ValueRO.RotateSpeedValue * SystemAPI.Time.DeltaTime);
        }
    }

}
