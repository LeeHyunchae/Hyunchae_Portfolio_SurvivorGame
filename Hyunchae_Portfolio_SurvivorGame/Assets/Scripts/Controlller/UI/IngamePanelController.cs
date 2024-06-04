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

    private ItemManager itemManager;
    private float waveTime;
    private float curWaveTime;
    private Color whiteColor = Color.white;
    private Color redColor = Color.red;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
        pauseButton.onClick.AddListener(OnClickItemBoxButton);

        itemManager = ItemManager.getInstance;
    }

    private void OnClickPauseButton()
    {

    }

    private void OnClickItemBoxButton()
    {

    }

    public void SetLevelText(string _level)
    {
        levelText.text = _level;
    }

    public void SetPieceCountText(string _pieceCount)
    {
        pieceCountText.text = _pieceCount;
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
        Debug.Log(_waveTime);
        waveTime = _waveTime;
        curWaveTime = _waveTime;

        SetWaveText(_wave.ToString());
        SetWaveTimeText(waveTime.ToString());
        waveTimeText.color = whiteColor;
    }

    private void Update()
    {
        curWaveTime -= Time.deltaTime;

        SetWaveTimeText(((int)curWaveTime).ToString());
        
        if(curWaveTime <= TIMETEXT_CHANGE_TIME)
        {
            waveTimeText.color = redColor;
        }
    }
}
