using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfoController : MonoBehaviour
{
    [SerializeField] private Button firstStatusInfoButton;
    [SerializeField] private Button secondStatusInfoButton;
    [SerializeField] private Button weaponInventoryButton;
    [SerializeField] private Button itemInventoryButton;
    [SerializeField] private GameObject weaponItemListObject;
    [SerializeField] private ItemButtonElement[] weaponItemButtonElements;
    [SerializeField] private ScrollRect passiveItemListScrollRect;
    [SerializeField] private ItemButtonElement originPassiveItemButtonElement;
    [SerializeField] private ItemStatusInfoElement itemInfoElement;
    [SerializeField] private Image dimImage;
    [SerializeField] private ScrollRect statusInfoObject;
    [SerializeField] private StatusElement originStatusElements;

    private GlobalData globalData;
    private ItemManager itemManager;
    private CharacterManager characterManager;
    private int curInfoItemSlotNum = -1;
    private List<ItemButtonElement> passiveItemButtonList = new List<ItemButtonElement>();
    private List<StatusElement> statusElementList = new List<StatusElement>();

    private void Awake()
    {
        firstStatusInfoButton.onClick.AddListener(OnClickFirstStatusInfoButton);
        secondStatusInfoButton.onClick.AddListener(OnClickSecondStatusInfoButton);
        weaponInventoryButton.onClick.AddListener(OnClickWeaponInventoryButton);
        itemInventoryButton.onClick.AddListener(OnClickItemInventoryButton);

        itemManager = ItemManager.getInstance;
        globalData = GlobalData.getInstance;
        characterManager = CharacterManager.getInstance;

        itemInfoElement.SetActiveCloseButton(true);
        itemInfoElement.SetActiveSellButton(true);
        
        itemManager.OnRefreshEquipWeaponList += SetWeaponData;
        itemManager.OnRefreshEquipPassiveList += AddPassiveItemData;

        itemInfoElement.GetCloseButtonEvent.AddListener(OnClickWeaponItemInfoCloseButton);
        itemInfoElement.GetSellButtonEvent.AddListener(OnClickWeaponItemSellButton);
        itemInfoElement.GetCombineButton.AddListener(OnClickWeaponItemCombineButton);

        InitPassiveItemButtonElement();

        InitStatusInfoElements();
    }

    private void InitPassiveItemButtonElement()
    {
        Transform contentTransform = passiveItemListScrollRect.content.transform;

        int count = 100;

        for(int i = 0; i< count; i++)
        {
            ItemButtonElement itemButtonElement = Instantiate<ItemButtonElement>(originPassiveItemButtonElement,contentTransform);

            passiveItemButtonList.Add(itemButtonElement);
        }
    }

    private void InitStatusInfoElements()
    {
        Transform parent = statusInfoObject.content.transform;
        int count = (int)ECharacterStatus.END;

        Character character = characterManager.GetPlayerCharacter;

        character.onRefreshStatusAction += OnRefreshPlayerStatus;

        for (int i = 0;  i < count; i++)
        {
            StatusElement statusElement = Instantiate<StatusElement>(originStatusElements, parent);

            statusElementList.Add(statusElement);

            BaseCharacterStatus characterStatus = character.GetPlayerStatus((ECharacterStatus)i);

            string statusName = ((ECharacterStatus)i).ToString();
            statusName = statusName.Substring("PLAYER_".Length);

            statusElement.SetStatusName(statusName);
            statusElement.SetStatusValue(characterStatus.multiplierApplyStatus.ToString());
            statusElement.gameObject.SetActive(true);
        }
    }

    private void ExtensionPassiveItemButtonList()
    {
        Transform contentTransform = passiveItemListScrollRect.content.transform;

        int curCount = passiveItemButtonList.Count;
        int extensionCount = curCount * 2;

        for (int i = curCount; i < extensionCount; i++)
        {
            ItemButtonElement itemButtonElement = Instantiate<ItemButtonElement>(originPassiveItemButtonElement, contentTransform);

            passiveItemButtonList.Add(itemButtonElement);
        }
    }


    private void OnClickFirstStatusInfoButton()
    {
        statusInfoObject.gameObject.SetActive(true);
        weaponItemListObject.gameObject.SetActive(false);
        passiveItemListScrollRect.gameObject.SetActive(false);
    }
    private void OnClickSecondStatusInfoButton()
    {
        statusInfoObject.gameObject.SetActive(true);
        weaponItemListObject.gameObject.SetActive(false);
    }
    private void OnClickWeaponInventoryButton()
    {
        statusInfoObject.gameObject.SetActive(false);
        weaponItemListObject.gameObject.SetActive(true);
        passiveItemListScrollRect.gameObject.SetActive(false);

        SetWeaponData();
    }
    private void OnClickItemInventoryButton()
    {
        statusInfoObject.gameObject.SetActive(false);
        weaponItemListObject.gameObject.SetActive(false);
        passiveItemListScrollRect.gameObject.SetActive(true);
    }

    private void OnClickWeaponItemButton(int _itemUid, int _itemSlotNum)
    {
        curInfoItemSlotNum = _itemSlotNum;

        //itemInfoElement.SetTumbnail(itemManager.GetSpriteToName(_itemUid));
        //itemInfoElement.SetName(itemManager.GetWeaponItemModel(_itemUid).itemName);
        //itemInfoElement.SetCharacterInfo(itemManager.GetWeaponItemModel(_itemUid).status);

        dimImage.enabled = true;

        WeaponItemModel model = itemManager.GetItemModel(_itemUid) as WeaponItemModel;

        itemInfoElement.SetName(model.itemName);
        itemInfoElement.SetTumbnail(itemManager.GetItemSprite(model.itemUid));
        itemInfoElement.SetActive(true);

        string weaponInfo = "Damage : " + model.status.damage + " \n" + "attack speed : " + model.status.cooldown;

        itemInfoElement.SetInfoText(weaponInfo);
        itemInfoElement.SetItemPrice(model.itemPrice.ToString());

        itemInfoElement.SetActiveCombineButton(itemManager.CheckCombineItemExistence(model));
    }

    private void OnClickWeaponItemInfoCloseButton()
    {
        dimImage.enabled = false;
        curInfoItemSlotNum = -1;
    }

    private void OnClickWeaponItemSellButton()
    {
        if(curInfoItemSlotNum == -1)
        {
            return;
        }

        BaseItemModel itemModel = itemManager.GetEquipWeaponItemModel(curInfoItemSlotNum);

        globalData.IncreasePieceCount((int)itemModel.itemPrice);

        Debug.Log("IncreasePieceCount");

        itemManager.SellWeaponItem(curInfoItemSlotNum);
        dimImage.enabled = false;
        itemInfoElement.Hide();
    }

    private void OnClickWeaponItemCombineButton()
    {
        if (curInfoItemSlotNum == -1)
        {
            return;
        }

        itemManager.CombineWeaponItem(curInfoItemSlotNum);
        dimImage.enabled = false;
        itemInfoElement.Hide();
    }

    private void SetWeaponData()
    {
        int weaponCapacity = 6;

        for (int i = 0; i < weaponCapacity; i++)
        {
            BaseItemModel weaponModel = itemManager.GetEquipWeaponItemModel(i);

            ItemButtonElement itemButtonElement = weaponItemButtonElements[i];

            int slotIdx = i;

            if (weaponModel == null)
            {
                itemButtonElement.SetThumbnail(null);
                itemButtonElement.SetItemUID(-1);
                itemButtonElement.GetButtonClickedEvent.RemoveAllListeners();
            }
            else
            {
                itemButtonElement.SetThumbnail(itemManager.GetItemSprite(weaponModel.itemUid));
                itemButtonElement.SetItemUID(weaponModel.itemUid);
                itemButtonElement.GetButtonClickedEvent.RemoveAllListeners();
                itemButtonElement.GetButtonClickedEvent.AddListener(() => OnClickWeaponItemButton(weaponModel.itemUid,slotIdx));
            }

        }

    }

    private void AddPassiveItemData()
    {
        List<BaseItemModel> itemList = itemManager.GetAllEquipPassiveItemModelList;

        int slotIndex = itemList.Count - 1;

        if(slotIndex >= passiveItemButtonList.Count)
        {
            ExtensionPassiveItemButtonList();
        }

        BaseItemModel itemModel = itemList[slotIndex];

        ItemButtonElement buttonElement = passiveItemButtonList[slotIndex];

        buttonElement.SetThumbnail(itemManager.GetItemSprite(itemModel.itemUid));
        buttonElement.SetItemUID(itemModel.itemUid);
        buttonElement.GetButtonClickedEvent.AddListener(() => OnClickPassiveItemButton(itemModel.itemUid));
        buttonElement.SetActive(true);
    }

    private void OnClickPassiveItemButton(int _itemUid)
    {
        dimImage.enabled = true;

        PassiveItemModel model = itemManager.GetItemModel(_itemUid) as PassiveItemModel;

        itemInfoElement.SetName(model.itemName);
        itemInfoElement.SetTumbnail(itemManager.GetItemSprite(model.itemUid));
        itemInfoElement.SetActive(true);
        itemInfoElement.SetInfoText(model.itemInfo);
        itemInfoElement.SetItemPrice(model.itemPrice.ToString());

        itemInfoElement.SetActiveCombineButton(false);
        itemInfoElement.SetActiveSellButton(false);
    }

    private void OnRefreshPlayerStatus(int _statusNum)
    {
        Character character = characterManager.GetPlayerCharacter;

        BaseCharacterStatus characterStatus = character.GetPlayerStatus((ECharacterStatus)_statusNum);

        statusElementList[_statusNum].SetStatusValue(characterStatus.multiplierApplyStatus.ToString());
    }
}
