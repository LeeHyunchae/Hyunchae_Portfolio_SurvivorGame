using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelControlller : MonoBehaviour
{
    [SerializeField] private Sprite originWarningImage;



    private void Awake()
    {
        LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite warningSprite = Instantiate<Sprite>(originWarningImage, this.transform);
    }

    private void Update()
    {
        
    }
}
