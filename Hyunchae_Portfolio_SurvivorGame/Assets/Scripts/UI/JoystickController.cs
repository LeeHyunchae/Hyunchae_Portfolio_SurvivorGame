using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class JoystickController : MonoBehaviour, IPointerDownHandler,IPointerMoveHandler,IPointerUpHandler
{
    private const float RADIUS_COLLECT_VALUE = 0.3f;

    [SerializeField] private Image joystick_Frame;
    [SerializeField] private Image joystick_Lever;

    private bool isDown = false;
    private float radius;

    private RectTransform frameTransform;
    private RectTransform leverTransform;

    public Action<Vector2> OnJoystickDown;

    private void Awake()
    {
        frameTransform = joystick_Frame.GetComponent<RectTransform>();
        leverTransform = joystick_Lever.GetComponent<RectTransform>();

        radius = frameTransform.rect.width * RADIUS_COLLECT_VALUE;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        frameTransform.position = eventData.position;

        joystick_Frame.enabled = true;
        joystick_Lever.enabled = true;

        isDown = true;

        OnTouch(eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(!isDown)
        {
            return;
        }


        OnTouch(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick_Frame.enabled = false;
        joystick_Lever.enabled = false;

        leverTransform.position = Vector2.zero;

        isDown = false;
    }

    private void OnTouch(Vector2 _pos)
    {
        leverTransform.position = _pos - frameTransform.anchoredPosition;

        Vector2 leverPos = new Vector2(_pos.x - frameTransform.position.x, _pos.y - frameTransform.position.y);

        leverPos = Vector2.ClampMagnitude(leverPos, radius);

        leverTransform.localPosition = leverPos;

        //float sqr = (frameTransform.position - leverTransform.position).sqrMagnitude / (radius * radius);
        //Vector2 vecNormal = leverPos.normalized;

        //Vector3 move = new Vector3(vecNormal.x, 0f, vecNormal.y);

        OnJoystickDown?.Invoke(leverPos.normalized);
    }
}
