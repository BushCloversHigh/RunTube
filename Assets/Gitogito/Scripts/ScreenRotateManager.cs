using UnityEngine;
using System.Collections;

public enum Orientation
{
    LANDSCAPE,
    PORTRAIT
}

[DefaultExecutionOrder (-1)]
public class ScreenRotateManager : MonoBehaviour
{
    public static Orientation currentOrientation = Orientation.PORTRAIT;

    public static string UI_Path = "UI/Portrait/";

    private void Awake ()
    {
        ChangeOrientation (DataBase.GetSavedOrientation ());
    }

    private void ChangeOrientation (Orientation orientation)
    {
        StartCoroutine (ChangeOrientaionCor (orientation));
        DataBase.SaveOrientation (orientation);
    }

    private IEnumerator ChangeOrientaionCor (Orientation orientation)
    {
        yield return null;
        GameObject ui = GameObject.FindWithTag ("UI");
        currentOrientation = orientation;
        switch (orientation)
        {
        case Orientation.LANDSCAPE:
            UI_Path = "UI/Landscape/";
            ui.transform.Find ("Landscape").gameObject.SetActive (true);
            ui.transform.Find ("Portrait").gameObject.SetActive (false);
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            break;
        case Orientation.PORTRAIT:
            UI_Path = "UI/Portrait/";
            ui.transform.Find ("Landscape").gameObject.SetActive (false);
            ui.transform.Find ("Portrait").gameObject.SetActive (true);
            Screen.orientation = ScreenOrientation.Portrait;
            break;
        }
        FixedResolution (orientation);
        AutoRotate (orientation);
    }

    private void FixedResolution (Orientation orientation)
    {
        StartCoroutine (FixedResolutionCor (orientation));
    }

    private IEnumerator FixedResolutionCor (Orientation orientation)
    {
        yield return new WaitForSeconds (0.05f);
        switch (orientation)
        {
        case Orientation.LANDSCAPE:
            Screen.SetResolution (Resolution.screenHeight, Resolution.screenWidth, true);
            break;
        case Orientation.PORTRAIT:
            Screen.SetResolution (Resolution.screenWidth, Resolution.screenHeight, true);
            break;
        }
    }

    private void AutoRotate (Orientation orientation)
    {
        switch (orientation)
        {
        case Orientation.LANDSCAPE:
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            break;
        case Orientation.PORTRAIT:
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            break;
        }
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    private void FixedRotate ()
    {
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
}
