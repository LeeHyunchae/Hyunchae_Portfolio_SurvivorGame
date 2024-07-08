using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObbCollisionObject : MonoBehaviour
{
    private float BOUNDSIZE_CORRECTION = 0.7f;
    private ITargetable[] targetArr;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector2 spriteSize;

    private int targetCount;

    private bool isCollisionCheck = true;


    public Action<ITargetable> OnCollisionAction;

    private List<ITargetable> alreadyCollisionList = new List<ITargetable>();

    private void Awake()
    {
        myTransform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void RefreshSprite()
    {
        spriteSize = spriteRenderer.bounds.size * BOUNDSIZE_CORRECTION;
    }

    private void Update()
    {
        if (!isCollisionCheck)
        {
            return;
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        for (int i = 0; i < targetCount; i++)
        {
            ITargetable target = targetArr[i];

            if (target.GetIsDead())
            {
                continue;
            }

            if (IsCollision(target))
            {
                int count = alreadyCollisionList.Count;
                bool isAlreadyCollision = false;
                for (int j = 0; j < count; j++)
                {
                    if (target == alreadyCollisionList[j])
                    {
                        isAlreadyCollision = true;
                        break;
                    }
                }

                if(isAlreadyCollision)
                {
                    continue;
                }
                alreadyCollisionList.Add(target);
                OnCollisionAction?.Invoke(target);
            }
        }
    }

    public void SetTarget(params ITargetable[] _targetArr)
    {
        targetArr = _targetArr;

        targetCount = _targetArr.Length;
    }

    public void SetIsCollisionCheck(bool _isCheck) 
    {
        isCollisionCheck = _isCheck;

        if (isCollisionCheck)
        {
            alreadyCollisionList.Clear(); 
        }
    }

    private bool IsCollision(ITargetable _target)
    {
        Vector2 distance = GetCenterDistanceVector(_target);
        // 검사할 타겟과 자신의 높이, 너비 벡터 구하기
        Vector2[] vec = new Vector2[4]
        {
            GetMyHeightVector(),
            GetTargetHeightVector(_target),
            GetMyWidthVector(),
            GetTargetWidthVector(_target)
        };

        Vector2 unitVec;

        // 타겟과 자신의 높이, 너비벡터를 단위벡터로 하는 길이 검사 실시
        for (int i = 0; i < 4; i++)
        {
            float sum = 0f;
            unitVec = GetUnitVector(vec[i]);
            for (int j = 0; j < 4; j++)
            {
                sum += Mathf.Abs(vec[j].x * unitVec.x + vec[j].y * unitVec.y);
            }

            if (Mathf.Abs(distance.x * unitVec.x + distance.y * unitVec.y) > sum)
            {
                return false;
            }
        }
        return true;
    }

    private Vector2 GetCenterDistanceVector(ITargetable _target)
    {
        Vector2 myPos = myTransform.position;

        return myPos - _target.GetPosition();
    }

    private Vector2 GetMyHeightVector()
    {
        float x =  spriteSize.y * Mathf.Cos(Deg2Rad(myTransform.eulerAngles.z - 90f)) * 0.5f;
        float y =  spriteSize.y * Mathf.Sin(Deg2Rad(myTransform.eulerAngles.z - 90f)) * 0.5f;

        return new Vector2(x, y);
    }

    private Vector2 GetTargetHeightVector(ITargetable _target)
    {
        float x = _target.GetSpriteBounds().size.y * BOUNDSIZE_CORRECTION * Mathf.Cos(Deg2Rad(_target.GetTransform().eulerAngles.z - 90f)) * 0.5f;
        float y = _target.GetSpriteBounds().size.y * BOUNDSIZE_CORRECTION * Mathf.Sin(Deg2Rad(_target.GetTransform().eulerAngles.z - 90f)) * 0.5f;

        return new Vector2(x, y);
    }

    private Vector2 GetMyWidthVector()
    {
        float x = spriteSize.x * Mathf.Cos(Deg2Rad(myTransform.eulerAngles.z)) * 0.5f;
        float y = spriteSize.x * Mathf.Sin(Deg2Rad(myTransform.eulerAngles.z)) * 0.5f;

        return new Vector2(x, y);
    }

    private Vector2 GetTargetWidthVector(ITargetable _target)
    {
        float x = _target.GetSpriteBounds().size.x * BOUNDSIZE_CORRECTION * Mathf.Cos(Deg2Rad(_target.GetTransform().eulerAngles.z)) * 0.5f;
        float y = _target.GetSpriteBounds().size.x * BOUNDSIZE_CORRECTION * Mathf.Sin(Deg2Rad(_target.GetTransform().eulerAngles.z)) * 0.5f;

        return new Vector2(x, y);
    }

    private Vector2 GetUnitVector(Vector2 _vector)
    {
        float len = Mathf.Sqrt(Mathf.Pow(_vector.x, 2) + Mathf.Pow(_vector.y, 2));

        return new Vector2(_vector.x / len, _vector.y / len);
    }

    private float Deg2Rad(float _deg)
    {
        return _deg / 180 * Mathf.PI;
    }
}
