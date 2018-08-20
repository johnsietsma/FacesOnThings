using System;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARSim
{    
    internal static class NativeApi
    {
        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setTrackingState(
            TrackingState trackingState);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setPlaneData(
            TrackableId planeId, Pose pose, Vector3 center, Vector2 bounds,
            Vector3[] boundaryPoints, int numPoints);

        [DllImport("UnityARSim")]
        public static extern TrackableId UnityARSim_createTrackableId(Guid guid);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_removePlane(TrackableId planeId);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setDepthData(
            Vector3[] positions, float[] confidences, int count);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setCameraTransform(
            Pose pose, Matrix4x4 transform);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setProjectionMatrix(
            Matrix4x4 projectionMatrix, Matrix4x4 inverseProjectionMatrix);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setLightEstimation(
            float averageBrightness, float colorTemperature);

        [DllImport("UnityARSim")]
        public static extern void UnityARSim_setTrackableIdGenerator(
            Func<TrackableId> generator);

        [DllImport("UnityARSim")]
        public static extern TrackableId UnityARSim_attachReferencePoint(
            TrackableId trackableId, Pose pose);
    }
}
