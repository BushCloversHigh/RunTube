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

    public static Orientation GetSavedOrientation ()
    {
        return (Orientation) PlayerPrefs.GetInt (ORIETATION, (int) Orientation.PORTRAIT);
    }

    public static void SaveOrientation (Orientation value)
    {
        PlayerPrefs.SetInt (ORIETATION, (int)value);
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

    public static string GetScoreID ()
    {
        return PlayerPrefs.GetString (OBJECTID, "");
    }

    public static void SetScoreID (string id)
    {
        PlayerPrefs.SetString (OBJECTID, id);
    }

    public static string GetUserName ()
    {
        return PlayerPrefs.GetString (USERNAME, "");
    }

    public static void SetUserName (string userName)
    {
        PlayerPrefs.SetString (USERNAME, userName);
    }
}
