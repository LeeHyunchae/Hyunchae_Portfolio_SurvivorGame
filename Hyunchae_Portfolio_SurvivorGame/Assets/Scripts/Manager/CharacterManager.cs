using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private List<CharacterModel> characterModels = new List<CharacterModel>();
    private Dictionary<int, CharacterModel> characterModelDict = new Dictionary<int, CharacterModel>();
    private Dictionary<int, Sprite> characterSpriteDict = new Dictionary<int, Sprite>();

    private Character playerCharacter;
    public Character GetPlayerCharacter => playerCharacter;
    public List<CharacterModel> GetAllCharacterModel => characterModels;

    
    public override bool Initialize()
    {
        playerCharacter = new Character();
        LoadCharacterInfo();
        return true;
    }

    private void LoadCharacterInfo()
    {
        characterModels = TableLoader.LoadFromFile<List<CharacterModel>>("Character/TestCharacter");

        int count = characterModels.Count;

        for(int i = 0; i < count; i++)
        {
            CharacterModel model = characterModels[i];

            characterModelDict.Add(model.characterUid, model);

            characterSpriteDict.Add(model.characterUid, Resources.Load<Sprite>(model.characterThumbnail));
        }
    }

    private CharacterModel GetCharacterModel(int _uid)
    {
        characterModelDict.TryGetValue(_uid , out CharacterModel model);

        if(model != null)
        {
            return model;
        }

        return null;
    }

    public void SelectCharacterModel(int _uid)
    {
        characterModelDict.TryGetValue(_uid, out CharacterModel model);

        if (model != null)
        {
            playerCharacter.SetModel(model);
        }
    }

    public Sprite GetCharacterSprite(int _uid)
    {
        characterSpriteDict.TryGetValue(_uid, out Sprite sprite);

        if(sprite != null)
        {
            return sprite;
        }

        Debug.Log("sprite path error");
        return null;
    }
}
