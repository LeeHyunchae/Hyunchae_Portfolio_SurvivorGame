using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterTable : Singleton<CharacterTable>
{
    private Dictionary<int, CharacterModel> characterModelDict = new Dictionary<int, CharacterModel>();

    public override bool Initialize()
    {
        LoadJsonFile();

        return base.Initialize();
    }

    public void LoadJsonFile()
    {
        List<CharacterModel> characters = new List<CharacterModel>();
        characters = TableLoader.LoadFromFile<List<CharacterModel>>("Character/TestCharacter");

        int count = characters.Count;

        for(int i = 0; i < count; i ++)
        {
            CharacterModel model = characters[i];
            characterModelDict.Add(model.characterUid, model);
        }
    }


}
