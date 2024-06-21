using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour , IPoolable
{
    private const float SPEED = 10f;

    private ItemManager itemManager;
    private SpriteRenderer spriteRenderer;
    private Vector2 pos;
    private Vector2 direction;
    private Vector2 startPos;
    private float range;
    private Transform myTransform;
    private ObbCollisionObject obbCollision;
    private DamageData damageData;


    public void Init()
    {
        myTransform = gameObject.GetComponent<RectTransform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        obbCollision = gameObject.GetComponent<ObbCollisionObject>();
        itemManager = ItemManager.getInstance;
        obbCollision.OnCollisionAction = OnCollisionTarget;
        obbCollision.SetIsCollisionCheck(false);
    }

    public void InitData()
    {
        direction = Vector2.zero;
        startPos = Vector2.zero;
        damageData = null;
        range = 0;
    }

    public void SetPrjectileInfo(DamageData _damageData, Vector2 _startPos, float _range)
    {
        myTransform.position = _startPos;
        damageData = _damageData;
        direction = _damageData.direction;
        startPos = _startPos;
        pos = startPos;
        range = _range;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

        myTransform.rotation = quaternion;
        obbCollision.SetIsCollisionCheck(true);
    }

    public void Fire()
    {
        pos.x += direction.x * Time.deltaTime * SPEED;
        pos.y += direction.y * Time.deltaTime * SPEED;

        myTransform.position = pos;

        if (Vector2.Distance(startPos, pos) >= range)
        {
            itemManager.EnqueueProjectile(this);
        }
    }

    private void Update()
    {
        if(direction == Vector2.zero)
        {
            return;
        }

        Fire();
    }

    public void OnEnqueue()
    {
        InitData();
        gameObject.SetActive(false);
    }

    public void OnDequeue()
    {
        gameObject.SetActive(true);
    }

    public void SetSprite(string _spriteName)
    {
        spriteRenderer.sprite = itemManager.GetSpriteToName(_spriteName);

        obbCollision.RefreshSprite();
    }

    public void SetTarget(params ITargetable[] _targets)
    {
        obbCollision.SetTarget(_targets);
    }

    public void OnCollisionTarget(ITargetable _target)
    {
        if(!gameObject.activeSelf)
        {
            return;
        }

        _target.OnDamaged(damageData);
        obbCollision.SetIsCollisionCheck(false);
        OnEnqueue();
    }
}
