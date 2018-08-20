using UnityEngine.XR.ARSim;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARSimExample
{
    public class SimulateSession : MonoBehaviour
    {
        [SerializeField]
        TrackingState m_TrackingState = TrackingState.Tracking;

        void Update()
        {
            SessionApi.SetTrackingState(m_TrackingState);
        }
    }
}
