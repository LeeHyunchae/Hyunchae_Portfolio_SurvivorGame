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

    private ItemManager itemManager;
    private CharacterManager characterManager;

    private void Awake()
    {
        firstStatusInfoButton.onClick.AddListener(OnClickFirstStatusInfoButton);
        secondStatusInfoButton.onClick.AddListener(OnClickSecondStatusInfoButton);
        weaponInventoryButton.onClick.AddListener(OnClickWeaponInventoryButton);
        itemInventoryButton.onClick.AddListener(OnClickItemInventoryButton);

        itemManager = ItemManager.getInstance;
        characterManager = CharacterManager.getInstance;

        itemInfoElement.SetActiveCloseButton(true);

        itemManager.OnRefreshEquipWeaponList += SetWeaponData;
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

    private void OnClickWeaponItemButton(int _itemUid)
    {

        //itemInfoElement.SetTumbnail(itemManager.GetSpriteToName(_itemUid));
        //itemInfoElement.SetName(itemManager.GetWeaponItemModel(_itemUid).itemName);
        //itemInfoElement.SetCharacterInfo(itemManager.GetWeaponItemModel(_itemUid).status);

        WeaponItemModel model = itemManager.GetWeaponItemModel(_itemUid);

        itemInfoElement.SetName(model.itemName);
        itemInfoElement.SetTumbnail(itemManager.GetSpriteToName(model.itemThumbnail));
        itemInfoElement.SetActive(true);

        string weaponInfo = "Damage : " + model.status.damage + " \n" + "attack speed : " + model.status.cooldown;

        itemInfoElement.SetInfoText(weaponInfo);
    }

    private void SetWeaponData()
    {
        int weaponCapacity = 6;

        for (int i = 0; i < weaponCapacity; i++)
        {
            WeaponItemModel weaponModel = itemManager.GetEquipWeaponItemModel(i);

            ItemButtonElement itemButtonElement = itemButtonElements[i];

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
                itemButtonElement.GetButtonClickedEvent.AddListener(() => OnClickWeaponItemButton(weaponModel.itemUid));
            }

        }

    }
}
