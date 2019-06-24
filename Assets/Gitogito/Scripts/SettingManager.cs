using UnityEngine;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private SettingGroup[] settings;

    [SerializeField] private LightweightRenderPipelineAsset lightweight;

    [SerializeField] private PostProcessProfile postProcess;

    private void Start ()
    {
        SetResolution ();
        for (int i = 0 ; i < settings.Length ; i++)
        {
            settings[i].number = i;
            settings[i].SelectSettingValue (DataBase.GetSettingValue (i, settings[i].defaultValue));
        }
    }

    public void InitSetting ()
    {
        for (int i = 0 ; i < settings.Length ; i++)
        {
            settings[i].SelectSetting (DataBase.GetSettingValue (i, settings[i].defaultValue));
        }
    }

    public void SaveSetting ()
    {
        for (int i = 0 ; i < settings.Length ; i++)
        {
            DataBase.SetSettingValue (i, settings[i].value);
        }
        DataBase.ApplyData ();
    }

    private void SetResolution ()
    {
        float screenRate = 720f / Screen.height;
        if (screenRate > 1)
        {
            screenRate = 1;
        }
        int width = (int)(Screen.width * screenRate);
        int height = (int)(Screen.height * screenRate);
        Screen.SetResolution (width, height, true);
    }

    public void SetControl (int s)
    {
        GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().control = s;
    }

    public void SetQuality (int s)
    {
        switch (s)
        {
        case 0:
            SetFrameRate (0);
            SetAntialiasing (0);
            SetPostEffect (0);
            SetReflection (0);
            break;
        case 1:
            SetFrameRate (1);
            SetAntialiasing (1);
            SetPostEffect (1);
            SetReflection (0);
            break;
        case 2:
            SetFrameRate (1);
            SetAntialiasing (2);
            SetPostEffect (1);
            SetReflection (1);
            break;
        }
    }

    public void SetFrameRate (int s)
    {
        Application.targetFrameRate = s == 0 ? 30 : 60;
        settings[2].SelectSetting (s);
    }

    public void SetAntialiasing (int s)
    {
        lightweight.msaaSampleCount = s == 0 ? 1 : s * 2;
        settings[3].SelectSetting (s);
    }

    public void SetPostEffect (int s)
    {
        Bloom bloom = postProcess.GetSetting<Bloom> ();
        bloom.enabled.value = s == 1;
        lightweight.supportsHDR = s == 1;
        settings[4].SelectSetting (s);
    }

    public void SetReflection (int s)
    {
        GameObject.Find ("Player/Collider/Reflection Probe").SetActive (s == 1);
        settings[5].SelectSetting (s);
    }

    private string[] explains =
    {
        "このゲームの操作方法を選択できます。「タッチパネルをスライドして操作する方法」(左)と、「端末を傾けて操作する方法」(右)の2通りです。",
        "グラフィッククオリティのプリセットです。",
        "描画のヌルヌルさの設定です。60だとヌルヌル動きますが、負荷が高くなります。30だと負荷は減りますが、動きがモッサリになります。",
        "3Dオブジェクトの輪郭のギザギザを目立たなくする設定です。数字が高いほど、ギザギザが軽減されますが、負荷が高くなります。",
        "画面効果の設定です。オンにすると光の表現がリッチになりますが、負荷が高くなります。オフにすると負荷は減りますが、映像がノッペリします。",
        "鏡面反射の設定です。オンにすると、壁やオブジェクトが反射するようになります。負荷はとても高いです。"
    };

    private void SettingExplain (int setting)
    {
        GameObject.FindWithTag ("Setting").transform.Find ("Explain").GetComponent<Text> ().text = explains[setting];
    }
}
