using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : UIBaseController
{
    [SerializeField] private TextMeshProUGUI pieceCountText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainSceneButton;

    private GlobalData globalData;
    private UIManager uiManager;
    private StageManager stageManager;

    protected override void Init()
    {
        base.Init();

        continueButton.onClick.AddListener(OnClickContinueButton);
        mainSceneButton.onClick.AddListener(OnClickMainSceneButton);

        globalData = GlobalData.getInstance;
        uiManager = UIManager.getInstance;
        stageManager = StageManager.getInstance;

    }

    public override void Show()
    {
        base.Show();
        SetWaveText();
        SetPieceCountText(globalData.GetPieceCount);
    }

    private void SetWaveText()
    {
        int stage = stageManager.GetCurStage;
        int wave = stageManager.GetCurWave;

        waveText.text = stage + " - " + wave;
    }

    private void OnClickContinueButton()
    {
        globalData.SetPause(false);
        uiManager.Hide();
    }

    private void OnClickMainSceneButton()
    {
        SceneChanger.getInstance.ChangeScene("MainScene");
    }

    private void SetPieceCountText(int _count)
    {
        pieceCountText.text = _count.ToString();
    }
}
