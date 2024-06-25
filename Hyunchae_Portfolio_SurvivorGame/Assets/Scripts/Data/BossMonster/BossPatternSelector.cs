using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternSelector
{
    private List<BaseBossAttackBehaviour> bossAttackBehaviours = new List<BaseBossAttackBehaviour>();
    private List<BaseBossMoveBehaviour> bossMoveBehiavours = new List<BaseBossMoveBehaviour>();

    private Dictionary<EBossMonsterSkill, IBossBehaviour> bossMonsterSkillDict = new Dictionary<EBossMonsterSkill, IBossBehaviour>();

    private Transform bossTransform;
    private ITargetable target;
    private float[] bossStatus;
    List<BossPatternModel> patternModels = new List<BossPatternModel>();

    public void Init()
    {
        bossMonsterSkillDict.Add(EBossMonsterSkill.DASH, new BossDashBehaviour());
        bossMonsterSkillDict.Add(EBossMonsterSkill.CIRCLESHOOT, new BossCircleShootBehaviour());
        bossMonsterSkillDict.Add(EBossMonsterSkill.TRIPLESHOOT, new BossTripleShootBehaviour());
        bossMonsterSkillDict.Add(EBossMonsterSkill.FOLLOWMOVE, new BossFolllowMoveBehaviour());
        bossMonsterSkillDict.Add(EBossMonsterSkill.HEXAGONSHOOT, new BossHexagonShootBehaviour());
        bossMonsterSkillDict.Add(EBossMonsterSkill.SEQUENCECIRCLESHOOT, new BossSequenceCircleShoot());
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

    public void SetBossPhase(List<BossPatternModel> _patternModels)
    {
        patternModels = _patternModels;
    }

    public BossPattern GetPattern(int _curPhaseCount)
    {
        BossPatternModel patternModel = patternModels[_curPhaseCount];

        int count = patternModel.skillList.Count;

        IBossBehaviour[] bossBehaviours = new IBossBehaviour[count];

        for(int i = 0; i < count; i ++)
        {
            bossMonsterSkillDict.TryGetValue(patternModel.skillList[i], out IBossBehaviour bossBehaviour);

            if(bossBehaviour != null)
            {
                bossBehaviour.Init();
                bossBehaviour.SetBossTransform(bossTransform);
                bossBehaviour.SetStatus(bossStatus);
                bossBehaviour.SetTarget(target);

                bossBehaviours[i] = bossBehaviour;
            }
        }

        BossPattern pattern = new BossPattern();

        pattern.SetPatternList(bossBehaviours);
        pattern.SetLogicType(patternModel.logicType);

        return pattern;
    }
}
