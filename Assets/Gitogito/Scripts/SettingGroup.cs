using UnityEngine;
using UnityEngine.Events;

public class SettingGroup : MonoBehaviour
{
    private SettingElement[] elements;

    [HideInInspector] public int number;
    [HideInInspector] public int value;
    public int defaultValue;

    [SerializeField] private string settingName;

    private void Awake ()
    {
        elements = new SettingElement[transform.childCount];
        for(int i = 0 ; i < transform.childCount ; i++)
        {
            elements[i] = transform.GetChild (i).GetComponent<SettingElement> ();
            elements[i].element = i;
        }
    }

    public void Selected (int element)
    {
        GameObject.FindWithTag ("GameSystem").SendMessage ("SettingExplain", number);
        SelectSettingValue (element);
    }

    public void SelectSettingValue (int element)
    {
        SelectSetting (element);
        GameObject.FindWithTag ("GameSystem").SendMessage (settingName, element);
    }

    public void SelectSetting (int element)
    {
        value = element;
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i].ChangeHighLight (i == value);
        }
    }


}
