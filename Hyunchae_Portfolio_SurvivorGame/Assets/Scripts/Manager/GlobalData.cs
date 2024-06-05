using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : Singleton<GlobalData>
{
    private int piece
    {
        get
        {
            return piece;
        }

        set
        {
            piece = value;
            OnRefreshPieceAction?.Invoke();
        }
    }

    public Action OnRefreshPieceAction;

    public void IncreasePieceCount(int _increaseValue)
    {
        piece += _increaseValue;
        OnRefreshPieceAction?.Invoke();
    }

    public void DecreasePieceCount(int _decreaseValue)
    {
        piece -= _decreaseValue;
        OnRefreshPieceAction?.Invoke();
    }
}
