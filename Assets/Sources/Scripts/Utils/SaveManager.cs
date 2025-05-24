using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Holds saving and loading methods
/// Add package Newtonsoft Json in package manager from git link
/// https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git#upm
/// </summary>
public static class SaveManager
{
    public static void Save<T>(string key, T data)
    {
        string savedData = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(key, savedData);
        PlayerPrefs.Save();
    }

    public static void Save(string key, int data)
    {
        PlayerPrefs.SetInt(key, data);
        PlayerPrefs.Save();
    }

    public static void Save(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }

    public static void Save(string key, float data)
    {
        PlayerPrefs.SetFloat(key, data);
        PlayerPrefs.Save();
    }

    public static void Save(string key, ulong data)
    {
        PlayerPrefs.SetFloat(key, data);
        PlayerPrefs.Save();
    }

    public static T Load<T>(string key)
    {
        T data = default;

        if (PlayerPrefs.HasKey(key))
        {
            string loadedJson = PlayerPrefs.GetString(key);
            data = JsonConvert.DeserializeObject<T>(loadedJson);
        }
        else
        {
            Save(key, data);
            data = Load<T>(key);
        }

        return data;
    }

    public static string LoadString(string key)
    {
        string data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : "";
        return data;
    }
    
    public static string LoadString(string key, string defaulData)
    {
        string data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : defaulData;
        return data;
    }

    public static int LoadInt(string key)
    {
        int data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
        return data;
    }
    
    public static int LoadInt(string key, int defaultData)
    {
        int data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultData;
        return data;
    }

    public static ulong LoadULong(string key)
    {
        ulong data = PlayerPrefs.HasKey(key) ? (ulong)PlayerPrefs.GetFloat(key) : 0;
        return data;
    }

    public static ulong LoadULong(string key, ulong defaulData)
    {
        ulong data = PlayerPrefs.HasKey(key) ? (ulong)PlayerPrefs.GetFloat(key) : defaulData;
        return data;
    }

    public static float LoadFloat(string key)
    {
        float data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : 0f;
        return data;
    }

    public static float LoadFloat(string key, float defaultData)
    {
        float data = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultData;
        return data;
    }
}