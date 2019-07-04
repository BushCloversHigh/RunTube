using UnityEngine;

public class EnemyLife : MonoBehaviour, IUpdate
{

    [SerializeField] private int upScore = 10;

    private void Awake ()
    {
        GitoBehaviour.AddUpdateList (this);
    }

    private void OnDestroy ()
    {
        GitoBehaviour.RemoveUpdateList (this);
    }

    private bool isWindAudio = false;
    public void UpdateMe ()
    {
        if (GameProcessor.proggress != Proggress.PLAYING)
        {
            for (int i = 1 ; i < transform.childCount ; i++)
            {
                transform.GetChild (i).gameObject.SetActive (false);
            }
            return;
        }
        if (transform.position.z < StageMover.playerPosZ + 5)
        {
            if (!isWindAudio)
            {
                isWindAudio = true;
                for(int i = 1 ;i<transform.childCount ; i++)
                {
                    transform.GetChild (i).gameObject.SetActive (true);
                }
            }
        }
        if (transform.position.z < StageMover.playerPosZ - 5)
        {
            ScoreCounter.UpScore (upScore * ((int)Difficulty.speedDifficulty + 1));
            Destroy (gameObject);
        }
    }
}
