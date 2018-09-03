using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.EventSystems
{
    /// <summary>
    /// Simple event system using AR Foundation raycasts.
    /// </summary>
    [AddComponentMenu("Event/ARFoundation Raycaster")]
    public class ARFoundationRaycaster : BaseRaycaster
    {
        [SerializeField]
        ARSessionOrigin m_SessionOrigin;

        /// <summary>
        /// Layer mask used to filter events. Always combined with the camera's culling mask if a camera is used.
        /// </summary>
        [SerializeField]
        TrackableType m_TrackableTypeMask = TrackableType.All;

        /// <summary>
        /// Use this to determine if hits from this raycaster take precendence over other raycasters.
        /// </summary>
        [SerializeField]
        int m_SortingOrder;

        List<ARRaycastHit> m_raycastHits = new List<ARRaycastHit>();
        ARPlaneManager m_PlaneManger;
        ARReferencePointManager m_referencePointManager;

        public override Camera eventCamera
        {
            get { return arSessionOrigin.camera; }
        }

        public override int sortOrderPriority
        {
            get { return m_SortingOrder; }
        }

        public ARSessionOrigin arSessionOrigin
        {
            get
            {
                if (m_SessionOrigin == null)
                    m_SessionOrigin = GetComponent<ARSessionOrigin>();
                return m_SessionOrigin;
            }
        }

        /// <summary>
        /// Layer mask used to filter events. Always combined with the camera's culling mask if a camera is used.
        /// </summary>
        public TrackableType trackableTypeMask
        {
            get { return m_TrackableTypeMask; }
            set { m_TrackableTypeMask = value; }
        }


        /// <summary>
        /// The index of the RaycastResult can be used to find the trackableId.
        /// Use TrackableId id = (int)eventData.pointerCurrentRaycast.index;
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TrackableId GetTrackableId(int index)
        {
            if (index < m_raycastHits.Count) return m_raycastHits[index].trackableId;
            return TrackableId.InvalidId;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (eventCamera == null || !eventCamera.pixelRect.Contains(eventData.position))
                return;

            if (m_PlaneManger == null) m_PlaneManger = arSessionOrigin.GetComponent<ARPlaneManager>();
            if (m_referencePointManager == null) m_referencePointManager = arSessionOrigin.GetComponent<ARReferencePointManager>();


            if (arSessionOrigin.Raycast(eventData.position, m_raycastHits, trackableTypeMask))
            {
                m_raycastHits.Sort((r1, r2) => r1.distance.CompareTo(r2.distance));

                for (int i = 0; i < m_raycastHits.Count; i++)
                {
                    var hit = m_raycastHits[i];
                    GameObject hitTarget = null;

                    // Attempt to find a plane or reference point that was hit
                    if (m_PlaneManger != null)
                    {
                        ARPlane arPlane = m_PlaneManger.GetPlane(hit.trackableId);
                        if (arPlane != null) hitTarget = arPlane.gameObject;
                    }

                    if(m_referencePointManager!=null && hitTarget!=null )
                    {
                        ARReferencePoint referencePoint = m_referencePointManager.GetReferencePoint(hit.trackableId);
                        if (referencePoint != null) hitTarget = referencePoint.gameObject;
                    }

                    Vector3 worldPosition = m_SessionOrigin.transform.InverseTransformPoint(hit.pose.position);
                    Vector3 worldUp = m_SessionOrigin.transform.TransformDirection(hit.pose.up);

                    resultAppendList.Add(
                        new RaycastResult()
                        {
                            gameObject = hitTarget, // Send messages to the GameObject which has the trackable
                            module = this,
                            distance = hit.distance,
                            worldPosition = worldPosition,
                            worldNormal = worldUp,
                            screenPosition = eventData.position,
                            index = i,
                            sortingLayer = 0,
                            sortingOrder = m_SortingOrder
                        }
                    );
                }
            }
        }
    }
}
