using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{

    private float fadeSpeed = 0.3f;

    public void Show (string message)
    {
        CancelInvoke ("Close");
        SVGImage toast = GameObject.FindWithTag ("Toast").GetComponent<SVGImage> ();
        Text mess = toast.transform.Find ("Text").GetComponent<Text> ();
        mess.text = message;
        toast.DOFade (0f, 0f);
        mess.DOFade (0f, 0f);
        toast.DOFade (0.7f, fadeSpeed);
        mess.DOFade (1f, fadeSpeed);
        Invoke ("Close", 3f);
    }

    private void Close ()
    {
        SVGImage toast = GameObject.FindWithTag ("Toast").GetComponent<SVGImage> ();
        toast.DOFade (0f, fadeSpeed);
        Text mess = toast.transform.Find ("Text").GetComponent<Text> ();
        mess.DOFade (0f, fadeSpeed);
    }

}
