using System;
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

    public Action OnHideAugmentPanelAction;

    private int curSelectAugmentIndex;

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

        RerollAugmentData();
    }

    private void InitButtons()
    {
        rerollButton.onClick.AddListener(OnClickRerollButton);
        selectCancleButton.onClick.AddListener(OnClickSelectCancleButton);
        selectConfirmButton.onClick.AddListener(OnClickSelectConfirmButton);

        int count = augmentElements.Length;

        for (int i = 0; i < count; i++)
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

        RerollAugmentData();
    }

    private void OnClickAugmentButton(int _augmentIndex)
    {
        augmentSelectPopup.SetActive(true);

        AugmentData data = augmentManager.GetAugmentData(augmentElements[_augmentIndex].GetAugmentUid);

        selectAugmentElement.SetAugmentUid(data.augmentUid);
        selectAugmentElement.SetAugmentNameText(data.augmentName);
        selectAugmentElement.SetAugmentInfoText(data.augmentInfo);

        curSelectAugmentIndex = _augmentIndex;

    }

    private void OnClickSelectCancleButton()
    {
        curSelectAugmentIndex = -1;
        augmentSelectPopup.SetActive(false);
    }

    private void OnClickSelectConfirmButton()
    {
        Hide();
        augmentSelectPopup.SetActive(false);
        OnHideAugmentPanelAction?.Invoke();
        augmentManager.SelectAugment(augmentElements[curSelectAugmentIndex].GetAugmentUid);
    }

    private void RerollAugmentData()
    {
        List<AugmentData> augmentDatas = augmentManager.GetRandomAugment();

        int count = augmentElements.Length;

        for(int i = 0; i<count; i ++)
        {
            AugmentElement element = augmentElements[i];
            AugmentData data = augmentDatas[i];

            element.SetAugmentUid(data.augmentUid);
            element.SetAugmentNameText(data.augmentName);
            element.SetAugmentInfoText(data.augmentInfo);
        }
    }
}
