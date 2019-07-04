using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InputFieldEffect : MonoBehaviour, IUpdate
{
    private InputField nameField;

    private bool isEditting = false;

    private void Awake ()
    {
        GitoBehaviour.AddUpdateList (this);

        nameField = GetComponent<InputField> ();
    }

    private void OnEnable ()
    {
        if (!string.IsNullOrEmpty (nameField.text))
        {
            RectTransform place = transform.Find ("Placeholder").GetComponent<RectTransform> ();
            place.DOScale (Vector3.one * 0.5f, 0);
        }
    }

    public void UpdateMe ()
    {
        if (nameField.isFocused)
        {
            if (!isEditting)
            {
                OnSelected ();
            }
        }
        else
        {
            if (isEditting)
            {
                OnEndEdit ();
            }
        }
        isEditting = nameField.isFocused;
    }

    private float rippleSpeed = 0.4f;

    private void OnSelected ()
    {
        RectTransform ripple1 = transform.Find ("Ripple").GetComponent<RectTransform> ();
        RectTransform ripple2 = transform.Find ("UnderLine/Ripple").GetComponent<RectTransform> ();
        RectTransform place = transform.Find ("Placeholder").GetComponent<RectTransform> ();
        ripple1.DOSizeDelta (Vector2.one * 500f, rippleSpeed);
        ripple1.GetComponent<SVGImage> ().DOFade (0f, rippleSpeed);
        ripple2.DOSizeDelta (Vector2.one * 500f, rippleSpeed);
        place.DOScale (Vector3.one * 0.5f, rippleSpeed);
        place.gameObject.GetComponent<Text> ().DOColor (new Color (0.55f, 0.88f, 1, 0.8f), rippleSpeed);
    }

    private void OnEndEdit ()
    {
        RectTransform ripple1 = transform.Find ("Ripple").GetComponent<RectTransform> ();
        RectTransform ripple2 = transform.Find ("UnderLine/Ripple").GetComponent<RectTransform> ();
        RectTransform place = transform.Find ("Placeholder").GetComponent<RectTransform> ();
        ripple1.sizeDelta = Vector2.zero;
        ripple1.GetComponent<SVGImage> ().color = new Color (1, 1, 1, 1);
        ripple2.DOSizeDelta (Vector2.zero, rippleSpeed);
        place.gameObject.GetComponent<Text> ().DOColor (new Color (1, 1, 1, 0.8f), rippleSpeed);
        if (string.IsNullOrEmpty (nameField.text))
        {
            place.DOScale (Vector3.one, rippleSpeed);
        }
    }
}
