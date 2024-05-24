using SaveDate;
using UnityEngine;

public static class SC_SaveManager
{
    public static void Save<T>(string kay, T saveDate)
    {
        string jsonDateString =  JsonUtility.ToJson(saveDate, true);
        PlayerPrefs.SetString(kay, jsonDateString);
    }

    public static T Load<T>(string kay) where T: new()
    {
        if (PlayerPrefs.HasKey(kay))
        {
            string loadedString = PlayerPrefs.GetString(kay);
            return JsonUtility.FromJson<T>(loadedString);
        }
        else 
            return new T();
    }
}
