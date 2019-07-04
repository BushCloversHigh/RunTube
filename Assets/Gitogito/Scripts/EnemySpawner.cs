using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour, IUpdate
{

    [SerializeField] private GameObject enemyCube, enemyPyramid;
    
    [SerializeField] private float 
        constSpawnInterval = 1f,
        constSpawnProbability = 0.25f,
        patternSpawnInterval = 3f,
        patternSpawnProbability = 0.5f;

    private int spawnPosBase = 0;

    private void Awake ()
    {
        GitoBehaviour.AddUpdateList (this);
    }

    public void UpdateMe ()
    {
        if (GameProcessor.proggress != Proggress.PLAYING) return;

        spawnPosBase = Mathf.CeilToInt (StageMover.playerPosZ) + 38;
        ConstSpawn ();
        PatternSpawn ();
    }

    private float tc = 0;
    private void ConstSpawn ()
    {
        tc += Time.deltaTime;
        float interval = constSpawnInterval - Difficulty.intervalDifficulty;
        float probability = constSpawnProbability + Difficulty.probabiltyDifficulty;

        if (tc < interval)
        {
            return;
        }
        tc = 0;
        if (TrueOrFalse (probability))
        {
            Spawn (TrueOrFalse (0.1f + Difficulty.probabiltyDifficulty) ? enemyCube : enemyPyramid, spawnPosBase, Random.Range (-5, 7));
        }
    }

    private float tp = 0;
    private void PatternSpawn ()
    {
        tp += Time.deltaTime;
        float interval = patternSpawnInterval - Difficulty.intervalDifficulty * 2f;
        float probability = patternSpawnProbability + Difficulty.probabiltyDifficulty;
        if (tp > interval && TrueOrFalse(probability))
        {
            StartCoroutine ("Pattern" + Random.Range (1, 4));
            tp = 0;
        }
    }

    private IEnumerator Pattern1 ()
    {
        int randomRot = Random.Range (-5, 7);
        int randomDir = Random.Range (-1, 2);
        int posZ = spawnPosBase;
        for (int i = 0 ; i < 10 ; i++)
        {
            Spawn (enemyPyramid, posZ + i, randomRot + i * randomDir);
            yield return null;
        }
        yield break;
    }

    private IEnumerator Pattern2 ()
    {
        int randomRot = Random.Range (-5, 7);
        int randomDir = Random.Range (-1, 2);
        int posZ = spawnPosBase;
        for (int i = 0 ; i < 4 ; i++)
        {
            Spawn (enemyPyramid, posZ + i, randomRot + i * randomDir);
            yield return null;
        }
        yield break;
    }

    private IEnumerator Pattern3 ()
    {
        int randomNum = Random.Range (1, 10);
        int[] randomRots = new int[randomNum];
        int posZ = spawnPosBase;
        for (int i = 0 ; i < randomNum ; i++)
        {
            while (true)
            {
                randomRots[i] = Random.Range (-5, 7);
                bool equal = false;
                for (int j = 0 ; j < i ; j++)
                {
                    if (randomRots[i] == randomRots[j])
                    {
                        equal = true;
                    }
                }
                if (!equal)
                {
                    break;
                }
            }
            Spawn (enemyPyramid, posZ, randomRots[i]);
            yield return null;
        }
        yield break;
    }

    private void Spawn (GameObject go, int pos, int rot)
    {
        GameObject go_enemy = Instantiate (go);
        go_enemy.transform.position = new Vector3 (0, 0, pos);
        if (go == enemyPyramid)
        {
            go_enemy.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rot * 30f + (rot != 0 ? Mathf.Abs (rot) / 10f : 0)));
        }
        else
        {
            go_enemy.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rot * 30f));
        }
    }

    private bool TrueOrFalse (float p)
    {
        return p > Random.value;
    }
}
