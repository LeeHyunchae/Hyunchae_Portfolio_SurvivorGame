using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemElement : MonoBehaviour
{
    [SerializeField] private Image itemThumbnail;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private Button LockButton;
    [SerializeField] private Button BuyButton;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    public Button.ButtonClickedEvent GetLockButtomClickListner => LockButton.onClick;
    public Button.ButtonClickedEvent GetBuyButtonClickListner => BuyButton.onClick;

    public void SetThumbnail(Sprite _image)
    {
        itemThumbnail.sprite = _image;
    }

    public void SetItemData(BaseItemModel _itemModel)
    {
        itemName.text = _itemModel.itemName;
        itemPriceText.text = _itemModel.itemPrice.ToString();
    }

    public void SetItemInfo(string _info)
    {
        itemInfo.text = _info;
    }
}
