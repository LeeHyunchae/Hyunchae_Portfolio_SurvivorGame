using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class JoystickControlller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject joystickFrame;
    [SerializeField] private RectTransform LeverTransform;

    private RectTransform FrameTransform;

    private float radius;

    private Vector2 moveValue;
    private bool isDown = false;

    public Action<Vector2> OnPointerDownAction;
    public Action OnPointerUpAction;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        FrameTransform = joystickFrame.GetComponent<RectTransform>();
        radius = FrameTransform.rect.width * 0.3f;
    }

    public Action<Vector2> GetDownAction => OnPointerDownAction;

    public void OnDrag(PointerEventData eventData)
    {
        // 클릭 후 드래그 시
        moveValue = eventData.position - (Vector2)FrameTransform.position;
        LeverTransform.localPosition = Vector2.ClampMagnitude(moveValue, radius);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭 시
        joystickFrame.SetActive(true);
        FrameTransform.position = eventData.position;
        LeverTransform.position = eventData.position;

        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 클릭 해제 시
        joystickFrame.SetActive(false);
        LeverTransform.position = Vector3.zero;
        FrameTransform.position = Vector3.zero;

        isDown = false;
        moveValue = Vector2.zero;

        OnPointerUpAction?.Invoke();
    }

    private void Update()
    {
        if (isDown)
        {
            // 타겟(플레이어)에게 조이스틱 방향 보내기
            SendDirectionToTarget();
        }
    }

    private void SendDirectionToTarget()
    {
        if (moveValue == Vector2.zero)
        {
            return;
        }

        OnPointerDownAction?.Invoke(moveValue);
    }
}