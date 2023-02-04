using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text[] killCounts;
    public RectTransform gameOverScreen;
    public bool Controller;
    static public GameManager Instance { get; private set; }

    private int KillCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Controller = true;
        Application.targetFrameRate = 60; //Flo's Laptop sucks...

        if (Instance != null)
            Debug.LogError("unsuported second GameManager!");
        Instance = this;
    }

    public void OnStart()
    {
        KillCount = 0;
        Time.timeScale = 1;

        gameOverScreen.gameObject.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnEnemyDie()
    {
        KillCount += 1;
        foreach (var killCount in killCounts)
        {
            killCount.text = $"Kills {KillCount}";
        }
    }

    public void OnPlayerDie()
    {
        //GAME_OVER
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);
    }
}
