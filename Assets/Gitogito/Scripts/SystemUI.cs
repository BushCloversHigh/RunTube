using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SystemUI : MonoBehaviour
{

    public void SetActiveDelay (GameObject obj, bool active, float delay)
    {
        StartCoroutine (SetActiveCor (obj, active, delay));
    }

    private IEnumerator SetActiveCor (GameObject obj, bool active, float delay)
    {
        yield return new WaitForSeconds (delay);
        obj.SetActive (active);
        yield break;
    }

    private float fadeSpeed = 0.3f;
    private bool isShowing = false;
    private IEnumerator cor;
    public void ShowToast (string message)
    {
        if (isShowing)
        {
            StopCoroutine (cor);
        }
        isShowing = true;
        GameObject toast = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Toast").gameObject;
        toast.SetActive (true);
        SVGImage back = toast.transform.GetChild (0).GetComponent<SVGImage> ();
        Text mess = back.transform.Find ("Text").GetComponent<Text> ();
        mess.text = message;
        back.DOFade (0f, 0f);
        mess.DOFade (0f, 0f);
        back.DOFade (0.7f, fadeSpeed);
        mess.DOFade (1f, fadeSpeed);
        cor = Close ();
        StartCoroutine (cor);
    }

    private IEnumerator Close ()
    {
        yield return new WaitForSeconds (3f);
        GameObject toast = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Toast").gameObject;
        SVGImage back = toast.transform.GetChild (0).GetComponent<SVGImage> ();
        back.DOFade (0f, fadeSpeed);
        Text mess = back.transform.Find ("Text").GetComponent<Text> ();
        mess.DOFade (0f, fadeSpeed);
        yield return new WaitForSeconds (fadeSpeed);
        toast.gameObject.SetActive (false);
        isShowing = false;
    }

    private bool loading = false;
    public void Loading (bool t)
    {
        GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Load").gameObject.SetActive (t);
        loading = t;
        StartCoroutine (Kurukuru ());
    }

    private float rotSpeed = 0.6f;

    private IEnumerator Kurukuru ()
    {
        Image cir = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Load/Back/Circle").GetComponent<Image> ();
        cir.fillAmount = 1f;
        bool clock = true;
        while (loading)
        {
            clock = !clock;
            cir.fillClockwise = clock;
            cir.DOFillAmount (clock ? 1f : 0f, rotSpeed);
            cir.transform.DORotate (new Vector3 (0, 0, cir.transform.eulerAngles.z - 180f), rotSpeed);
            yield return new WaitForSeconds (rotSpeed);
        }
        yield break;
    }
}
