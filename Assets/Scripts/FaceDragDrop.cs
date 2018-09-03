using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private enum DragTarget { None, Canvas, ARTrackable };

    [SerializeField]
    GameObject m_FacePrefab;

    [SerializeField]
    [Tooltip("The UI sprite animator component that will move moved during the drag, and hold the currently dragged animation")]
    private SpriteAnimator m_DragImageAnimator;

    [SerializeField]
    [Tooltip("The mesh sprite animation that is moved over the pane during a drag.")]
    private SpriteAnimator m_DragMeshAnimator;

    DragTarget m_DragTarget;

    void Awake()
    {
        UpdateDragTarget(DragTarget.None);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Begin Drag: {eventData.pointerPress}");

        SpriteAnimator spriteAnimator = eventData.pointerPress.GetComponent<SpriteAnimator>();
        if (spriteAnimator == null) return; // We only drag sprite animations

        // Set the animation data
        m_DragImageAnimator.AnimationData = spriteAnimator.AnimationData;
        m_DragMeshAnimator.AnimationData = spriteAnimator.AnimationData;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var raycasterModule = eventData.pointerCurrentRaycast.module;
        if (raycasterModule != null && raycasterModule is ARFoundationRaycaster)
        {
            // We're over an AR trackable
            var raycast = eventData.pointerCurrentRaycast;
            Vector3 worldPos = raycast.worldPosition;
            Vector3 worldNormal = raycast.worldNormal;
            //Debug.Log("On Drag: " + eventData.position + " wp: " + eventData.pointerCurrentRaycast.worldPosition);
            m_DragMeshAnimator.transform.position = worldPos;
            m_DragMeshAnimator.transform.up = worldNormal;
            UpdateDragTarget(DragTarget.ARTrackable);
        }
        else
        {
            // We're over the UI
            m_DragImageAnimator.transform.position = eventData.position;
            m_DragMeshAnimator.gameObject.SetActive(false);
            UpdateDragTarget(DragTarget.Canvas);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        if (m_DragTarget == DragTarget.ARTrackable)
        {
            // TODO: Add reference point
            // TODO: Handle scale

            // Instantiate and set animation data
            GameObject faceObject = Instantiate(m_FacePrefab, m_DragMeshAnimator.transform.position, m_DragMeshAnimator.transform.rotation);
            SpriteAnimator spriteAnimator = faceObject.GetComponent<SpriteAnimator>();
            spriteAnimator.AnimationData = m_DragMeshAnimator.AnimationData;
        }
        UpdateDragTarget(DragTarget.None);
    }

    private void UpdateDragTarget(DragTarget dragTarget)
    {
        if (m_DragTarget != dragTarget)
        {
            m_DragTarget = dragTarget;
            Debug.Log($"New drag target {dragTarget}");

            bool isActive_Mesh = false;
            bool isActive_Image = false;

            switch (dragTarget)
            {
                case DragTarget.ARTrackable:
                    isActive_Mesh = true; break;
                case DragTarget.Canvas:
                    isActive_Image = true; break;
                default:
                    break; // fallthrough
            }

            m_DragMeshAnimator.gameObject.SetActive(isActive_Mesh);

            // It's usually more efficient to disbale the canvas. But we want to stop the sprite animation as well.
            // There is only one Image here, so rebuilding the canvas on drag is no a huge cost.
            m_DragImageAnimator.gameObject.SetActive(isActive_Image);
        }
    }
}
