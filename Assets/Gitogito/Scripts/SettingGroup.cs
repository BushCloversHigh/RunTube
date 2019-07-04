using UnityEngine;

public class SettingGroup : MonoBehaviour
{
    [SerializeField] private Setting setting;

    private SettingElement[] elements;

    [HideInInspector] public int number;
    public int defaultValue;
    
    private void Awake ()
    {
        elements = new SettingElement[transform.childCount];
        for (int i = 0 ; i < transform.childCount ; i++)
        {
            elements[i] = transform.GetChild (i).GetComponent<SettingElement> ();
            elements[i].element = i;
        }
        Selected (DataBase.GetSettingValue ((int)setting, defaultValue));
      
    }

    public void Selected (int element)
    {
        GameObject.FindWithTag ("GameSystem").GetComponent<SettingManager> ().SetSetting (setting, element);
        ToggleFix (element);
    }

    public void ToggleFix (int element)
    {
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i].ChangeHighLight (i == element);
        }
    }

    public void InitToggle ()
    {
        int element = DataBase.GetSettingValue ((int)setting, defaultValue);
        for (int i = 0 ; i < elements.Length ; i++)
        {
            elements[i].ChangeHighLight (i == element);
        }
    }
}
