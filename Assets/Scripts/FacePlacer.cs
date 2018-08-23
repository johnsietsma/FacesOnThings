using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class FacePlacer : MonoBehaviour, IPointerDownHandler
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
            if (Input.touchCount > 0) touchPosition = Input.GetTouch(0).position;
            else touchPosition = Input.mousePosition;

            if (m_SessionOrigin.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                //Pose pose = s_Hits[0].pose;
                //BoundedPlane plane = ARSubsystemManager.planeSubsystem.GetPlane(s_Hits[0].trackableId);
                //PlaceFace( pose.position, pose.up, plane.Pose);
            }
        }
    }


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

    private void PlaceFace( Vector3 position, Vector3 normal, Pose planePose )
    {
        m_FacePlaneTransform.position = position + normal * 0.1f;

        Vector3 towardsCam = (m_SessionOrigin.camera.transform.position - position).normalized;
        Vector3 right = -Vector3.Cross(towardsCam, planePose.forward);
        Vector3 forward = Vector3.Cross(right, planePose.up);
        m_FacePlaneTransform.rotation = Quaternion.LookRotation(forward, planePose.up);


        //Debug.LogFormat("Hit Pose - {0} {1}", hitPose.rotation.eulerAngles, sessionPose.rotation.eulerAngles);
        //Debug.LogFormat("Hit Pose - Up:{0:0.####} Fwd:{1:0.####} Right:{2:0.####}", hitPose.up, hitPose.forward, hitPose.right);
        //Debug.LogFormat("ToCam:{0} Fwd:{1} Right:{2} Euler:{3:0.####}", towardsCam, forward, right, m_FacePlaneTransform.rotation.eulerAngles);
    }
}
