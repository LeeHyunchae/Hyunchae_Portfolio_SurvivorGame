using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : UIBaseController
{
    private const int SHOWING_ITEM_COUNT = 4;
    private const int FIRST_REROLL_PRICE = 3;
    private const int ADDED_REROLL_PRICE = 2;

    private UIManager uiManager;
    private GlobalData globalData;

    [SerializeField] TextMeshProUGUI pieceCountText;
    [SerializeField] Button nextWaveButton;
    [SerializeField] Button itemListRerollButton;
    [SerializeField] TextMeshProUGUI itemListRerollPriceText;
    [SerializeField] ShopItemElement[] shopItemElements;
    [SerializeField] StatusInfoController playerStatusInfo;

    public Action OnClickNextWaveAction;
    private ItemManager itemManager;

    private List<BaseItemModel> randomItems = new List<BaseItemModel>();

    private BaseItemModel[] curShowingItemArr = new BaseItemModel[SHOWING_ITEM_COUNT];
    private int curRerollPrice;

    protected override void Init()
    {
        base.Init();

        uiManager = UIManager.getInstance;
        globalData = GlobalData.getInstance;
        itemManager = ItemManager.getInstance;
        nextWaveButton.onClick.AddListener(OnClickNextWave);

        SetPieceCountText(globalData.GetPieceCount);
        globalData.OnRefreshPieceAction += SetPieceCountText;

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

            element.GetBuyButtonClickListner.AddListener(() => OnClickBuyButton(index));
        }
    }

    public override void Show()
    {
        base.Show();

        SetRandomItemList();

        curRerollPrice = FIRST_REROLL_PRICE;
        itemListRerollPriceText.text = curRerollPrice.ToString();
    }

    private void SetRandomItemList()
    {
        randomItems = itemManager.GetRandomItemList();

        int count = randomItems.Count;

        for (int i = 0; i < count; i++)
        {

            ShopItemElement element = shopItemElements[i];

            element.SetThumbnail(itemManager.GetItemSprite(randomItems[i].itemUid));
            element.SetItemData(randomItems[i]);
            element.SetActive(true);

            if(randomItems[i].itemType == EItemType.WEAPON)
            {
                StringBuilder stringBuilder = (randomItems[i] as WeaponItemModel).GetWeaponInfo();
                element.SetItemInfo(stringBuilder.ToString());
            }
            else
            {
                element.SetItemInfo((randomItems[i] as PassiveItemModel).itemInfo);
            }

            curShowingItemArr[i] = randomItems[i];
        }
    }

    private void RemoveBuyItemToItemList()
    {
        int count = curShowingItemArr.Length;

        for (int i = 0; i < count; i++)
        {
            if(curShowingItemArr[i] == null)
            {
                shopItemElements[i].SetActive(false);
            }
        }
    }

    public void SetPieceCountText(int _pieceCount)
    {
        pieceCountText.text = _pieceCount.ToString();
    }

    private void OnClickNextWave()
    {
        uiManager.Hide();

        OnClickNextWaveAction?.Invoke();
    }

    private void OnClickRerollButton()
    {
        globalData.DecreasePieceCount(curRerollPrice);

        curRerollPrice += ADDED_REROLL_PRICE;
        itemListRerollPriceText.text = curRerollPrice.ToString();

        SetRandomItemList();
    }

    private void OnClickBuyButton(int _elementIndex)
    {
        BaseItemModel itemModel = curShowingItemArr[_elementIndex];

        if(globalData.GetPieceCount < itemModel.itemPrice)
        {
            Debug.Log("Not enough Piece");
            return;
        }

        bool isBuySuccess = itemManager.OnBuyItem(itemModel.itemUid);

        if (isBuySuccess)
        {
            curShowingItemArr[_elementIndex] = null;
            RemoveBuyItemToItemList();
            globalData.DecreasePieceCount((int)itemModel.itemPrice);
            Debug.Log("DecreasePieceCount");
        }
        else
        {
            Debug.Log("Item Buy Failed");
        }


    }

}
