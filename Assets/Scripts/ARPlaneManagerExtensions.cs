using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;


public static class ARPlaneManagerExtensions
{
    private static List<ARPlane> s_Planes = new List<ARPlane>();

    public static ARPlane GetPlane( this ARPlaneManager planeSubsystem, TrackableId trackableId )
    {
        planeSubsystem.GetAllPlanes(s_Planes);
        foreach( var plane in s_Planes )
        {
            if (plane.boundedPlane.Id == trackableId) return plane;
        }
        return null;
    }
}
