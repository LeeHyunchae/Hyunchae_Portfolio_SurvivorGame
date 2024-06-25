using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    private const int POS_CORRECTION_VALUE = 150;
    [SerializeField] private Image hpBarBGImage;
    [SerializeField] private Image hpBarImage;

    private Camera cam;

    public void SetActive(bool _isActive) => gameObject.SetActive(_isActive);

    public void UpdatePos(Vector2 _playerPos)
    {
        Vector2 hpBarPos = cam.WorldToScreenPoint(_playerPos);
        hpBarPos.y += POS_CORRECTION_VALUE;

        hpBarBGImage.transform.position = hpBarPos;
    }

    public void SetHPBarFillAmount(float _amount)
    {
        hpBarImage.fillAmount = _amount;
    }

    public void Init()
    {
        cam = Camera.main;
    }

    public void OnEnqueue()
    {
        SetActive(false);
    }

    public void OnDequeue()
    {
        SetActive(true);
    }
}
