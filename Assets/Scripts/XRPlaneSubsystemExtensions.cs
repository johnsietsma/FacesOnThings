using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;


public static class XRPlaneSubsystemExtensions {

    private static List<BoundedPlane> s_BoundedPlanes = new List<BoundedPlane>();

    public static BoundedPlane GetPlane( this XRPlaneSubsystem planeSubsystem, TrackableId trackableId )
    {
        planeSubsystem.GetAllPlanes(s_BoundedPlanes);
        foreach( var plane in s_BoundedPlanes )
        {
            if (plane.Id == trackableId) return plane;
        }
        return new BoundedPlane();
    }
}
