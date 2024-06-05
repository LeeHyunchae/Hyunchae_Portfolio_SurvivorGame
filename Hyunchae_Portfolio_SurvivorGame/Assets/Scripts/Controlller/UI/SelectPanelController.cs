using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelController : UIBaseController
{
    private const int BUTTON_CAPACITY = 20;

    [SerializeField] private GameObject originSelectButtonItem;
    [SerializeField] private Transform selectButtonListBG;
    [SerializeField] private CharacterStatusInfo statusInfo;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private UIManager uiManager;

    private List<CharacterModel> characters = new List<CharacterModel>();
    private List<WeaponItemModel> weapons = new List<WeaponItemModel>();

    private SelectButtonElement[] selectButtons = new SelectButtonElement[BUTTON_CAPACITY];

    private CharacterModel selectCharacter = null;
    private WeaponItemModel selectWeapon = null;

    private CharacterManager characterManager;
    private ItemManager itemManager;

    private void Start()
    {
        characterManager = CharacterManager.getInstance;
        itemManager = ItemManager.getInstance;

        nextButton.onClick.AddListener(OnClickNextButton);
        prevButton.onClick.AddListener(OnClickPrevButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        InitData();

        CreateSelectButton();
    }

    private void InitData()
    {
        characters = characterManager.GetAllCharacterModel;
        weapons = ItemManager.getInstance.GetAllWeaponModel();
    }

    private void CreateSelectButton()
    {
        for (int i = 0; i < BUTTON_CAPACITY; i++)
        {
            SelectButtonElement element = Instantiate<GameObject>(originSelectButtonItem,selectButtonListBG).GetComponent<SelectButtonElement>();

            element.Init();

            element.SetActive(false);

            selectButtons[i] = element;
        }

        SetCharacterButton();
    }

    private void SetCharacterButton()
    {
        int count = characters.Count;

        for(int i = 0; i < count; i++)
        {
            int index = i;

            SelectButtonElement element = selectButtons[i];

            element.SetThumbnail(characterManager.GetCharacterSprite(characters[i].characterUid));
            element.GetButtonClickedEvent.RemoveAllListeners();
            element.GetButtonClickedEvent.AddListener(() => OnClickCharacterButton(index));

            element.SetActive(true);
        }
    }

    private void SetWeaponButton()
    {
        int count = weapons.Count;

        for (int i = 0; i < count; i++)
        {
            int index = i;

            SelectButtonElement element = selectButtons[i];

            element.SetThumbnail(itemManager.GetWeaponItemSprite(weapons[i].itemUid));
            element.GetButtonClickedEvent.RemoveAllListeners();
            element.GetButtonClickedEvent.AddListener(() => OnClickWeaponButton(index));

            element.SetActive(true);
        }
    }


    private void OnClickCharacterButton(int _index)
    {
        CharacterModel model = characters[_index];

        selectCharacter = model;

        statusInfo.SetName(model.characterName);
        statusInfo.SetCharacterTumbnail(characterManager.GetCharacterSprite(model.characterUid));
        statusInfo.SetActive(true);

        StringBuilder stringBuilder = new StringBuilder();

        int count = model.variances.Count;

        for(int i = 0; i <count; i++)
        {
            string varianceInfo = model.variances[i].characterStatus + " : " + model.variances[i].variance + "\n";
            stringBuilder.Append(varianceInfo);
        }

        statusInfo.SetCharacterInfo(stringBuilder.ToString());

    }

    private void OnClickWeaponButton(int _index)
    {
        WeaponItemModel model = weapons[_index];

        selectWeapon = model;

        statusInfo.SetName(model.itemName);
        statusInfo.SetCharacterTumbnail(itemManager.GetWeaponItemSprite(model.itemUid));
        statusInfo.SetActive(true);

        string weaponInfo = "Damage : " + model.status.damage + " \n" + "attack speed : " + model.status.cooldown;

        statusInfo.SetCharacterInfo(weaponInfo);
    }

    private void OnClickNextButton()
    {
        if(selectCharacter != null && selectWeapon != null)
        {
            //Game Start
            itemManager.AddEquipWeaponItem(selectWeapon.itemUid);
            characterManager.SelectCharacterModel(selectCharacter.characterUid);
            SceneChanger.getInstance.ChangeScene("IngameScene");
        }

        if(selectCharacter == null)
        {
            return;
        }

        statusInfo.SetActive(false);

        SetWeaponButton();

        prevButton.gameObject.SetActive(true);
    }

    private void OnClickPrevButton()
    {
        selectWeapon = null;

        prevButton.gameObject.SetActive(false);

        statusInfo.SetActive(false);

        SetCharacterButton();
    }

    private void OnClickCloseButton()
    {
        selectCharacter = null;
        selectWeapon = null;
        uiManager.Hide();
    }
}
