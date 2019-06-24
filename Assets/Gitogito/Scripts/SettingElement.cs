using UnityEngine;
using UnityEngine.EventSystems;

public class SettingElement : MonoBehaviour, IPointerClickHandler
{
    private SettingGroup settingGroup;
    [HideInInspector] public int element;

    private void Awake ()
    {
        settingGroup = transform.parent.GetComponent<SettingGroup> ();
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        settingGroup.Selected (element);
    }

    public void ChangeHighLight (bool active)
    {
        transform.GetChild (0).gameObject.SetActive (active);
    }
}
