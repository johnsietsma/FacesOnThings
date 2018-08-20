using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARSimExample
{
    [RequireComponent(typeof(ARSessionOrigin))]
    public class PlaceOnPlaneWithMouse : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

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
            if (!Input.GetMouseButton(0))
                return;

            if (m_SessionOrigin.Raycast(Input.mousePosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}
