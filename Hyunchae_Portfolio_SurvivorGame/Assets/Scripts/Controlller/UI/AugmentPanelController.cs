using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentPanelController : UIBaseController
{
    private const int REROLL_COUNT = 1;

    [SerializeField] AugmentElement[] augmentElements;
    [SerializeField] Button rerollButton;
    [SerializeField] TextMeshProUGUI rerollPriceText;
    [SerializeField] TextMeshProUGUI pieceCountText;
    [SerializeField] GameObject augmentSelectPopup;
    [SerializeField] AugmentElement selectAugmentElement;
    [SerializeField] Button selectCancleButton;
    [SerializeField] Button selectConfirmButton;

    private AugmentManager augmentManager;
    private int curRerollCount;

    protected override void Init()
    {
        base.Init();

        augmentManager = AugmentManager.getInstance;

        InitButtons();
    }

    public override void Show()
    {
        base.Show();

        curRerollCount = REROLL_COUNT;
        rerollButton.gameObject.SetActive(true);
    }

    private void InitButtons()
    {
        rerollButton.onClick.AddListener(OnClickRerollButton);
        selectCancleButton.onClick.AddListener(OnClickSelectCancleButton);
        selectConfirmButton.onClick.AddListener(OnClickSelectConfirmButton);

        int count = augmentElements.Length;

        for(int i = 0; i <count; i ++)
        {
            int index = i;
            augmentElements[i].GetSelectButtonClickEvent.AddListener(() => OnClickAugmentButton(index));
        }
    }

    private void OnClickRerollButton()
    {
        curRerollCount--;

        if(curRerollCount == 0)
        {
            rerollButton.gameObject.SetActive(false);
        }
    }

    private void OnClickAugmentButton(int _augmentIndex)
    {

    }

    private void OnClickSelectCancleButton()
    {

    }

    private void OnClickSelectConfirmButton()
    {

    }
}
