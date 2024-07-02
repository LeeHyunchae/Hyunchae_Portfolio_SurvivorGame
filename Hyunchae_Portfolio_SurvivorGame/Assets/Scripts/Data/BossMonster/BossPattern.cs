using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern
{
    private IBossBehaviour[] behaviourList;
    private EMonsterLogicType logicType;

    private int curEndBehaviourCount;

    public Action OnEndPatternAction;

    public void InitPatterns()
    {
        int count = behaviourList.Length;

        for(int i = 0; i < count; i ++)
        {
            behaviourList[i].Init();
        }
    }

    public void SetPatternList(params IBossBehaviour[] _pattern)
    {
        behaviourList = _pattern;
    }

    public void SetLogicType(EMonsterLogicType _logicType)
    {
        logicType = _logicType;

        SetBehaviourAction();
    }

    public void Update()
    {
        if(logicType == EMonsterLogicType.SEQUENCE)
        {
            UpdateSequenceType();
        }
        else
        {
            UpdateLoopType();
        }
    }

    private void SetBehaviourAction()
    {

        int count = behaviourList.Length;

        for (int i = 0; i < count; i++)
        {
            behaviourList[i].SetOnEndBehaviourAction(EndBehaviour);
        }

    }

    private void UpdateSequenceType()
    {
        behaviourList[curEndBehaviourCount].Update();
    }

    private void UpdateLoopType()
    {
        int count = behaviourList.Length;

        for(int i = 0; i <count; i++)
        {
            behaviourList[i].Update();
        }
    }

    private void EndBehaviour()
    {
        curEndBehaviourCount++;

        if(curEndBehaviourCount >= behaviourList.Length)
        {
            Debug.Log("Pattern End!");
            curEndBehaviourCount = 0;
            OnEndPatternAction?.Invoke();
        }
    }

}
