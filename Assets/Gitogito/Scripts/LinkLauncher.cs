using UnityEngine;
using UnityEngine.EventSystems;

public class LinkLauncher : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string url;

    public void OnPointerClick (PointerEventData eventData)
    {
        OpenLink (url);
    }

    public static void OpenLink (string url)
    {
        Application.OpenURL (url);
    }
}
