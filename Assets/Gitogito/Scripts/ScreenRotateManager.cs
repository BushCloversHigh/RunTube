using UnityEngine;
using System.Collections;

public enum Orientation
{
    LANDSCAPE,
    PORTRAIT
}

[DefaultExecutionOrder (-1)]
public class ScreenRotateManager : MonoBehaviour, IUpdate
{
    public static Orientation currentOrientation;
    
    public static string UI_Path;

    private int screenWidth = 720, screenHeight = 1280;

    private void Awake ()
    {
        GitoBehaviour.AddUpdateList (this);
        currentOrientation = GetOrientation ();
        ChangeOrientation (currentOrientation);
    }

    private void Start ()
    {
        SetResolution ();

        Invoke ("RotateEnable", 1.0f);
    }

    private void RotateEnable ()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    public void UpdateMe ()
    {
        if (GameProcessor.proggress != Proggress.TITLE)
        {
            return;
        }
        Orientation newOrientation = GetOrientation ();
        if (newOrientation != currentOrientation)
        {
            GameObject.FindWithTag ("GameSystem").SendMessage ("ChangedOrientation");
            currentOrientation = newOrientation;
            ChangeOrientation (newOrientation);
            FixedResolution ();
        }
    }

    private Orientation GetOrientation ()
    {
        if (Screen.currentResolution.width > Screen.currentResolution.height)
        {
            return Orientation.LANDSCAPE;
        }
        else
        {
            return Orientation.PORTRAIT;
        }

    }

    private void ChangeOrientation (Orientation orientation)
    {
        GameObject ui = GameObject.FindWithTag ("UI");
        switch (orientation)
        {
        case Orientation.LANDSCAPE:
            UI_Path = "UI/Landscape/";
            ui.transform.Find ("Landscape").gameObject.SetActive (true);
            ui.transform.Find ("Portrait").gameObject.SetActive (false);
            break;
        case Orientation.PORTRAIT:
            UI_Path = "UI/Portrait/";
            ui.transform.Find ("Landscape").gameObject.SetActive (false);
            ui.transform.Find ("Portrait").gameObject.SetActive (true);
            break;
        }
    }

    private void SetResolution ()
    {
        float screenRate = 1;
        switch (currentOrientation)
        {
        case Orientation.LANDSCAPE:
            screenRate = 720f / Screen.currentResolution.height;
            break;
        case Orientation.PORTRAIT:
            screenRate = 720f / Screen.currentResolution.width;
            break;
        }
        if (screenRate > 1)
        {
            screenRate = 1;
        }
        screenWidth = (int)(Screen.width * screenRate);
        screenHeight = (int)(Screen.height * screenRate);
        Screen.SetResolution (screenWidth, screenHeight, true);
    }

    private void FixedResolution ()
    {
        switch (currentOrientation)
        {
        case Orientation.LANDSCAPE:
            Screen.SetResolution (screenHeight, screenWidth, true);
            break;
        case Orientation.PORTRAIT:
            Screen.SetResolution (screenWidth, screenHeight, true);
            break;
        }
    }
}
