using System;

namespace UnityEngine.XR.ARSim
{
    public static class DepthApi
    {
        public static void SetDepthData(Vector3[] positions, float[] confidences)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");

            if (confidences != null && positions.Length != confidences.Length)
                throw new ArgumentException("confidences must be the same length as positions");

            NativeApi.UnityARSim_setDepthData(positions, confidences, positions.Length);
        }
    }
}
