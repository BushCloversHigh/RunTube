using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Proggress
{
    TITLE,
    PLAYING,
    GAMEOVER
}

public class GameProcessor : SystemUI
{
    public static Proggress proggress;

    private bool animing = false;

    private void Awake ()
    {
        proggress = Proggress.TITLE;
    }

    private void Start ()
    {
        StartCoroutine (LateStart ());
    }

    private IEnumerator LateStart ()
    {
        yield return new WaitForEndOfFrame();
        Transform ui = GameObject.Find ("UI/").transform.GetChild(0);
        ui.Find ("Settings").gameObject.SetActive (false);
        ui.Find ("Ranking").gameObject.SetActive (false);
        ui.Find ("Info").gameObject.SetActive (false);
        ui = GameObject.Find ("UI/").transform.GetChild (1);
        ui.Find ("Settings").gameObject.SetActive (false);
        ui.Find ("Ranking").gameObject.SetActive (false);
        ui.Find ("Info").gameObject.SetActive (false);
        GetComponent<GoogleAdMob> ().RequestBannerTop ();
        yield break;
    }

    private void OnPlayStart ()
    {
        if (animing)
        {
            return;
        }
        proggress = Proggress.PLAYING;
        GetComponent<GoogleAdMob> ().DestroyBanner ();
        GameObject title = GameObject.Find (ScreenRotateManager.UI_Path + "Title");
        GameObject axis = title.transform.Find ("Axis").gameObject;
        GameObject ripple = axis.transform.Find ("Ripple").gameObject;
        GameObject mask = axis.transform.Find ("UnMask").gameObject;
        axis.transform.DOScale (Vector3.one * 2.5f, 1.5f);
        ripple.transform.DOScale (Vector3.one * 15f, 1.0f);
        ripple.GetComponent<SVGImage> ().DOFade (0f, 1.0f);
        mask.transform.DOScale (Vector3.one * 15f, 1.0f);
        SetActiveDelay (title, false, 1.5f);
    }

    private float menuAnimSpeed = 0.5f;

    private void ChangedOrientation ()
    {
        OnSettingBack ();
        OnRankingBack ();
        OnInfoBack ();
    }

    private void OnSettingPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject settings = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Settings").gameObject;
        MenuOpen (settings);
        GetComponent<SettingManager> ().InitSetting ();
    }

    private void OnSettingBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject settings = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Settings").gameObject;
        MenuClose (settings);
        GetComponent<SettingManager> ().SaveSetting ();
    }

    private void OnRankingPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject ranking = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking").gameObject;
        MenuOpen (ranking);
        GetComponent<RankingManager> ().OpenRanking ();
    }

    private void OnRankingBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject ranking = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Ranking").gameObject;
        MenuClose (ranking);
        GetComponent<RankingManager> ().CloseRanking ();
    }

    private void OnInfoPushed ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject info = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Info").gameObject;
        MenuOpen (info);
    }

    private void OnInfoBack ()
    {
        if (animing)
        {
            return;
        }
        Animing ();
        GameObject info = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Info").gameObject;
        MenuClose (info);
    }

    private void MenuOpen(GameObject menu)
    {
        menu.SetActive (true);
        RectTransform rect = menu.transform.GetChild (0).GetComponent<RectTransform> ();
        switch (ScreenRotateManager.currentOrientation)
        {
        case Orientation.LANDSCAPE:
            rect.DOMoveX (Screen.width - 320f, menuAnimSpeed).SetEase (Ease.OutExpo);
            break;
        case Orientation.PORTRAIT:
            rect.DOMoveY (320f, menuAnimSpeed).SetEase (Ease.OutExpo);
            break;
        }
    }

    private void MenuClose (GameObject menu)
    {
        RectTransform rect = menu.transform.GetChild (0).GetComponent<RectTransform> ();
        switch (ScreenRotateManager.currentOrientation)
        {
        case Orientation.LANDSCAPE:
            rect.DOMoveX (Screen.width + 500f, menuAnimSpeed).SetEase (Ease.OutExpo);
            break;
        case Orientation.PORTRAIT:
            rect.DOMoveY (-600f, menuAnimSpeed).SetEase (Ease.OutExpo);
            break;
        }
        SetActiveDelay (menu, false, menuAnimSpeed);
    }

    private void OnHited ()
    {
        if(proggress == Proggress.GAMEOVER)
        {
            return;
        }
        proggress = Proggress.GAMEOVER;
        GameObject.FindWithTag ("Audio").GetComponent<AudioManager> ().BGMStop ();
        StartCoroutine (ScreenFlashAndGameOver ());
    }

    private IEnumerator ScreenFlashAndGameOver ()
    {
        GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().enabled = false;
        GameObject flash = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Flash").gameObject;
        flash.gameObject.SetActive (true);
        SVGImage flash_img = flash.transform.Find ("Img").GetComponent<SVGImage>();
        Camera.main.DOShakePosition (1.0f, 3f, 20, 180, true);
        Sequence sequence = DOTween.Sequence ()
            .OnStart (() =>
            {
                flash_img.DOFade (1f, 0.5f);
            })
            .AppendInterval (0.5f)
            .Append (flash_img.DOFade (0f, 0.5f))
            .AppendInterval (0.5f).
            OnKill (() =>
            {
                flash.gameObject.SetActive (false);
            });
        sequence.Play ();
        yield return new WaitForSeconds (1.5f);
        GameObject gameOver = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("GameOver").gameObject;
        RectTransform gameOverRect = gameOver.transform.GetChild (0).GetComponent<RectTransform> ();
        Text scoreText = gameOverRect.transform.Find ("ScoreBoard/Score").GetComponent<Text> ();
        int thisScore = ScoreCounter.score;
        scoreText.text = "Score : " + thisScore + " Pt";
        gameOver.SetActive (true);
        gameOverRect.DOSizeDelta (Vector2.one * Screen.width, 0.7f);

        GetComponent<GoogleAdMob> ().RequestBannerTop ();
        GetComponent<GoogleAdMob> ().RequestBannerBottom ();

        if (thisScore > DataBase.GetBestScore ())
        {
            DataBase.SetBestScore (thisScore);
            DataBase.ApplyData ();
        }
    }

    private void OnRestart ()
    {
        GetComponent<GoogleAdMob> ().DestroyBanner ();
        SceneManager.LoadScene (0);
    }

    private IEnumerator Animing ()
    {
        animing = true;
        yield return new WaitForSeconds (menuAnimSpeed + 0.1f);
        animing = false;
    }
}
