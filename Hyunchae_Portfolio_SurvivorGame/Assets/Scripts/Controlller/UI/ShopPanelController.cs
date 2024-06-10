using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : UIBaseController
{
    private const int SHOWING_ITEM_COUNT = 4;
    private UIManager uiManager;

    [SerializeField] TextMeshProUGUI peiceCountText;
    [SerializeField] Button nextWaveButton;
    [SerializeField] Button itemListRerollButton;
    [SerializeField] TextMeshProUGUI itemListRerollPriceText;
    [SerializeField] ShopItemElement[] shopItemElements;
    [SerializeField] StatusInfoController playerStatusInfo;

    public Action OnClickNextWaveAction;
    private ItemManager itemManager;

    private List<BaseItemModel> items = new List<BaseItemModel>();

    private BaseItemModel[] curShowingItemArr = new BaseItemModel[SHOWING_ITEM_COUNT];

    protected override void Init()
    {
        base.Init();

        uiManager = UIManager.getInstance;
        nextWaveButton.onClick.AddListener(OnClickNextWave);
        itemManager = ItemManager.getInstance;

        InitButtons();
    }

    private void InitButtons()
    {
        itemListRerollButton.onClick.AddListener(OnClickRerollButton);

        int count = shopItemElements.Length;

        for(int i = 0; i <count; i++)
        {
            int index = i;

            ShopItemElement element = shopItemElements[i];

            element.GetLockButtomClickListner.AddListener(() => OnClickLockButton(index));
            element.GetBuyButtonClickListner.AddListener(() => OnClickBuyButton(index));
        }
    }

    public override void Show()
    {
        base.Show();

        SetItemList();
    }

    private void SetItemList()
    {
        items = itemManager.GetRandomItemList();

        int count = items.Count;

        for (int i = 0; i < count; i++)
        {

            ShopItemElement element = shopItemElements[i];

            //Todo RandomSystem
            element.SetThumbnail(itemManager.GetSpriteToName(items[i].itemThumbnail));
            element.SetItemData(items[i]);

            curShowingItemArr[i] = items[i];
        }
    }

    private void OnClickNextWave()
    {
        uiManager.Hide();

        OnClickNextWaveAction?.Invoke();
    }

    private void OnClickRerollButton()
    {
        SetItemList();
    }

    private void OnClickBuyButton(int _elementIndex)
    {
        itemManager.AddEquipWeaponItem(curShowingItemArr[_elementIndex].itemUid);
    }

    private void OnClickLockButton(int _elementIndex)
    {
        Debug.Log("LockButtonClick idx : " + _elementIndex + " itemModelName : " + curShowingItemArr[_elementIndex].itemName);
    }
}
