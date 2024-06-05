using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusName;
    [SerializeField] TextMeshProUGUI statusValue;

    public void SetStatusName(string _name)
    {
        statusName.text = _name;
    }

    public void SetStatusValue(string _value)
    {
        statusValue.text = _value;
    }
}
