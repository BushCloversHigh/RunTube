using UnityEngine;

public class StageMover : MonoBehaviour
{

    [SerializeField] private float initialSpeed = 1f;

    private Transform tubeStage, player;

    public static float playerPosZ = 0;

    private void Awake ()
    {
        tubeStage = GameObject.FindWithTag ("Stage").transform;
        player = GameObject.FindWithTag ("Player").transform;
    }

    private void FixedUpdate ()
    {
        float moveSpeed = initialSpeed + Difficulty.speedDifficulty;
        Vector3 dir = Vector3.forward;
        float deltaTime = Time.fixedDeltaTime;
        player.transform.position += Vector3.forward * moveSpeed * deltaTime;
        playerPosZ = player.transform.position.z;
        if (playerPosZ > tubeStage.transform.position.z + 15f)
        {
            tubeStage.transform.position = new Vector3 (0, 0, playerPosZ - 2f);
        }
    }

}
    
