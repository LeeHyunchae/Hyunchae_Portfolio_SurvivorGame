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
    [SerializeField] private GameObject statusInfoObject;
    [SerializeField] private StatusElement[] statusElements;
    [SerializeField] private GameObject itemListObject;
    [SerializeField] private ItemButtonElement[] itemButtonElements;
    [SerializeField] private ItemStatusInfoElement itemInfoElement;
    [SerializeField] private Image dimImage;

    private ItemManager itemManager;
    private CharacterManager characterManager;
    private int curInfoItemSlotNum = -1;

    private void Awake()
    {
        firstStatusInfoButton.onClick.AddListener(OnClickFirstStatusInfoButton);
        secondStatusInfoButton.onClick.AddListener(OnClickSecondStatusInfoButton);
        weaponInventoryButton.onClick.AddListener(OnClickWeaponInventoryButton);
        itemInventoryButton.onClick.AddListener(OnClickItemInventoryButton);

        itemManager = ItemManager.getInstance;
        characterManager = CharacterManager.getInstance;

        itemInfoElement.SetActiveCloseButton(true);
        itemInfoElement.SetActiveSellButton(true);
        
        itemManager.OnRefreshEquipWeaponList += SetWeaponData;

        itemInfoElement.GetCloseButtonEvent.AddListener(OnClickWeaponItemInfoCloseButton);
        itemInfoElement.GetSellButtonEvent.AddListener(OnClickWeaponItemSellButton);
        itemInfoElement.GetCombineButton.AddListener(OnClickWeaponItemCombineButton);
    }

    private void OnClickFirstStatusInfoButton()
    {
        statusInfoObject.gameObject.SetActive(true);
        itemListObject.gameObject.SetActive(false);
    }
    private void OnClickSecondStatusInfoButton()
    {
        statusInfoObject.gameObject.SetActive(true);
        itemListObject.gameObject.SetActive(false);
    }
    private void OnClickWeaponInventoryButton()
    {
        statusInfoObject.gameObject.SetActive(false);
        itemListObject.gameObject.SetActive(true);

        SetWeaponData();
    }
    private void OnClickItemInventoryButton()
    {
        statusInfoObject.gameObject.SetActive(false);
        itemListObject.gameObject.SetActive(true);
    }

    private void OnClickWeaponItemButton(int _itemUid, int _itemSlotNum)
    {
        curInfoItemSlotNum = _itemSlotNum;

        //itemInfoElement.SetTumbnail(itemManager.GetSpriteToName(_itemUid));
        //itemInfoElement.SetName(itemManager.GetWeaponItemModel(_itemUid).itemName);
        //itemInfoElement.SetCharacterInfo(itemManager.GetWeaponItemModel(_itemUid).status);

        dimImage.enabled = true;

        WeaponItemModel model = itemManager.GetWeaponItemModel(_itemUid);

        itemInfoElement.SetName(model.itemName);
        itemInfoElement.SetTumbnail(itemManager.GetSpriteToName(model.itemThumbnail));
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
            WeaponItemModel weaponModel = itemManager.GetEquipWeaponItemModel(i);

            ItemButtonElement itemButtonElement = itemButtonElements[i];

            int slotIdx = i;

            if (weaponModel == null)
            {
                itemButtonElement.SetThumbnail(null);
                itemButtonElement.SetItemUID(-1);
                itemButtonElement.GetButtonClickedEvent.RemoveAllListeners();
            }
            else
            {
                itemButtonElement.SetThumbnail(itemManager.GetSpriteToName(weaponModel.itemThumbnail));
                itemButtonElement.SetItemUID(weaponModel.itemUid);
                itemButtonElement.GetButtonClickedEvent.RemoveAllListeners();
                itemButtonElement.GetButtonClickedEvent.AddListener(() => OnClickWeaponItemButton(weaponModel.itemUid,slotIdx));
            }

        }

    }
}
