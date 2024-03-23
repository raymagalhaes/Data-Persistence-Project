using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public Button startButton;
    public TextMeshProUGUI playerNameInput;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        DataManager.Instance.LoadHighScore();
        highScoreText.text = $"Best Score : {DataManager.Instance.HighScoreName} : {DataManager.Instance.HighScore}";
    }

    public void startGame()
    {
        DataManager.Instance.playerName = playerNameInput.text;
        SceneManager.LoadScene("main");
    }

}