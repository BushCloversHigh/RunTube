using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static int score ;

    private void Awake ()
    {
        score = 0;
    }

    public static void UpScore(int up)
    {
        score += up;
    }

}
