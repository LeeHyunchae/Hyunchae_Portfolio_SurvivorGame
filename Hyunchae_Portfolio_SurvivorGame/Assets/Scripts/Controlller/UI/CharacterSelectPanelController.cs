using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanelController : UIBaseController
{
    [SerializeField] private GameObject originSelectButtonItem;
    [SerializeField] private Transform characterListBG;
    [SerializeField] private CharacterStatusInfo statusInfo;
    [SerializeField] private Button closeButton;

    private UIManager uiManager;

    private List<CharacterModel> characters = new List<CharacterModel>();

    private void Start()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);

        InitData();
    }

    private void InitData()
    {
        characters = TableLoader.LoadFromFile<List<CharacterModel>>("Character/TestCharacter");

        CreateSelectButton();
    }

    private void CreateSelectButton()
    {
        int count = characters.Count;

        for(int i = 0; i<count;i++)
        {
            int index = i;

            CharacterSelectElement element = Instantiate<GameObject>(originSelectButtonItem,characterListBG).GetComponent<CharacterSelectElement>();

            element.Init();

            element.SetThumbnail(Resources.Load<Sprite>(characters[i].thumbnail_image));
            element.GetButtonClickedEvent.AddListener(() => OnClickCharacterButton(index));

            element.gameObject.SetActive(true);

        }
    }

    private void OnClickCharacterButton(int _index)
    {
        CharacterModel model = characters[_index];

        statusInfo.SetActiveInfo(true);
        statusInfo.SetCharacterName(model.character_Name);
        statusInfo.SetCharacterTumbnail(Resources.Load<Sprite>(model.thumbnail_image));

        StringBuilder stringBuilder = new StringBuilder();

        int count = model.variances.Count;

        for(int i = 0; i <count; i++)
        {
            stringBuilder.Append(model.variances[i].characterStatus + " : " + model.variances[i].variance + "\n");
        }

        stringBuilder.Append(model.ability_Info);

        statusInfo.SetCharacterInfo(stringBuilder.ToString());

    }

    private void OnClickCloseButton()
    {
        uiManager.Hide();
    }
}
