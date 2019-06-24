using UnityEngine;

public class DataBase : DataStrings
{
    public static int GetSettingValue (int setting, int defaultValue)
    {
        return PlayerPrefs.GetInt (SETTINGS[setting], defaultValue);
    }

    public static void SetSettingValue(int setting, int value)
    {
        PlayerPrefs.SetInt (SETTINGS[setting], value);
    }

    public static void ApplyData ()
    {
        PlayerPrefs.Save ();
    }

    public static int GetBestScore ()
    {
        return PlayerPrefs.GetInt (SCORE, 0);
    }

    public static void SetBestScore (int score)
    {
        PlayerPrefs.SetInt (SCORE, score);
    }
}
