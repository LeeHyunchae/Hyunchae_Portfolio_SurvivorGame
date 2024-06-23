using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern
{
    private IBossBehaviour[] behaviourList;
    private EMonsterLogicType logicType;
    private int curSequenceNum;
    private int curLoopNum;

    public Action OnEndPatternAction;

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
            if (logicType == EMonsterLogicType.SEQUENCE)
            {
                behaviourList[i].SetOnEndBehaviourAction(EndSequenceBehaviour);
            }
            else
            {
                behaviourList[i].SetOnEndBehaviourAction(EndLoopBehaviour);
            }

        }

    }

    private void UpdateSequenceType()
    {
        behaviourList[curSequenceNum].Update();
    }

    private void UpdateLoopType()
    {
        int count = behaviourList.Length;

        for(int i = 0; i <count; i++)
        {
            behaviourList[i].Update();
        }
    }

    private void EndLoopBehaviour()
    {
        curLoopNum++;

        if(curLoopNum >= behaviourList.Length)
        {
            Debug.Log("Pattern End!");
            OnEndPatternAction?.Invoke();
        }
    }

    private void EndSequenceBehaviour()
    {
        curSequenceNum++;

        if(curSequenceNum >= behaviourList.Length)
        {
            //Todo Pattern Exit
            Debug.Log("Pattern End!");
            OnEndPatternAction?.Invoke();
        }
    }
}
