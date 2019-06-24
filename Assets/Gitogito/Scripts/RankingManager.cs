using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{

    private void Awake ()
    {
        int bestScore = DataBase.GetBestScore ();
        Debug.Log (bestScore);
        string scoreStr = bestScore == 0 ? "------" : bestScore.ToString ();
        GameObject.Find ("Canvas").transform.Find("Ranking/Your").GetComponent<Text> ().text = "あなたのベストスコア : " + scoreStr + " pt";
    }
}
