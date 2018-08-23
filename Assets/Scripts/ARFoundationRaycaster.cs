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

        private List<ARRaycastHit> m_raycastHits = new List<ARRaycastHit>();

        public override Camera eventCamera
        {
            get { return arSessionOrigin.camera; }
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
        public TrackableId GetTrackableId( int index)
        {
            if (index < m_raycastHits.Count) return m_raycastHits[index].trackableId;
            return TrackableId.InvalidId;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (arSessionOrigin.Raycast(eventData.position, m_raycastHits, trackableTypeMask))
            {
                m_raycastHits.Sort((r1, r2) => r1.distance.CompareTo(r2.distance));

                for (int i = 0; i < m_raycastHits.Count; i++)
                {
                    var hit = m_raycastHits[i];
                    resultAppendList.Add(
                        new RaycastResult()
                        {
                            gameObject = gameObject, // TODO Generic find trackables?
                            module = this,
                            distance = hit.distance,
                            worldPosition = hit.pose.position,
                            worldNormal = hit.pose.up,
                            screenPosition = eventData.position,
                            index = i,
                            sortingLayer = 0,
                            sortingOrder = 0
                        }
                    );
                }
            }
        }
    }
}
