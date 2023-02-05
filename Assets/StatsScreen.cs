using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{
    public Text killsText;
    public Text cashText;
    public Text timeText;

    public void SetStats(int cash, int kills)
    {
        killsText.text = $"HECKEN SLASHED: {kills}";
        cashText.text = $"CASH EARNED: {cash}";
        timeText.text = $"TIME PLAYED: {(int)(Time.time / 60)}m {(int)(Time.time % 60)}s";
    }
}
