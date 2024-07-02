using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngamePanelController : MonoBehaviour
{
    private const int TIMETEXT_CHANGE_TIME = 5;

    [SerializeField] private TextMeshProUGUI pieceCountText;
    [SerializeField] private TextMeshProUGUI waveTimeText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button itemBoxButton;

    private GlobalData globalData;
    private UIManager uiManager;
    private float waveTime;
    private float curWaveTime;
    private Color whiteColor = Color.white;
    private Color redColor = Color.red;

    private bool isWaveEnd = false;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
        pauseButton.onClick.AddListener(OnClickItemBoxButton);

        globalData = GlobalData.getInstance;
        uiManager = UIManager.getInstance;

        SetPieceCountText(globalData.GetPieceCount);
        globalData.OnRefreshPieceAction += SetPieceCountText;
    }

    private void OnClickPauseButton()
    {
        globalData.SetPause(true);
        uiManager.Show<PausePanelController>("UI/PausePanel");
    }

    private void OnClickItemBoxButton()
    {

    }

    public void SetLevelText(string _level)
    {
        levelText.text = _level;
    }

    public void SetPieceCountText(int _pieceCount)
    {
        Debug.Log("Refresh IngamePanel PieceCount");
        pieceCountText.text = _pieceCount.ToString();
    }

    public void SetWaveText(string _wave)
    {
        waveText.text = _wave;
    }

    public void SetWaveTimeText(string _time)
    {
        waveTimeText.text = _time;
    }

    public void StartWave(int _wave, float _waveTime)
    {
        isWaveEnd = false;
        waveTime = _waveTime;
        curWaveTime = _waveTime;

        SetWaveText(_wave.ToString());
        SetWaveTimeText(waveTime.ToString());
        waveTimeText.color = whiteColor;
    }

    private void Update()
    {
        if(isWaveEnd)
        {
            return;
        }

        curWaveTime -= Time.deltaTime;

        SetWaveTimeText(((int)curWaveTime).ToString());
        
        if(curWaveTime <= TIMETEXT_CHANGE_TIME)
        {
            waveTimeText.color = redColor;
        }
    }

    public void EndWave()
    {
        isWaveEnd = true;
        curWaveTime = 0;
        SetWaveTimeText(0.ToString());

        waveTimeText.color = Color.white;
    }
}
