using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour , IPoolable
{

    private SpriteRenderer spriteRenderer;
    private MonsterModel monsterModel;
    private Transform _transform;
    private Transform targetTransform;
    private BaseMonsterBehaviourRoutine behaviourRoutine;
    public void Init()
    {
        _transform = this.transform;
        spriteRenderer = _transform.GetComponent<SpriteRenderer>();

    }
    public void SetMonsterModel(MonsterModel _monsterModel)
    {
        monsterModel = _monsterModel;

    }

    public void SetPlayerTransform(Transform _target)
    {
        targetTransform = _target;

    }

    public void Update()
    {
        if (monsterModel == null || targetTransform == null)
        {
            return;
        }

        behaviourRoutine.Update();
    }



    public void OnEnqueue()
    {
    }

    public void OnDequeue()
    {
    }
}
