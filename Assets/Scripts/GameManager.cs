using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public float MapBorder = 32;

    public Text[] killCounts;
    public TMPro.TMP_Text WaveIntroText;
    public Text MoneyText;
    public GameObject GameOver;
    public GameObject Shop;
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

        GameOver.SetActive(false);
        Shop.SetActive(false);
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
        GameOver.SetActive(true);
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
        MoneyText.text = $"Money: {Money}";

        //Open-Shop
        Shop.SetActive(true);
    }

    public void StartNextWave()
    {
        CurrentWave++;
        Shop.SetActive(false);
        if (CurrentWave >= WaveData.Length)
        {
            //WIN!!!
            Debug.LogWarning("Du hast gewonnen!");
            OnPlayerDie();
        }
        else
        {
            StartWave(CurrentWave);
        }
    }

    private void StartWave(int index)
    {
        Player.transform.position = Vector3.zero;
        Player.AttackingEntity.HP = Player.AttackingEntity.maxHP;

        StartCoroutine(FadeIntroText(WaveData[index].IntroText, 2, 3, 1.5f));

        foreach (var enemyData in WaveData[index].Enemies)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                Enemies.Add(SpawnOnBorder(enemyData.Prefab));
            }
        }
    }

    IEnumerator FadeIntroText(string text, float fadeIn, float stay, float fadeOut)
    {
        yield return new WaitForSeconds(1);

        WaveIntroText.text = text;

        float fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime / fadeIn;
            Color c = WaveIntroText.color;
            c.a = fade;
            WaveIntroText.color = c;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(stay);

        fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime / fadeOut;
            Color c = WaveIntroText.color;
            c.a = 1f - fade;
            Debug.Log(c.a);
            WaveIntroText.color = c;

            yield return new WaitForEndOfFrame();
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
