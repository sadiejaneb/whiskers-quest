using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material[] materialsToSwap; // The materials to cycle through
    private SkinnedMeshRenderer skinnedMeshRenderer; // The skinned mesh renderer component on the GameObject
    private int currentMaterialIndex = 0; // The index of the current swapping material

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>(); // Get the skinned mesh renderer component

        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("No SkinnedMeshRenderer found on this GameObject.");
        }
    }

    void Update()
    {
        if (skinnedMeshRenderer == null) return;

        if (Input.GetKeyDown(KeyCode.LeftAlt)) // Check if the left Alt key is pressed
        {
            // Clone the existing materials array
            Material[] currentMaterials = skinnedMeshRenderer.materials;

            for (int i = 0; i < currentMaterials.Length; i++)
            {
                // Check if the material uses an Unlit/Transparent shader
                if (currentMaterials[i].shader.name == "Unlit/Transparent")
                {
                    // Increment the material index, and wrap around if it exceeds the array length
                    currentMaterialIndex = (currentMaterialIndex + 1) % materialsToSwap.Length;

                    // Replace the material
                    currentMaterials[i] = materialsToSwap[currentMaterialIndex];

                    // Update the materials on the SkinnedMeshRenderer
                    skinnedMeshRenderer.materials = currentMaterials;

                    // Optionally, break the loop if you only expect one material to match
                    break;
                }
            }
        }
    }
}