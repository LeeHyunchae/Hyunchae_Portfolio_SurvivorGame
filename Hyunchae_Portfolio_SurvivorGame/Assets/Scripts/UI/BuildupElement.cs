using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildupElement : MonoBehaviour
{
    [SerializeField] Button selectButton;
    [SerializeField] Image thumbnailImage;
    [SerializeField] TextMeshProUGUI buildupNameText;
    [SerializeField] TextMeshProUGUI buildupInfoText;

    public Button.ButtonClickedEvent GetSelectButtonClickEvent;

    public void SetThumnailImage(Sprite _sprite)
    {
        thumbnailImage.sprite = _sprite;
    }

    public void SetBuildupNameText(string _name)
    {
        buildupNameText.text = _name;
    }

    public void SetBuildupInfoText(string _info)
    {
        buildupInfoText.text = _info;
    }
}
