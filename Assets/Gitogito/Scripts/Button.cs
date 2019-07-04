using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private bool highLight = true;

    private Image highLight_img;

    [SerializeField] private string EventName;

    [SerializeField] private SoundEffect soundEffect;

    private void Awake ()
    {
        highLight_img = GetComponentInChildren<Image> ();
    }


    public void OnPointerDown (PointerEventData eventData)
    {
        if (highLight)
        {
            highLight_img.DOFade (0.8f, 0.2f);
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (highLight)
        {
            highLight_img.DOFade (0f, 0.2f);
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        GameObject.FindWithTag ("GameSystem").SendMessage (EventName);
        GameObject.FindWithTag ("Audio").GetComponent<AudioManager> ().SoundEffectPlay (soundEffect);
    }

}
