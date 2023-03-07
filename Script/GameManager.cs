using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverUI;
    public TextMeshProUGUI lvUI;
    public Image xpUI;
    public int lv;
    public float xp;
    private static GameManager _currentGameManager;
    public static GameManager Instance
    {
        get
        {
            if (_currentGameManager == null)
                _currentGameManager = new GameManager();
            return _currentGameManager;
        }
        set
        {
            _currentGameManager = value;
        }
    }
    private void Awake()
    {
        if (_currentGameManager == null)
            Instance = this;
    }

    public void Restart()
    {
        GameOverUI.SetActive(false);
        PlayerController.Instance.gameObject.SetActive(true);
        Maze.Instance.Restart();
        MazeRender.Instance.NewMaze();
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        PlayerController.Instance.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void setXp(int cur, int max)
    {
        xp = (float)cur / (float)max;
        xpUI.fillAmount = xp;
    }

    public void setLv(int lv)
    {
        lvUI.text = "" + lv;
    }
}
