using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProcessor : MonoBehaviour
{
    private int process = 0;

    private bool animing = false;

    private void Awake ()
    {
        GetComponent<EnemySpawner> ().enabled = false;
        GetComponent<Difficulty> ().enabled = false;
    }

    private void Start ()
    {
        GameObject canvas = GameObject.Find ("Canvas");
        canvas.transform.Find ("Settings").gameObject.SetActive (false);
        canvas.transform.Find ("Ranking").gameObject.SetActive (false);
        canvas.transform.Find ("Info").gameObject.SetActive (false);
    }

    private void OnPlayStart ()
    {
        if (animing)
        {
            return;
        }
        if (process == 0)
        {
            process = 1;
            StartCoroutine (PlayStartCor ());
        }
    }

    private IEnumerator PlayStartCor ()
    {
        GetComponent<EnemySpawner> ().enabled = true;
        GetComponent<Difficulty> ().enabled = true;
        GameObject title = GameObject.Find ("Canvas/TitlePanel");
        GameObject ripple = GameObject.Find ("Canvas/TitlePanel/Ripple");
        GameObject mask = GameObject.Find ("Canvas/TitlePanel/UnMask");
        title.transform.DOScale (Vector3.one * 2.5f, 1.5f);
        ripple.transform.DOScale (Vector3.one * 15f, 1.0f);
        ripple.GetComponent<SVGImage> ().DOFade (0f, 1.0f);
        mask.transform.DOScale (Vector3.one * 15f, 1.0f);
        yield return new WaitForSeconds (2f);
        title.SetActive (false);
    }

    private float menuAnimSpeed = 0.5f;

    private void OnSettingPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform settingsRect = GameObject.Find ("Canvas").transform.Find ("Settings").GetComponent<RectTransform> ();
        settingsRect.gameObject.SetActive (true);
        settingsRect.DOMoveX (Screen.currentResolution.width - 270f, menuAnimSpeed).SetEase (Ease.OutExpo);
        SendMessage ("InitSetting");
    }

    private void OnSettingBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform settingsRect = GameObject.Find ("Canvas").transform.Find ("Settings").GetComponent<RectTransform> ();
        settingsRect.DOMoveX (Screen.currentResolution.width + 500f, menuAnimSpeed).SetEase(Ease.OutExpo);
        ObjectActiveFalse (settingsRect.gameObject, menuAnimSpeed);
        SendMessage ("SaveSetting");
    }

    private void OnRankingPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform rankingRect = GameObject.Find ("Canvas").transform.Find ("Ranking").GetComponent<RectTransform> ();
        rankingRect.gameObject.SetActive (true);
        rankingRect.DOMoveX (Screen.currentResolution.width - 270f, menuAnimSpeed).SetEase (Ease.OutExpo);
        SendMessage ("OpenRanking");
    }

    private void OnRankingBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform rankingRect = GameObject.Find ("Canvas").transform.Find ("Ranking").GetComponent<RectTransform> ();
        rankingRect.DOMoveX (Screen.currentResolution.width + 500f, menuAnimSpeed).SetEase (Ease.OutExpo);
        ObjectActiveFalse (rankingRect.gameObject, menuAnimSpeed);
        SendMessage ("CloseRanking");
    }

    private void OnInfoPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform infoRect = GameObject.Find ("Canvas").transform.Find ("Info").GetComponent<RectTransform> ();
        infoRect.gameObject.SetActive (true);
        infoRect.DOMoveX (Screen.currentResolution.width - 270f, menuAnimSpeed).SetEase (Ease.OutExpo);
    }

    private void OnInfoBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        RectTransform infoRect = GameObject.Find ("Canvas").transform.Find ("Info").GetComponent<RectTransform> ();
        infoRect.DOMoveX (Screen.currentResolution.width + 500f, menuAnimSpeed).SetEase (Ease.OutExpo);
        ObjectActiveFalse (infoRect.gameObject, menuAnimSpeed);
    }


    private void OnHited ()
    {
        if(process == 2)
        {
            return;
        }
        process = 2;

        StartCoroutine (GameOverCor ());
    }

    private IEnumerator GameOverCor ()
    {
        GetComponent<StageMover> ().enabled = false;
        GetComponent<EnemySpawner> ().enabled = false;
        GetComponent<Difficulty> ().enabled = false;
        ScreenFlash ();
        yield return new WaitForSeconds (1.5f);
        RectTransform gameOver = GameObject.Find ("Canvas").transform.Find("GameOver").GetComponent<RectTransform> ();
        Text scoreText = gameOver.transform.Find ("ScoreBoard/Score").GetComponent<Text> ();
        int thisScore = ScoreCounter.score;
        scoreText.text = "Score : " + thisScore + " Pt";
        gameOver.gameObject.SetActive (true);
        gameOver.DOSizeDelta (Vector2.one * Screen.width, 1f);

        if(thisScore > DataBase.GetBestScore ())
        {
            DataBase.SetBestScore (thisScore);
            DataBase.ApplyData ();
        }
    }

    private void ScreenFlash ()
    {
        GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().enabled = false;
        SVGImage flash = GameObject.Find ("Canvas").transform.Find ("Flash").GetComponent<SVGImage> ();
        flash.gameObject.SetActive (true);
        Camera.main.DOShakePosition (1.0f, 3f, 20, 180, true);
        Sequence sequence = DOTween.Sequence ()
            .OnStart (() =>
            {
                flash.DOFade (1f, 0.5f);
            })
            .AppendInterval (0.5f)
            .Append (flash.DOFade (0f, 0.5f))
            .AppendInterval (0.5f).
            OnKill (() =>
            {
                flash.gameObject.SetActive (false);
            });
        sequence.Play ();
    }

    private void OnRestart ()
    {
        SceneManager.LoadScene (0);
    }

    private void ObjectActiveFalse (GameObject obj, float time)
    {
        StartCoroutine (ObjectActiveFalseCor (obj, time));
    }

    private IEnumerator ObjectActiveFalseCor (GameObject obj, float time)
    {
        yield return new WaitForSeconds (time);
        obj.SetActive (false);
    }

    private IEnumerator Animing ()
    {
        animing = true;
        yield return new WaitForSeconds (menuAnimSpeed + 0.1f);
        animing = false;
    }
}
