using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectElement : MonoBehaviour
{
    [SerializeField] private Image thumbnail;

    private Button button;
    public Button.ButtonClickedEvent GetButtonClickedEvent => button.onClick;

    public void Init()
    {
        button = GetComponent<Button>();
    }

    public void SetThumbnail(Sprite _image)
    {
        thumbnail.sprite = _image;
    }

}
