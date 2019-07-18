using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField] [Multiline] private string jp, en;

    private void Awake ()
    {
        Text text = GetComponent<Text> ();
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = jp;
        }
        else
        {
            text.text = en;
        }
    }
}