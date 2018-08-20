using System;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARSim
{
    public static class SessionApi
    {
        public static void SetTrackingState(TrackingState trackingState)
        {
            NativeApi.UnityARSim_setTrackingState(trackingState);
        }
    }
}
