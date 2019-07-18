using UnityEngine;

[CreateAssetMenu]
public class Resolution : ScriptableObject
{
    public static int screenWidth = 720, screenHeight = 1280;

    private void OnEnable ()
    {
        float screenRate = 1;
        screenRate = 720f / Screen.currentResolution.width;
        if (screenRate > 1)
        {
            screenRate = 1;
        }
        screenWidth = (int)(Screen.width * screenRate);
        screenHeight = (int)(Screen.height * screenRate);
        Screen.SetResolution (screenWidth, screenHeight, true);
    }
}
