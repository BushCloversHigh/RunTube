using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.Collections.Generic;

public class RankingManager : Toast
{

    private GameObject rankerBoard, rankerElement;

    private InputField nameField;

    private bool top10 = true;

    private void Start ()
    {
        Debug.Log (DataBase.GetUserName ());

        NCMBSettings.ApplicationKey = DataStrings.NCMBAPPKEY;
        NCMBSettings.ClientKey = DataStrings.NCMBCLIANTKEY;

        int bestScore = DataBase.GetBestScore ();
        string scoreStr = bestScore == 0 ? "------" : bestScore.ToString ();
        GameObject.Find ("Canvas").transform.Find("Ranking/Your").GetComponent<Text> ().text = "あなたのベストスコア : " + scoreStr + " pt";

        rankerBoard = GameObject.Find ("Canvas/Ranking/LeaderBoard/Viewport/Content");
        rankerElement = rankerBoard.transform.Find ("Element").gameObject;
        rankerElement.SetActive (false);

        nameField = GameObject.Find ("Canvas/Ranking/NameField").GetComponent<InputField> ();
        nameField.text = DataBase.GetUserName ();

    }

    private void OpenRanking ()
    {
        GetRanking_Top ();
    }

    private void CloseRanking ()
    {
        DataBase.ApplyData ();
    }

    private void OnChangeRanking ()
    {
        if (top10)
        {
            GetRanking_Rivals ();
        }
        else
        {
            GetRanking_Top();
        }
    }

    private void OnScoreUpload ()
    {
        int bestScore = DataBase.GetBestScore ();
        if(bestScore == 0)
        {
            Show ("まだスコアがありません");
            return;
        }

        string upName = nameField.text;
        if (string.IsNullOrEmpty (upName))
        {
            Show ("名前を入力してください");
            return;
        }

        DataBase.SetUserName (upName);
        string id = DataBase.GetScoreID ();
        if (string.IsNullOrEmpty (id))
        {
            NewUserUpload (upName, bestScore);
        }
        else
        {
            ScoreUpdate (id, upName, bestScore);
        }
    }

    private void NewUserUpload (string upName, int bestScore)
    {
        NCMBObject obj = new NCMBObject ("ScoreRanking");
        obj["Name"] = upName;
        obj["Score"] = bestScore;
        obj.SaveAsync ((NCMBException ee) => {
            if (ee == null)
            {
                GetRanking_Top ();
                DataBase.SetScoreID (obj.ObjectId);
                DataBase.ApplyData ();
            }
            else
            {
                Show ("エラーが発生してしまいました。");
            }
        });
    }

    private void ScoreUpdate (string id, string upName, int bestScore)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.WhereEqualTo ("objectId", id);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null || objList.Count == 0)
            {
                Show ("エラーが発生してしまいました。");
                return;
            }

            int cloudScore = System.Convert.ToInt32 (objList[0]["Score"]);
            if (bestScore >= cloudScore)
            {
                objList[0]["Score"] = bestScore;
                objList[0]["Name"] = upName;
                objList[0].SaveAsync ((NCMBException ee) =>
                {
                    if (ee != null)
                    {
                        Show ("エラーが発生してしまいました。");
                        return;
                    }

                    if (top10)
                    {
                        GetRanking_Top ();
                    }
                    else
                    {
                        GetRanking_Rivals ();
                    }


                });
            }
        });
    }

    private void GetRanking_Top ()
    {
        top10 = true;
        GameObject.Find ("Canvas/Ranking/Change/Text").GetComponent<Text> ().text = "あなたの順位";
        RankerReset ();

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.OrderByDescending ("Score");
        query.Limit = 30;
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                Show ("エラーが発生してしまいました。");
                return;
            }

            int r = 1;
            foreach (NCMBObject obj in objList)
            {
                SetRankerElement (obj, r);
                r++;
            }
        });
    }

    private void GetRanking_Rivals ()
    {
        string scoreID = DataBase.GetScoreID ();
        if (string.IsNullOrEmpty (scoreID))
        {
            Show ("まだあなたのスコアが登録されていません。");
            return;
        }
        top10 = false;
        GameObject.Find ("Canvas/Ranking/Change/Text").GetComponent<Text> ().text = "トップ";
        RankerReset ();

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.WhereEqualTo ("objectId", scoreID);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null || objList.Count == 0)
            {
                Show ("エラーが発生してしまいました。");
                return;
            }

            NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject> ("ScoreRanking");
            rankQuery.WhereGreaterThan ("Score", objList[0]["Score"]);
            rankQuery.CountAsync ((int count, NCMBException ee) =>
            {
                if (ee != null)
                {
                    Show ("エラーが発生してしまいました。");
                    return;
                }
                int rank = count + 1;
                GetRanking_Rivals (rank);
            });
        });
    }

    private void GetRanking_Rivals (int rank)
    {
        int numSkip = rank - 5;
        if (numSkip < 0)
        {
            numSkip = 0;
        }
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.OrderByDescending ("Score");
        query.Skip = numSkip;
        query.Limit = 15;
        query.FindAsync ((List<NCMBObject> objListy, NCMBException e) =>
        {
            if (e != null)
            {
                Show ("エラーが発生してしまいました。");
                return;
            }

            int r = numSkip + 1;
            foreach (NCMBObject obj in objListy)
            {
                SetRankerElement (obj, r);
                r++;
            }
        });
    }

    private void SetRankerElement (NCMBObject obj, int rank)
    {
        string id = obj.ObjectId;
        int s = System.Convert.ToInt32 (obj["Score"]);
        string n = System.Convert.ToString (obj["Name"]);
        GameObject go_ranker = Instantiate (rankerElement, rankerBoard.transform);
        go_ranker.SetActive (true);
        go_ranker.transform.Find ("Num").GetComponent<Text> ().text = rank.ToString ();
        go_ranker.transform.Find ("Name").GetComponent<Text> ().text = n;
        go_ranker.transform.Find ("Score").GetComponent<Text> ().text = s.ToString ();
        if (id == DataBase.GetScoreID ())
        {
            go_ranker.transform.Find ("Back").GetComponent<Image> ().color = new Color (0.6f, 0.6f, 0.6f, 1f);
        }
    }

    private void RankerReset ()
    {
        for(int i = 1 ; i < rankerBoard.transform.childCount ; i++)
        {
            Destroy (rankerBoard.transform.GetChild (i).gameObject);
        }
    }
}
