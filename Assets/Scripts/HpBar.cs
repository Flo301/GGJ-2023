using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

public class HpBar : MonoBehaviour
{
    public Slider RedBar;

    public void setHP(float value)
    {
        Debug.Log(value);
        RedBar.value = value;
    }
}
