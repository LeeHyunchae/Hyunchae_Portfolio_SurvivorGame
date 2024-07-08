using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelController : UIBaseController
{
    [SerializeField] private TextMeshProUGUI pieceCountText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button mainSceneButton;
    [SerializeField] private GameObject deadTextObject;
    [SerializeField] private GameObject clearTextObject;


    private GlobalData globalData;
    private UIManager uiManager;
    private StageManager stageManager;

    protected override void Init()
    {
        base.Init();

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


    private void OnClickMainSceneButton()
    {
        globalData.SetGameEnd(true);
        SceneChanger.getInstance.ChangeScene("MainScene");
    }

    private void SetPieceCountText(int _count)
    {
        pieceCountText.text = _count.ToString();
    }

    public void OnDeadText()
    {
        deadTextObject.SetActive(true);
        clearTextObject.SetActive(false);
    }

    public void OnClearText()
    {
        deadTextObject.SetActive(false);
        clearTextObject.SetActive(true);
    }
}
