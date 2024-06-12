using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentElement : MonoBehaviour
{
    [SerializeField] Button selectButton;
    [SerializeField] Image thumbnailImage;
    [SerializeField] TextMeshProUGUI augmentNameText;
    [SerializeField] TextMeshProUGUI augmentInfoText;

    public Button.ButtonClickedEvent GetSelectButtonClickEvent;

    public void SetThumnailImage(Sprite _sprite)
    {
        thumbnailImage.sprite = _sprite;
    }

    public void SetAugmentNameText(string _name)
    {
        augmentNameText.text = _name;
    }

    public void SetAugmentInfoText(string _info)
    {
        augmentInfoText.text = _info;
    }
}
