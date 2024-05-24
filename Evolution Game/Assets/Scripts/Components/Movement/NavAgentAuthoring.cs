using Unity.Entities;
using UnityEngine;

public class NavAgentAuthoring : MonoBehaviour
{
    public float MovementSpeed;
    public float NextPathCalculationTime;
    public Entity TargetEntity;
    public int CurrentWaypoint;
    public bool PathCalculated;
    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float movementSpeed;
    
    private class AuthoringBaker : Baker<NavAgentAuthoring>
    {
        public override void Bake(NavAgentAuthoring authoring)
        {
            Entity authoringEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(authoringEntity, new NavMeshComponent()
            {
                movementSpeed = authoring.MovementSpeed,
                TargetEntity = GetEntity(authoring.targetTransform, TransformUsageFlags.Dynamic),
                
            });
            AddBuffer<WaypontBuffer>(authoringEntity);
        }
    }
        
}