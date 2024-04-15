using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusInfo : MonoBehaviour
{
    [SerializeField] Image character_Thumbnail;
    [SerializeField] TextMeshProUGUI character_name;
    [SerializeField] TextMeshProUGUI character_Info;

    public void SetCharacterTumbnail(Sprite _image)
    {
        character_Thumbnail.sprite = _image;
    }

    public void SetCharacterName(string _name)
    {
        character_name.text = _name;
    }

    public void SetCharacterInfo(string _info)
    {
        character_Info.text = _info;
    }
}
