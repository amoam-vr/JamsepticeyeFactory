using UnityEngine;

public class SpiritSeal : MonoBehaviour
{
    public float SealingRadius = 5;
    public Transform radiusVisualizer;

    private void Awake()
    {
        radiusVisualizer.localScale = new Vector3(SealingRadius * 2, SealingRadius * 2, 1);
    }
}
