using Unity.Entities;
using Unity.Mathematics;


public struct NavMeshComponent : IComponentData 
{ 
        public Entity TargetEntity;
        public bool PathCalculated;
        public int CurrentWaypoint;
        public float MovementSpeed;
        public float NextPathCalculationTime;
        public float movementSpeed { get; set; }
}

public struct WaypontBuffer : IBufferElementData
{
        public float3 Waypoint;
}