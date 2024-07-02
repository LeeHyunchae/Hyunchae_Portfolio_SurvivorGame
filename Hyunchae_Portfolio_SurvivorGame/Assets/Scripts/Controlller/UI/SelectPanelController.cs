using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum ESELECTPHASE
{
    CHARACTER = 0,
    WEAPON,
    STAGE,
    END
}

public class SelectPanelController : UIBaseController
{
    private const int BUTTON_CAPACITY = 20;
    private const int STAGECOUNT = 5;

    [SerializeField] private GameObject originSelectButtonItem;
    [SerializeField] private Transform selectButtonListBG;
    [SerializeField] private ItemStatusInfoElement statusInfo;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private UIManager uiManager;

    private List<CharacterModel> characters = new List<CharacterModel>();
    private List<WeaponItemModel> weapons = new List<WeaponItemModel>();

    private SelectButtonElement[] selectButtons = new SelectButtonElement[BUTTON_CAPACITY];

    private CharacterModel selectCharacter = null;
    private WeaponItemModel selectWeapon = null;
    private int selectStage = -1;

    private CharacterManager characterManager;
    private ItemManager itemManager;
    private StageManager stageManager;

    private ESELECTPHASE selectPhase;

    private void Start()
    {
        characterManager = CharacterManager.getInstance;
        itemManager = ItemManager.getInstance;
        uiManager = UIManager.getInstance;
        stageManager = StageManager.getInstance;

        nextButton.onClick.AddListener(OnClickNextButton);
        prevButton.onClick.AddListener(OnClickPrevButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        InitData();

        CreateSelectButton();

        statusInfo.SetActiveCloseButton(false);
        statusInfo.SetActiveSellButton(false);
        statusInfo.SetActiveCombineButton(false);

        selectPhase = ESELECTPHASE.CHARACTER;
    }

    private void InitData()
    {
        characters = characterManager.GetAllCharacterModel;
        InitSelectableWeaponModels();
    }

    private void InitSelectableWeaponModels()
    {
        List<WeaponItemModel> weaponModels = ItemManager.getInstance.GetAllWeaponModel();

        int count = weaponModels.Count;

        for(int i = 0; i < count; i ++)
        {
            if(weaponModels[i].weaponTier == 0)
            {
                weapons.Add(weaponModels[i]);
            }
        }
    }

    private void ClearButtons()
    {
        for (int i = 0; i < BUTTON_CAPACITY; i++)
        {
            SelectButtonElement element = selectButtons[i];

            element.Init();

            element.SetActive(false);
        }
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
        ClearButtons();

        int count = characters.Count;

        for(int i = 0; i < count; i++)
        {
            int index = i;

            SelectButtonElement element = selectButtons[i];

            element.SetThumbnail(characterManager.GetCharacterSprite(characters[i].characterUid));
            element.SetThumbnailText(string.Empty);
            element.GetButtonClickedEvent.RemoveAllListeners();
            element.GetButtonClickedEvent.AddListener(() => OnClickCharacterButton(index));

            element.SetActive(true);
        }
    }

    private void SetWeaponButton()
    {
        ClearButtons();
        
        int count = weapons.Count;

        for (int i = 0; i < count; i++)
        {
            int index = i;

            SelectButtonElement element = selectButtons[i];

            element.SetThumbnail(itemManager.GetItemSprite(weapons[i].itemUid));
            element.SetThumbnailText(string.Empty);
            element.GetButtonClickedEvent.RemoveAllListeners();
            element.GetButtonClickedEvent.AddListener(() => OnClickWeaponButton(index));

            element.SetActive(true);
        }
    }

    private void SetStageButton()
    {
        ClearButtons();

        Debug.Log("SetStageButton");

        for (int i = 0; i <= STAGECOUNT; i++)
        {
            int index = i;

            SelectButtonElement element = selectButtons[i];

            element.SetThumbnail(null);
            element.SetThumbnailText((index+1).ToString());
            element.GetButtonClickedEvent.RemoveAllListeners();
            element.GetButtonClickedEvent.AddListener(() => OnClickStageButton(index));

            element.SetActive(true);
        }
    }


    private void OnClickCharacterButton(int _index)
    {
        CharacterModel model = characters[_index];

        selectCharacter = model;

        statusInfo.SetName(model.characterName);
        statusInfo.SetTumbnail(characterManager.GetCharacterSprite(model.characterUid));
        statusInfo.SetActive(true);

        StringBuilder stringBuilder = new StringBuilder();

        int count = model.variances.Count;

        for(int i = 0; i <count; i++)
        {
            string varianceInfo = model.variances[i].characterStatus + " : " + model.variances[i].variance + "\n";
            stringBuilder.Append(varianceInfo);
        }

        statusInfo.SetInfoText(stringBuilder.ToString());

    }

    private void OnClickWeaponButton(int _index)
    {
        WeaponItemModel model = weapons[_index];

        selectWeapon = model;

        statusInfo.SetName(model.itemName);
        statusInfo.SetTumbnail(itemManager.GetItemSprite(model.itemUid));
        statusInfo.SetActive(true);

        StringBuilder sb = selectWeapon.GetWeaponInfo();

        statusInfo.SetInfoText(sb.ToString());
    }

    private void OnClickStageButton(int _index)
    {
        selectStage = _index;

        statusInfo.SetName(string.Empty);
        statusInfo.SetTumbnail(null);
        statusInfo.SetActive(true);
        statusInfo.SetInfoText("Stage : " + (_index + 1));
    }

    private void OnClickNextButton()
    {
        int selectPhaseIndex = (int)selectPhase;
        selectPhaseIndex++;
        selectPhase = (ESELECTPHASE)selectPhaseIndex;

        Debug.Log(selectPhase);

        if (selectPhase == ESELECTPHASE.END)
        {
            //Game Start

            itemManager.OnBuyItem(selectWeapon.itemUid);
            characterManager.SelectCharacterModel(selectCharacter.characterUid);
            stageManager.SetCurStage(selectStage + 1);
            SceneChanger.getInstance.ChangeScene("IngameScene");
        }

        RefreshSelectPage(selectPhase);

        statusInfo.SetActive(false);

        prevButton.gameObject.SetActive(true);
    }

    private void OnClickPrevButton()
    {
        int selectPhaseIndex = (int)selectPhase;
        selectPhaseIndex--;
        selectPhase = (ESELECTPHASE)selectPhaseIndex;

        if (selectPhase == ESELECTPHASE.CHARACTER)
        {
            prevButton.gameObject.SetActive(false);
        }
        
        statusInfo.SetActive(false);

        RefreshSelectPage(selectPhase);
    }

    private void OnClickCloseButton()
    {
        selectCharacter = null;
        selectWeapon = null;
        selectStage = -1;
        uiManager.Hide();
    }

    private void RefreshSelectPage(ESELECTPHASE _selectPhase)
    {
        switch(_selectPhase)
        {
            case ESELECTPHASE.CHARACTER:
                SetCharacterButton();
                break;
            case ESELECTPHASE.WEAPON:
                SetWeaponButton();
                break;
            case ESELECTPHASE.STAGE:
                SetStageButton();
                break;
        }
    }
}
