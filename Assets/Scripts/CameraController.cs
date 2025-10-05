using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Author: Gustavo
    // Controls camera movement
    public float followSpeed;
    public float zDistance;
    private Vector2 currentDistance;

    private void Update()
    {
        currentDistance = Vector3.Lerp(transform.position, Toy.possessedToy.transform.position, 0.01f * followSpeed);
        transform.position = new Vector3(currentDistance.x, currentDistance.y, Toy.possessedToy.transform.position.z - zDistance);
    }
}
