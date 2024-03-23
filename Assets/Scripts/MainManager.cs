using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    string saveFile;


    // Start is called before the first frame update
    void Start()
    {
        saveFile = Application.persistentDataPath + "savefile.json";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        LoadHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SaveHighScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points > DataManager.Instance.HighScore)
        {
            DataManager.Instance.HighScoreName = DataManager.Instance.playerName;
            DataManager.Instance.HighScore = m_Points;
            DisplayHighScore();
        }

    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreName;
    }

    void SaveHighScore()
    {

        SaveData data = new SaveData();
        data.highScore = DataManager.Instance.HighScore;
        data.highScoreName = DataManager.Instance.HighScoreName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(saveFile, json);

    }

    void LoadHighScore()
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

        DisplayHighScore();

    }

    void DisplayHighScore()
    {
        HighScoreText.text = $"High Score : {DataManager.Instance.HighScoreName} : {DataManager.Instance.HighScore}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
