using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternSelector
{
    private List<BaseBossAttackBehaviour> bossAttackBehaviours = new List<BaseBossAttackBehaviour>();
    private List<BaseBossMoveBehaviour> bossMoveBehiavours = new List<BaseBossMoveBehaviour>();

    private Transform bossTransform;
    private ITargetable target;
    private float[] bossStatus;

    public void Init()
    {
        BossTripleShootBehaviour bossCircleShootBehaviour = new BossTripleShootBehaviour();
        bossCircleShootBehaviour.Init();

        bossAttackBehaviours.Add(bossCircleShootBehaviour);
        bossMoveBehiavours.Add(new BossFolllowMoveBehaviour());
    }

    public void SetBossTransform(Transform _transform)
    {
        bossTransform = _transform;
    }

    public void SetTarget(ITargetable _target)
    {
        target = _target;
    }

    public void SetBossStatus(float[] _status)
    {
        bossStatus = _status;
    }

    public BossPattern GetPattern()
    {
        BossPattern pattern = new BossPattern();

        bossAttackBehaviours[0].Init();
        bossAttackBehaviours[0].SetBossTransform(bossTransform);
        bossAttackBehaviours[0].SetStatus(bossStatus);
        bossAttackBehaviours[0].SetTarget(target);

        bossMoveBehiavours[0].Init();
        bossMoveBehiavours[0].SetBossTransform(bossTransform);
        bossMoveBehiavours[0].SetStatus(bossStatus);
        bossMoveBehiavours[0].SetTarget(target);

        pattern.SetPatternList(bossAttackBehaviours[0], bossMoveBehiavours[0]);
        pattern.SetLogicType(EMonsterLogicType.LOOP);

        return pattern;
    }
}
