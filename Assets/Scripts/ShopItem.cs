using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text Price;
    public Image Icon;
    public Button BuyButton;

    public void SetPlayerAttack(PlayerAttack playerAttack, float money)
    {
        NameText.text = playerAttack.Name;
        Price.text = $"Price: {playerAttack.Price}";
        Icon.sprite = playerAttack.Icon;
        BuyButton.onClick.AddListener(delegate { GameManager.Instance.BuyItem(playerAttack.Name); });
        if (money >= playerAttack.Price && !playerAttack.isUnlocked)
        {
            BuyButton.interactable = true;
        }
        else if (money >= playerAttack.Price && playerAttack.isUnlocked) {
            BuyButton.interactable = false;
            BuyButton.GetComponentInChildren<TMP_Text>().text = "Owned";
        }
        else {
            BuyButton.interactable = false;
        }
    }
}