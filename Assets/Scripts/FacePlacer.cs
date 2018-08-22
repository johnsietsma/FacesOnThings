﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class FacePlacer : MonoBehaviour
{
    [SerializeField]
    Transform m_FacePlaneTransform;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    ARSessionOrigin m_SessionOrigin;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            // Ignore touches that are over the UI
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 touchPosition;
            if( Input.touchCount > 0 ) touchPosition = Input.GetTouch(0).position;
            else touchPosition = Input.mousePosition;

            if (m_SessionOrigin.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Debug.Log("Hit");
                Pose hitPose = s_Hits[0].pose;
                m_FacePlaneTransform.position = hitPose.position;
                m_FacePlaneTransform.rotation = hitPose.rotation;
            }
        }
    }
}
