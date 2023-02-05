using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public float MapBorder = 32;

    public Text[] killCounters;
    public TMPro.TMP_Text WaveIntroText;
    public Text MoneyText;
    public GameObject GameOverScreen;
    public GameObject ShopScreen;
    public GameObject WaveEndScreen;
    public GameObject GameEndScreen;
    public GameObject HUD;
    public bool Controller = false;
    public WaveData[] WaveData;
    public ShopItem[] ShopItems;
    private float Money = 0;
    private int CurrentWave = 0;
    private int KillCount = 0;

    private List<AttackingEntity> Enemies = new List<AttackingEntity>();

    private string[] KillCounterTexts = {
        /* 000 */ "",
        /* 001 */ "{0} Kill",
        /* 010 */ "{0} Kills",
        /* 011 */ "",
        /* 100 */ "{1} Enemies Left",
        /* 101 */ "{0} Kill, {1} Enemies Left",
        /* 110 */ "{0} Kills, {1} Enemies Left",
        /* 111 */ ""
    };

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

    private void UpdateKillCounters()
    {
        foreach (var killCounter in killCounters)
        {
            int selector = 0;
            if (KillCount == 1)
            {
                selector |= 1;
            }
            if (KillCount > 1)
            {
                selector |= 2;
            }
            if (killCounter.CompareTag("CountsEnemies"))
            {
                selector |= 4;
            }
            killCounter.text = string.Format(KillCounterTexts[selector], KillCount, Enemies.Count);
        }
    }

    #region GameStates
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPlayerDie()
    {
        //GAME_OVER
        Time.timeScale = 0;
        HUD.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    private void OnGameFinish()
    {
        HUD.SetActive(false);
        GameEndScreen.SetActive(true);
    }

    public void RestartGame()
    {
        KillCount = 0;
        Time.timeScale = 1;

        HUD.SetActive(true);
        GameOverScreen.SetActive(false);
        ShopScreen.SetActive(false);
        WaveEndScreen.SetActive(false);
        GameEndScreen.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    #endregion

    #region Shop
    IEnumerator OpenShopAfterDelay()
    {
        yield return new WaitForSeconds(5f);


        WaveEndScreen.SetActive(false);

        UpdateShopItems();

        ShopScreen.SetActive(true);
    }

    public void BuyItem(string item)
    {
        var index = Player.Attacks.FindIndex((attack) => attack.Name == item);
        var playerAttack = Player.Attacks[index];

        if (Money >= playerAttack.Price && !playerAttack.isUnlocked)
        {
            playerAttack.isUnlocked = true;
            Money -= playerAttack.Price;
            MoneyText.text = $"Money: {Money}";

            Player.Attacks[index] = playerAttack;
            Player.ChangeWeapon(true);

            UpdateShopItems();
        }

    }

    private void UpdateShopItems()
    {
        for (int i = 0; i < ShopItems.Length; i++)
        {
            var attack = Player.Attacks[i + 1];
            ShopItems[i].SetPlayerAttack(attack, Money);
        }
    }
    #endregion

    #region  WaveManagement
    public void StartNextWave()
    {
        CurrentWave++;
        KillCount = 0;
        ShopScreen.SetActive(false);
        HUD.SetActive(true);
        if (CurrentWave >= WaveData.Length)
        {
            OnGameFinish();
        }
        else
        {
            StartWave(CurrentWave);
        }
    }

    public void OnEnemyDie(AttackingEntity _enemy)
    {
        KillCount += 1;
        Enemies.Remove(_enemy);
        if (Enemies.Count == 0)
        {
            OnWaveEnd();
        }
        UpdateKillCounters();
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

    public void OnWaveEnd()
    {
        HUD.SetActive(false);
        //Make-Money
        var leftHp = (int)Player.AttackingEntity.HP;
        Money += leftHp;
        MoneyText.text = $"Money: {Money}";

        WaveEndScreen.SetActive(true);

        StartCoroutine(OpenShopAfterDelay());
    }

    IEnumerator FadeIntroText(string text, float fadeIn, float stay, float fadeOut)
    {
        yield return new WaitForSeconds(.3f);

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
            WaveIntroText.color = c;

            yield return new WaitForEndOfFrame();
        }
        UpdateKillCounters();
        yield return new WaitForEndOfFrame();
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
    #endregion
}
