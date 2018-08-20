using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARExtensions;

namespace UnityEngine.XR.ARSim
{
    internal static class ARSimCameraExtensions
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            XRCameraExtensions.RegisterIsPermissionGrantedHandler(k_SubsystemId, IsPermissionGranted);
            XRCameraExtensions.RegisterTryGetColorCorrectionHandler(k_SubsystemId, TryGetColorCorrection);
        }

        static bool IsPermissionGranted(XRCameraSubsystem cameraSubsystem)
        {
            return CameraApi.permissionGranted;
        }

        static bool TryGetColorCorrection(XRCameraSubsystem cameraSubsystem, out Color color)
        {
            if (CameraApi.colorCorrection.HasValue)
            {
                color = CameraApi.colorCorrection.Value;
                return true;
            }

            color = default(Color);
            return false;
        }

        static readonly string k_SubsystemId = "ARSim-Camera";
    }
}
