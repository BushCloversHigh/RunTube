using UnityEngine;

public class StageMover : MonoBehaviour
{

    public float moveSpeed = 1f;

    private Transform tubeStage, player;

    private void Awake ()
    {
        tubeStage = GameObject.FindWithTag ("Stage").transform;
        player = GameObject.FindWithTag ("Player").transform;
    }

    private void FixedUpdate ()
    {
        Vector3 dir = Vector3.forward;
        float deltaTime = Time.fixedDeltaTime;
        tubeStage.transform.position += Vector3.forward * moveSpeed * deltaTime;
        player.transform.position += Vector3.forward * moveSpeed * deltaTime;
    }
}
