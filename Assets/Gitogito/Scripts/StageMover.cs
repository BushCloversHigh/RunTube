using UnityEngine;

public class StageMover : MonoBehaviour, IUpdate
{

    [SerializeField] private float initialSpeed = 1f;

    private Transform tubeStage, player;

    public static float playerPosZ = 0;

    private void Awake ()
    {
        GitoBehaviour.AddUpdateList (this);
        tubeStage = GameObject.FindWithTag ("Stage").transform;
        player = GameObject.FindWithTag ("Player").transform;
    }

    public void UpdateMe ()
    {
        if (GameProcessor.proggress == Proggress.GAMEOVER) return;

        float moveSpeed = initialSpeed + Difficulty.speedDifficulty;
        Vector3 dir = Vector3.forward;
        player.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        playerPosZ = player.transform.position.z;
        if (playerPosZ > tubeStage.transform.position.z + 15f)
        {
            tubeStage.transform.position = new Vector3 (0, 0, playerPosZ - 2f);
        }
    }
}

