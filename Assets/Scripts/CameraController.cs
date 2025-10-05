using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Author: Gustavo
    // Controls camera movement
    //Contributors: Andre

    public float followSpeed;
    public float zDistance;
    private Vector2 currentDistance;

    Transform ghost;

    private void Start()
    {
        ghost = GameObject.FindWithTag("Ghost").transform;
        transform.position = Toy.possessedToy.transform.position;
    }

    private void Update()
    {
        Vector3 target = Toy.possessedToy == null ? ghost.position : Toy.possessedToy.transform.position;
         
        currentDistance = Vector3.Lerp(transform.position, target, 0.01f * followSpeed);
        transform.position = new Vector3(currentDistance.x, currentDistance.y, target.z - zDistance);
    }
}
