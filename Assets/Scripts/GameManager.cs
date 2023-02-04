using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public float MapBorder = 32;

    public Text[] killCounts;
    public RectTransform gameOverScreen;
    public RectTransform shopScreen;
    public bool Controller = false;
    public WaveData[] WaveData;

    private int Money = 0;
    private int CurrentWave = 0;
    private int KillCount = 0;
    private List<AttackingEntity> Enemies = new List<AttackingEntity>();

    // Start is called before the first frame update
    static public GameManager Instance { get; private set; }
    void Awake()
    {
        Application.targetFrameRate = 60; //Flo's Laptop sucks...

        if (Instance != null)
            Debug.LogError("unsuported second GameManager!");

        Instance = this;
        Player = FindObjectOfType<Player>();
    }

    public void Start()
    {
        StartWave(CurrentWave);
    }

    public void RestartGame()
    {
        KillCount = 0;
        Time.timeScale = 1;

        gameOverScreen.gameObject.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPlayerDie()
    {
        //GAME_OVER
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);
    }

    public void OnEnemyDie(AttackingEntity _enemy)
    {
        KillCount += 1;
        foreach (var killCount in killCounts)
        {
            killCount.text = $"Kills {KillCount}";
        }

        Enemies.Remove(_enemy);
        if (Enemies.Count == 0)
        {
            OnWaveEnd();
        }
    }

    private void OnWaveEnd()
    {
        //Make-Money
        var leftHp = (int)Player.AttackingEntity.HP;
        Money += leftHp;

        //Open-Shop
        shopScreen.gameObject.SetActive(true);
    }

    public void StartNextWave()
    {
        CurrentWave++;
        StartWave(CurrentWave);
    }

    private void StartWave(int index)
    {
        Player.transform.position = Vector3.zero;
        Player.AttackingEntity.HP = Player.AttackingEntity.maxHP;

        foreach (var enemyData in WaveData[index].Enemies)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                Enemies.Add(SpawnOnBorder(enemyData.Prefab));
            }
        }
    }

    private Enemy SpawnOnBorder(Enemy _prefab)
    {
        var rngValue = Random.Range(-MapBorder, MapBorder);
        var rngSide = Random.Range(0, 3);
        Vector3 position = Vector3.zero;
        switch (rngSide)
        {
            case 0:
                position = new Vector3(rngValue, 0, MapBorder);
                break;
            case 1:
                position = new Vector3(rngValue, 0, -MapBorder);
                break;
            case 2:
                position = new Vector3(MapBorder, 0, rngValue);
                break;
            case 3:
                position = new Vector3(-MapBorder, 0, rngValue);
                break;
        }

        var enemy = Instantiate(_prefab, position, Quaternion.identity);
        enemy.transform.LookAt(Player.transform.position);
        return enemy;
    }
}
