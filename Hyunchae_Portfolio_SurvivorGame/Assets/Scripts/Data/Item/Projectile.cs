using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour , IPoolable
{
    private const float SPEED = 10f;

    private ItemManager itemManager;
    private RectTransform _transform;
    private SpriteRenderer spriteRenderer;
    private Vector2 pos;
    private Vector2 direction;
    private Vector2 startPos;
    private float damage;
    private float range;


    public void Init()
    {
        _transform = GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemManager = ItemManager.getInstance;

    }

    public void InitData()
    {
        direction = Vector2.zero;
        startPos = Vector2.zero;
        damage = 0;
        range = 0;
    }

    public void SetPrjectileInfo(Vector2 _direction, float _damage, Vector2 _startPos, float _range)
    {
        _transform.position = _startPos;
        direction = _direction;
        startPos = _startPos;
        pos = startPos;
        damage = _damage;
        range = _range;
    }

    public void Fire()
    {
        pos.x += direction.x * Time.deltaTime * SPEED;
        pos.y += direction.y * Time.deltaTime * SPEED;

        _transform.position = pos;

        if (Vector2.Distance(startPos, pos) >= range)
        {
            itemManager.EnqueueProjectile(this);
        }
    }

    public void Update()
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
        _transform.gameObject.SetActive(false);
    }

    public void OnDequeue()
    {
        _transform.gameObject.SetActive(true);
    }

    public void SetSprite(string _spriteName)
    {
        spriteRenderer.sprite = itemManager.GetSpriteToName("Bullet 3");
    }
}
