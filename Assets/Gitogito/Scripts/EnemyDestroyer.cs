using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{

    [SerializeField] private int upScore = 10;

    private void Update ()
    {
        if (transform.position.z < StageMover.playerPosZ - 5)
        {
            ScoreCounter.UpScore (upScore * ((int)Difficulty.speedDifficulty + 1));
            Destroy (gameObject);
        }
    }
}
