using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text[] killCounts;
    public Text MoneyText;
    public GameObject GameOver;
    public GameObject Shop;
    public bool Controller = false;
    static public GameManager Instance { get; private set; }

    private int KillCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60; //Flo's Laptop sucks...

        if (Instance != null)
            Debug.LogError("unsuported second GameManager!");
        Instance = this;
    }

    public void OnStart()
    {
        KillCount = 0;
        Time.timeScale = 1;

        GameOver.gameObject.SetActive(false);
        Shop.gameObject.SetActive(false);
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
        GameOver.SetActive(true);
    }

    public void OnLevelFinish() {
        //BUY ITEM
        Time.timeScale = 0;
        //MoneyText.text = $"Money: {Money}";
        Shop.SetActive(true);
    }
}
