using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // foreach ((RefRW<LocalTransform> localTransform,RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<RotateSpeed>>())
        // {
        //     localTransform.ValueRW = localTransform.ValueRW.RotateX(rotateSpeed.ValueRO.RotateSpeedValue * SystemAPI.Time.DeltaTime);
        // }
        
        var job = new RotatingJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        job.Schedule();
    }
    
    
    [BurstCompile]
    public partial struct RotatingJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {
            localTransform = localTransform.RotateX(rotateSpeed.RotateSpeedValue * deltaTime);
        }
    }

}
