using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemStatusInfoElement : MonoBehaviour
{
    [SerializeField] Image thumbnailImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemInfoText;
    [SerializeField] Button closeButton;

    private GameObject _gameObject;

    public void SetActive(bool _isActive) => _gameObject.SetActive(_isActive);

    private void Awake()
    {
        _gameObject = this.gameObject;

        SetActive(false);

        closeButton.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        SetActive(false);
    }

    public void SetTumbnail(Sprite _image)
    {
        thumbnailImage.sprite = _image;
    }

    public void SetName(string _name)
    {
        itemNameText.text = _name;
    }

    public void SetInfoText(string _info)
    {
        itemInfoText.text = _info;
    }

    public void SetActiveCloseButton(bool _isActive)
    {
        closeButton.gameObject.SetActive(_isActive);
    }
}
