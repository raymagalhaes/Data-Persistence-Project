using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    public string playerName;

    public string HighScoreName;

    public int HighScore;

    string saveFile;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFile = Application.persistentDataPath + "/savefile.json";
        LoadHighScore();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreName;
    }

    public void SaveHighScore()
    {

        SaveData data = new SaveData();
        data.highScore = DataManager.Instance.HighScore;
        data.highScoreName = DataManager.Instance.HighScoreName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(saveFile, json);

    }

    public void LoadHighScore()
    {

        if (File.Exists(saveFile))
        {
            string json = File.ReadAllText(saveFile);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            DataManager.Instance.HighScore = data.highScore;
            DataManager.Instance.HighScoreName = data.highScoreName;
        }
        else
        {
            DataManager.Instance.HighScore = 0;
        }

    }

}
