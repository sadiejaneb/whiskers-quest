using UnityEngine;

public class SwapOverlayMaterial : MonoBehaviour
{
    public Material[] faceMaterials; // array to hold different face materials for emotions
    private int currentMaterialIndex = 0;
    private Renderer rend;

    void Start()
    {
        // Get the Renderer component on the cat head GameObject
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Check for Left Alt key press
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            // Update the index
            currentMaterialIndex = (currentMaterialIndex + 1) % faceMaterials.Length;

            // Get the current materials
            Material[] currentMaterials = rend.materials;

            // Change the material for the face (assuming it is at index 1)
            currentMaterials[1] = faceMaterials[currentMaterialIndex];

            // Update the materials
            rend.materials = currentMaterials;
        }
    }
}