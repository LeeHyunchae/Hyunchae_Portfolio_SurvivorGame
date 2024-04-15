using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanelController : UIBaseController
{
    [SerializeField] private GameObject originSelectButtonItem;
    [SerializeField] private CharacterStatusInfo statusInfo;
    [SerializeField] private Button closeButton;

    private UIManager uiManager;

    private Button[] selectButtons;
    private CharacterSelectElement[] characterSelectElements;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void Test()
    {

    }

    private void OnClickCloseButton()
    {
        uiManager.Hide();
    }
}
