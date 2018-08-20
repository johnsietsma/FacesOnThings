using System;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARSim
{
    public static class CameraApi
    {
        public static bool permissionGranted { get; set; }

        public static Pose pose
        {
            set
            {
                var transform = new Matrix4x4();
                transform.SetTRS(value.position, value.rotation, Vector3.one);
                NativeApi.UnityARSim_setCameraTransform(value, transform);
            }
        }

        public static Matrix4x4 projectionMatrix
        {
            set
            {
                NativeApi.UnityARSim_setProjectionMatrix(value, value.inverse);
            }
        }

        public static void SetLightEstimation(float averageBrightness, float colorTemperature, Color colorCorrection)
        {
            CameraApi.colorCorrection = colorCorrection;
            NativeApi.UnityARSim_setLightEstimation(averageBrightness, colorTemperature);
        }

        internal static Color? colorCorrection { get; set; }
    }
}
