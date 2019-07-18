using UnityEngine;

public static class Localize
{
    public static string GetLocalizeString (string jp, string en)
    {
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            return jp;
        }
        else
        {
            return en;
        }
    }
}
