using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.Collections.Generic;

public class RankingManager : SystemUI
{
    private bool top10 = true;

    [SerializeField] private GameObject rankerElement;

    private void Awake ()
    {
        NCMBSettings.ApplicationKey = DataStrings.NCMBAPPKEY;
        NCMBSettings.ClientKey = DataStrings.NCMBCLIANTKEY;
    }

    private void Start ()
    {
        NCMBSettings.ApplicationKey = DataStrings.NCMBAPPKEY;
        NCMBSettings.ClientKey = DataStrings.NCMBCLIANTKEY;
    }

    public void OpenRanking ()
    {
        int bestScore = DataBase.GetBestScore ();
        string scoreStr = bestScore == 0 ? "------" : bestScore.ToString ();
        Transform ranking = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking");
        ranking.Find ("Menu/Your").GetComponent<Text> ().text = "あなたのベストスコア : " + scoreStr + " pt";
        
        InputField nameField = ranking.Find ("Menu/NameField").GetComponent<InputField> ();
        nameField.text = DataBase.GetUserName ();
        GetRanking_Top ();
    }

    public void CloseRanking ()
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
            ShowToast ("まだスコアがありません");
            return;
        }

        string upName = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking/Menu/NameField").GetComponent<InputField> ().text;
        if (string.IsNullOrEmpty (upName))
        {
            ShowToast ("名前を入力してください");
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
        Loading (true);
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
                ShowToast ("エラーが発生してしまいました。");
            }
            Loading (false);
        });
    }

    private void ScoreUpdate (string id, string upName, int bestScore)
    {
        Loading (true);
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.WhereEqualTo ("objectId", id);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null || objList.Count == 0)
            {
                ShowToast ("エラーが発生してしまいました。");
            }
            else
            {
                int cloudScore = System.Convert.ToInt32 (objList[0]["Score"]);
                if (bestScore >= cloudScore)
                {
                    objList[0]["Score"] = bestScore;
                    objList[0]["Name"] = upName;
                    objList[0].SaveAsync ((NCMBException ee) =>
                    {
                        if (ee != null)
                        {
                            ShowToast ("エラーが発生してしまいました。");
                        }
                        else
                        {
                            if (top10)
                            {
                                GetRanking_Top ();
                            }
                            else
                            {
                                GetRanking_Rivals ();
                            }
                        }
                    });
                }
            }
            Loading (false);
        });
    }

    private void GetRanking_Top ()
    {
        Loading (true);
        top10 = true;
        GameObject.Find (ScreenRotateManager.UI_Path).transform.Find("Ranking/Menu/Change/Text").GetComponent<Text> ().text = "ライバル";
        RankerReset ();
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.OrderByDescending ("Score");
        query.Limit = 30;
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                ShowToast ("エラーが発生してしまいました。");
            }
            else
            {
                int r = 1;
                foreach (NCMBObject obj in objList)
                {
                    SetRankerElement (obj, r);
                    r++;
                }
            }
            Loading (false);
        });
    }

    private void GetRanking_Rivals ()
    {
        string scoreID = DataBase.GetScoreID ();
        if (string.IsNullOrEmpty (scoreID))
        {
            ShowToast ("まだあなたのスコアが登録されていません。");
            return;
        }
        Loading (true);
        top10 = false;
        GameObject.Find (ScreenRotateManager.UI_Path).transform.Find("Ranking/Menu/Change/Text").GetComponent<Text> ().text = "トップ";
        RankerReset ();
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("ScoreRanking");
        query.WhereEqualTo ("objectId", scoreID);
        query.FindAsync ((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null || objList.Count == 0)
            {
                ShowToast ("エラーが発生してしまいました。");
            }
            else
            {
                NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject> ("ScoreRanking");
                rankQuery.WhereGreaterThan ("Score", objList[0]["Score"]);
                rankQuery.CountAsync ((int count, NCMBException ee) =>
                {
                    if (ee != null)
                    {
                        ShowToast ("エラーが発生してしまいました。");
                    }
                    else
                    {
                        int rank = count + 1;
                        GetRanking_Rivals (rank);
                    }
                });
            }
            Loading (false);
        });
    }

    private void GetRanking_Rivals (int rank)
    {
        Loading (true);
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
                ShowToast ("エラーが発生してしまいました。");
            }
            else
            {
                int r = numSkip + 1;
                foreach (NCMBObject obj in objListy)
                {
                    SetRankerElement (obj, r);
                    r++;
                }
            }
            Loading (false);
        });
    }

    private void SetRankerElement (NCMBObject obj, int rank)
    {
        string id = obj.ObjectId;
        int s = System.Convert.ToInt32 (obj["Score"]);
        string n = System.Convert.ToString (obj["Name"]);
        Transform rankerBoard = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking/Menu/LeaderBoard/Viewport/Content");
        GameObject go_ranker = Instantiate (rankerElement, rankerBoard);
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
        Transform rankerBoard = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking/Menu/LeaderBoard/Viewport/Content");
        for (int i = 0 ; i < rankerBoard.childCount ; i++)
        {
            Destroy (rankerBoard.GetChild (i).gameObject);
        }
    }
}
