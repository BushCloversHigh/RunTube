using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.LWRP;
using UnityEngine.UI;

public enum Setting
{
    CONTROL,
    FRAMERATE,
    ANTIALIASING,
    POSTEFFECT,
    REFLECTION
}

public class SettingManager : MonoBehaviour
{
    [SerializeField] private LightweightRenderPipelineAsset lightweight;

    [SerializeField] private PostProcessProfile postProcess;

    public void SetSetting (Setting setting, int s)
    {
        switch (setting)
        {
        case Setting.CONTROL:
            SetControl (s);
            break;
        case Setting.FRAMERATE:
            SetFrameRate (s);
            break;
        case Setting.ANTIALIASING:
            SetAntialiasing (s);
            break;
        case Setting.POSTEFFECT:
            SetPostEffect (s);
            break;
        case Setting.REFLECTION:
            SetReflection (s);
            break;
        }
        DataBase.SetSettingValue ((int)setting, s);
    }

    public void InitSetting ()
    {
        Transform settingContent = GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Settings/Menu/Content/Viewport/Content");
        for (int i = 0 ; i < settingContent.childCount ; i++)
        {
            settingContent.GetChild (i).GetComponent<SettingGroup> ().InitToggle ();
        }
    }

    public void SaveSetting ()
    {
        DataBase.ApplyData ();
    }

    private void SetControl (int s)
    {
        GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().control = s;
    }

    private void SetFrameRate (int s)
    {
        Application.targetFrameRate = s == 0 ? 30 : 60;
    }

    private void SetAntialiasing (int s)
    {
        lightweight.msaaSampleCount = s == 0 ? 1 : s * 2;
    }

    private void SetPostEffect (int s)
    {
        Bloom bloom = postProcess.GetSetting<Bloom> ();
        bloom.enabled.value = s == 1;
        lightweight.supportsHDR = s == 1;
    }

    private void SetReflection (int s)
    {
        GameObject.Find ("Player/Collider/Reflection Probe").SetActive (s == 1);
    }

    private string[] explains_jp =
    {
        "このゲームの操作方法を選択できます。「タッチパネルをスライドして操作する方法」(左)と、「端末を傾けて操作する方法」(右)の2通りです。",
        "描画のヌルヌルさの設定です。60だとヌルヌル動きますが、負荷が高くなります。30だと負荷は減りますが、動きがモッサリになります。",
        "オブジェクトの輪郭のギザギザを目立たなくする設定です。数字が高いほど、ギザギザが軽減されますが、負荷が高くなります。",
        "画面効果の設定です。オンにすると光の表現がリッチになりますが、負荷が高くなります。オフにすると負荷は減りますが、映像がノッペリします。",
        "鏡面反射の設定です。オンにすると、壁やオブジェクトが反射するようになります。負荷はとても高いです。"
    };

    private string[] explains_en =
    {
        "You can choose how to operate this game. There are two ways, “How to slide and operate the touch panel” (left) and “How to tilt and operate the terminal” (right).",
        "It is setting of the smoothness of drawing. If it is 60, it works smoothly but the load is high. If it is 30, the load will be reduced, but the movement will be sluggish.",
        "It is a setting that makes the jagged edge of the object's outline less noticeable. The higher the number, the less jaggedness, but the higher the load.",
        "It is setting of the screen effect. When turned on, the light expression will be rich, but the load will be high. Off will reduce the load, but will make the image cheaper.",
        "Specular reflection setting. When turned on, walls and objects will be reflected. The load is very high."

    };

    public void SettingExplain (int setting)
    {
        if (setting == 100)
        {
            GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Settings/Menu/Content/Explain").GetComponent<Text> ().text = "";
        }
        else
        {
            GameObject.Find (ScreenRotateManager.UI_Path).transform.Find ("Settings/Menu/Content/Explain").GetComponent<Text> ().text = Localize.GetLocalizeString(explains_jp[setting], explains_en[setting]);
        }

    }
}
