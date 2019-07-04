using DG.Tweening;
using UnityEngine;

public class InformationManager : SystemUI
{
    private void OnPrivacyOpen ()
    {
        GameObject policy = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("PrivacyPolicy").gameObject;
        policy.SetActive (true);
        policy.transform.GetChild(0).GetComponent<RectTransform> ().DOSizeDelta (Vector2.one * 2000f, 0.7f);
    }

    private void OnPrivacyClose ()
    {
        GameObject policy = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("PrivacyPolicy").gameObject;
        policy.transform.GetChild(0).GetComponent<RectTransform> ().DOSizeDelta (Vector2.zero, 0.3f);
        SetActiveDelay (policy, false, 0.3f);
    }

    private void OnLicenseOpen ()
    {
        GameObject license = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("OpenSource").gameObject;
        license.SetActive (true);
        license.transform.GetChild(0).GetComponent<RectTransform> ().DOSizeDelta (Vector2.one * 2000f, 0.7f);
    }

    private void OnLicenseClose ()
    {
        GameObject license = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("OpenSource").gameObject;
        license.transform.GetChild(0).GetComponent<RectTransform> ().DOSizeDelta (Vector2.zero, 0.3f);
        SetActiveDelay (license, false, 0.3f);
    }
}
