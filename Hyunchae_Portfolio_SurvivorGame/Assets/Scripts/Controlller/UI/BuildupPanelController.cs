using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildupPanelController : UIBaseController
{
    [SerializeField] BuildupElement[] buildupElements;
    [SerializeField] Button rerollButton;
    [SerializeField] TextMeshProUGUI rerollPriceText;
    [SerializeField] TextMeshProUGUI pieceCountText;
    [SerializeField] GameObject buildupSelectPopup;
    [SerializeField] BuildupElement selectBuildupElement;
    [SerializeField] Button selectCancleButton;
    [SerializeField] Button selectConfirmButton;

    protected override void Init()
    {
        base.Init();
    }
}
