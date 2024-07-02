using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : Singleton<GlobalData>
{
    private int piece;
    private bool isPause;

    public Action<int> OnRefreshPieceAction;

    public int GetPieceCount => piece;

    public void IncreasePieceCount(int _increaseValue)
    {
        piece += _increaseValue;
        OnRefreshPieceAction?.Invoke(piece);
    }

    public void DecreasePieceCount(int _decreaseValue)
    {
        piece -= _decreaseValue;
        OnRefreshPieceAction?.Invoke(piece);
    }

    public void SetPause(bool _isPause) => isPause = _isPause;

    public bool GetPause => isPause;

    public void UnloadScene()
    {
        piece = 0;
        isPause = false;
        OnRefreshPieceAction = null;
    }
}
