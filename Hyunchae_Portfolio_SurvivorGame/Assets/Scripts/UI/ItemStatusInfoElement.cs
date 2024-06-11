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
    [SerializeField] Button sellButton;
    [SerializeField] Button combineButton;
    [SerializeField] TextMeshProUGUI itemPriceText;

    private GameObject _gameObject;

    public void SetActive(bool _isActive) => _gameObject.SetActive(_isActive);
    public Button.ButtonClickedEvent GetCloseButtonEvent => closeButton.onClick;
    public Button.ButtonClickedEvent GetSellButtonEvent => sellButton.onClick;
    public Button.ButtonClickedEvent GetCombineButton => combineButton.onClick;


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

    public void SetItemPrice(string _price)
    {
        itemPriceText.text = _price;
    }

    public void SetActiveCloseButton(bool _isActive)
    {
        closeButton.gameObject.SetActive(_isActive);
    }

    public void SetActiveSellButton(bool _isActive)
    {
        sellButton.gameObject.SetActive(_isActive);
    }

    public void SetActiveCombineButton(bool _isActive)
    {
        combineButton.gameObject.SetActive(_isActive);
    }
}
