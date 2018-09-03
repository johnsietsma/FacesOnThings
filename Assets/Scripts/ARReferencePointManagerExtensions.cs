using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;


public static class ARReferencePointManagerExtensions
{
    private static List<ARReferencePoint> s_ReferencePoints = new List<ARReferencePoint>();

    public static ARReferencePoint GetReferencePoint( this ARReferencePointManager referencePointManager, TrackableId trackableId )
    {
        referencePointManager.GetAllReferencePoints(s_ReferencePoints);
        foreach( var referencePoint in s_ReferencePoints )
        {
            if (referencePoint.sessionRelativeData.Id == trackableId) return referencePoint;
        }
        return null;
    }
}
