using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class FaceTapToPlace : MonoBehaviour
{
    [SerializeField]
    GameObject m_FacePlacementGuide;

    [SerializeField]
    GameObject m_FacePrefab;

    ARSessionOrigin m_SessionOrigin;
    SpriteAnimator m_FacePlacementGuidSpriteAnimator;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    static readonly Vector3 s_ViewportCenter = new Vector3(0.5f, 0.5f, 0);

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_FacePlacementGuidSpriteAnimator = m_FacePlacementGuide.GetComponent<SpriteAnimator>();
    }

    void Update()
    {
        Ray screenRay = m_SessionOrigin.camera.ViewportPointToRay(s_ViewportCenter);
        Vector3 touchPosition = Vector3.zero;


        if (m_SessionOrigin.Raycast(screenRay, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;
            BoundedPlane plane = ARSubsystemManager.planeSubsystem.GetPlane(s_Hits[0].trackableId);

            // Place the face just abouve the plane to avoid z-fighting
            Vector3 facePositon = hitPose.position + hitPose.up * 0.1f;
            Quaternion faceRotation = GetFaceRotation(hitPose.position, plane.Pose);

            m_FacePlacementGuide.transform.position = facePositon;
            m_FacePlacementGuide.transform.rotation = faceRotation;

            // Get the mouse button as well for testing purposes.
#if UNITY_EDITOR
            bool isTouchDown = Input.GetMouseButtonDown(0);
#else
            bool isTouchDown = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
            if ( isTouchDown )
            {
                // Note: This wont work if the ARFoundationRaycaster is in use. It registers the pointer begin over objects.
                if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                GameObject faceObject = Instantiate(m_FacePrefab, facePositon, faceRotation, m_SessionOrigin.transform );
                SpriteAnimator spriteAnimator = faceObject.GetComponent<SpriteAnimator>();
                spriteAnimator.AnimationData = m_FacePlacementGuidSpriteAnimator.AnimationData;
                Debug.Log("Placing new face: " + m_FacePlacementGuidSpriteAnimator.AnimationData.name);

                // TODO: App flow: Deselect face, hide trackables.
                // TODO: App flow, don't pass anim data around, have central selected face with events.
            }
        }
    }

    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        var arFoundationModule = eventData.pointerCurrentRaycast.module as ARFoundationRaycaster;
        if (arFoundationModule == null) return;

        var currentRaycast = eventData.pointerCurrentRaycast;

        int trackableIndex = (int)currentRaycast.index;
        TrackableId trackableId = arFoundationModule.GetTrackableId(trackableIndex);
        BoundedPlane plane = ARSubsystemManager.planeSubsystem.GetPlane(trackableId);

        PlaceFace(currentRaycast.worldPosition, currentRaycast.worldNormal, plane.Pose);
    }
    */

    private Quaternion GetFaceRotation(Vector3 position, Pose planePose)
    {
        Vector3 towardsCam = (m_SessionOrigin.camera.transform.position - position).normalized;
        Vector3 right = -Vector3.Cross(towardsCam, planePose.forward);
        Vector3 forward = Vector3.Cross(right, planePose.up);
        return Quaternion.LookRotation(forward, planePose.up);
    }
}
