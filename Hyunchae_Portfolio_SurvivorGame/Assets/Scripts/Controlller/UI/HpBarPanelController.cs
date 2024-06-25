using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarPanelController : UIBaseController
{
    [SerializeField] private HpBarController originHpBarObject;

    public HpBarController CreateHpBar()
    {
        HpBarController hpBar = Instantiate<HpBarController>(originHpBarObject, this.transform);

        return hpBar;
    }
}
