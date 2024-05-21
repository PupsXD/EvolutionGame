using System.Collections;
using System.Collections.Generic;
using Components.PlayerAnimal;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorButtonHandler : MonoBehaviour
{
    public Button changeColorButton;
    public Color newColor; 
    
    private bool isOriginalColor = true;

    void Start()
    {
        
        changeColorButton.onClick.AddListener(OnChangeColorButtonClicked);
    }

    void OnChangeColorButtonClicked()
    {
        // Get the EntityManager and World instance
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        // Query all entities with the PlayerAnimalVisuals component
        var query = entityManager.CreateEntityQuery(typeof(PlayerAnimalVisuals));

        using (var entities = query.ToEntityArray(Unity.Collections.Allocator.TempJob))
        {
            foreach (var entity in entities)
            {
                var visuals = entityManager.GetComponentData<PlayerAnimalVisuals>(entity);
                if (isOriginalColor)
                {
                    visuals.Color = new float4(newColor.r, newColor.g, newColor.b, newColor.a);
                }
                else 
                {
                    visuals.Color = visuals.OriginalColor;
                }
                
                
                entityManager.SetComponentData(entity, visuals);
            }
            isOriginalColor = !isOriginalColor;
        }
        
        
    }
}
