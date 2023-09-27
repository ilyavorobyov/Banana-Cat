using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControlButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private float _moveDirection;

    public static Action StopMoveEvent;
    public static Action<float> DirectionChangeEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        DirectionChangeEvent?.Invoke(_moveDirection);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopMoveEvent?.Invoke();
    }
}