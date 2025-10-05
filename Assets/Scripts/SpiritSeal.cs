using UnityEngine;

public class SpiritSeal : MonoBehaviour
{
    // Author: Gustavo
    // Marks the location of a spirit seal to reset the level if a toy dies too close to it

    public float SealingRadius = 5;
    public Transform radiusVisualizer;

    private void Awake()
    {
        radiusVisualizer.localScale = new Vector3(SealingRadius * 2, SealingRadius * 2, 1);
    }
}
