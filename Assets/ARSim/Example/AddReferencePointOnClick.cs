using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARSimExample
{
    public class AddReferencePointOnClick : MonoBehaviour
    {
        [SerializeField]
        ARSessionOrigin m_SessionOrigin;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (!m_SessionOrigin.Raycast(Input.mousePosition, s_Hits, TrackableType.PlaneWithinPolygon))
                return;

            var hit = s_Hits[0];

            var referencePointManager = m_SessionOrigin.GetComponent<ARReferencePointManager>();
            var planeManager = m_SessionOrigin.GetComponent<ARPlaneManager>();

            var plane = planeManager.TryGetPlane(hit.trackableId);
            if (plane == null)
                return;

            referencePointManager.TryAttachReferencePoint(plane, new Pose(hit.pose.position + Vector3.up * .1f, hit.pose.rotation));
        }
    }
}
