using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharacterStatusInfo : MonoBehaviour
{
    [SerializeField] Image character_Thumbnail;
    [SerializeField] TextMeshProUGUI character_name;
    [SerializeField] TextMeshProUGUI character_Info;

    private GameObject _gameObject;

    public void SetActiveInfo(bool _isActive) => _gameObject.SetActive(_isActive);

    private void Awake()
    {
        //Enum.TryParse<ECharacterStatus>(value, out var res);


        _gameObject = this.gameObject;

        SetActiveInfo(false);
    }

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
