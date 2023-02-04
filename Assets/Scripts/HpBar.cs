using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

public class HpBar : MonoBehaviour
{
    public Image RedBar;

    public void setHP(float value)
    {
        var width = 398;
        RedBar.rectTransform.SetSizeWithCurrentAnchors(Axis.Horizontal, width * value);
    }
}
