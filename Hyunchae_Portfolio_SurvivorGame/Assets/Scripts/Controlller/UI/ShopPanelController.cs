using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : UIBaseController
{
    private UIManager uiManager;

    [SerializeField] Button gamestartButton;

    public Action OnClickNextWaveAction;

    protected override void Init()
    {
        base.Init();

        uiManager = UIManager.getInstance;
        gamestartButton.onClick.AddListener(OnClickNextWave);
    }

    private void OnClickNextWave()
    {
        uiManager.Hide();

        OnClickNextWaveAction?.Invoke();
    }
}
