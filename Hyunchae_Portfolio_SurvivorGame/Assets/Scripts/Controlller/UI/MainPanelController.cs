using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelController : UIBaseController
{
    private UIManager uiManager;

    [SerializeField] Button gamestartButton;

    protected override void Init()
    {
        base.Init();

        uiManager = UIManager.getInstance;
        gamestartButton.onClick.AddListener(OnClickGameStart);
    }

    private void OnClickGameStart()
    {
        uiManager.Show<CharacterSelectPanelController>("UI/CharacterSelectPanel");
    }
}
