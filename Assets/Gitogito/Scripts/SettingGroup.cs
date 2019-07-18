using UnityEngine;

public class SettingGroup : MonoBehaviour
{
    [SerializeField] private Setting setting;

    [HideInInspector] public int number;
    public int defaultValue;

    private void Awake ()
    {
        Selected (DataBase.GetSettingValue ((int)setting, defaultValue));
    }

    public void Selected (int element)
    {
        GameObject.FindWithTag ("GameSystem").GetComponent<SettingManager> ().SetSetting (setting, element);
        ToggleFix (element);
        GameObject.FindWithTag ("GameSystem").GetComponent<SettingManager> ().SettingExplain ((int)setting);
    }

    public void ToggleFix (int element)
    {
        SettingElement[] elements = GetChildSettingEllements ();
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i].ChangeHighLight (i == element);
        }
    }

    public void InitToggle ()
    {
        SettingElement[] elements = GetChildSettingEllements ();
        int element = DataBase.GetSettingValue ((int)setting, defaultValue);
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i].ChangeHighLight (i == element);
        }
        GameObject.FindWithTag ("GameSystem").GetComponent<SettingManager> ().SettingExplain (100);
    }

    private SettingElement[] GetChildSettingEllements ()
    {
        SettingElement[] elements = new SettingElement[transform.childCount - 1];
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i] = transform.GetChild (i + 1).GetComponent<SettingElement> ();
            elements[i].element = i;
        }
        return elements;
    }
}
