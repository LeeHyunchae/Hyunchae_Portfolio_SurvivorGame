using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectButtonElement : MonoBehaviour
{
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI thumbnailText;

    private Button button;
    public Button.ButtonClickedEvent GetButtonClickedEvent => button.onClick;

    public void SetActive(bool _isOn) => gameObject.SetActive(_isOn);
    public void Init()
    {
        button = GetComponent<Button>();
    }

    public void SetThumbnail(Sprite _image)
    {
        thumbnail.sprite = _image;

        if(_image == null)
        {
            thumbnail.gameObject.SetActive(false);
        }else
        {
            thumbnail.gameObject.SetActive(true);
        }
    }

    public void SetThumbnailText(string _text)
    {
        thumbnailText.text = _text;
    }
}
