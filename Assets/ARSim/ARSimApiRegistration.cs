using AOT;
using System;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARSim
{
    internal class ARSimApiRegistration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            NativeApi.UnityARSim_setTrackableIdGenerator(GenerateIdFromGuid);
        }

        [MonoPInvokeCallback(typeof(Func<TrackableId>))]
        static TrackableId GenerateIdFromGuid()
        {
            return NativeApi.UnityARSim_createTrackableId(Guid.NewGuid());
        }
    }
}
