using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ARPlaneDragMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector3 m_LastPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_LastPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        BaseRaycaster raycaster = eventData.pointerCurrentRaycast.module;
        if (raycaster == null) return;
        Vector3 currentPosition = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 deltaPosition = currentPosition - m_LastPosition;
        transform.Translate(deltaPosition);
        m_LastPosition = currentPosition;
        Debug.Log("Drag: " + currentPosition);
    }
}
