using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneMeshVisualizer), typeof(MeshRenderer), typeof(ARPlane))]
public class ARPlaneMeshBorders : MonoBehaviour
{
    float borderWidth
    {
        get { return m_BorderWidth; }
        set { m_BorderWidth = value; }
    }

    [Tooltip("The width of the border (in world units).")]
    [SerializeField]
    float m_BorderWidth = 0.2f;

    [Tooltip("The amount to scale the border lengthwise.")]
    [SerializeField]
    float m_BorderLengthScale = 1.15f;

    ARPlaneMeshVisualizer m_PlaneMeshVisualizer;
    Transform m_PlaneTransform;

    void Awake()
    {
        m_PlaneMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();
        m_PlaneTransform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        m_PlaneMeshVisualizer.meshUpdated += ARPlaneMeshVisualizer_meshUpdated;
    }

    void OnDisable()
    {
        m_PlaneMeshVisualizer.meshUpdated -= ARPlaneMeshVisualizer_meshUpdated;
    }

    void ARPlaneMeshVisualizer_meshUpdated(ARPlaneMeshVisualizer planeMeshVisualizer)
    {
        GenerateBoundaryMeshes(planeMeshVisualizer.mesh);
    }

    void GenerateBoundaryMeshes(Mesh mesh)
    {
        int vertexCount = mesh.vertexCount;

        mesh.GetVertices(s_Vertices);
        mesh.GetUVs(0, s_standardUVs);
        mesh.GetUVs(0, s_borderLengthUVs);

        // Add 4 verts per border, -1 for the center vertex
        int borderVertexCount = (vertexCount - 1) * 4;
        int totalVertexCount = vertexCount + borderVertexCount;

        // Adding extra verts for the borders, make sure we have engouh rooom
        if (s_Vertices.Count < totalVertexCount) s_Vertices.Capacity = totalVertexCount;
        if (s_standardUVs.Count < totalVertexCount) s_standardUVs.Capacity = totalVertexCount;
        if (s_borderLengthUVs.Count < totalVertexCount) s_borderLengthUVs.Capacity = totalVertexCount;

        int borderVertIndex = vertexCount;
        float halfBorderWidth = borderWidth / 2;

        for (int vertIndex = 0; vertIndex < vertexCount - 1; vertIndex++)
        {
            int nextVertIndex = (vertIndex + 1) % (vertexCount - 1);
            Vector3 borderVert1 = m_PlaneTransform.TransformPoint(s_Vertices[vertIndex]);
            Vector3 borderVert2 = m_PlaneTransform.TransformPoint(s_Vertices[nextVertIndex]);
            Vector3 borderDirection = (borderVert2 - borderVert1).normalized;
            float borderLength = (borderVert2 - borderVert1).magnitude;

            // The border offset is a vector perpendicular to the border.
            Vector3 borderOffset = Vector3.Cross(m_PlaneTransform.up, borderDirection);
            borderOffset *= halfBorderWidth;

            // Figure out how much to extend the border in each direction
            Vector3 borderExtension = borderDirection * (0.5f * (borderLength * m_BorderLengthScale-borderLength));

            // Move the border off the plane, and add height for each sets of 4 borders to avoid z-fighting
            // There can be a lot of borders close togetherand overlapping. So a set of 5 is a good group size.
            const float zFightEpsilon = 0.0001f;
            Vector3 zFightOffset = m_PlaneTransform.up * (zFightEpsilon + (vertIndex % 5) * zFightEpsilon);

            Vector3 v1 = borderVert1 + borderOffset + zFightOffset - borderExtension;
            Vector3 v2 = borderVert2 + borderOffset + zFightOffset + borderExtension;
            Vector3 v3 = borderVert1 - borderOffset + zFightOffset - borderExtension;
            Vector3 v4 = borderVert2 - borderOffset + zFightOffset + borderExtension;

            v1 = m_PlaneTransform.InverseTransformPoint(v1);
            v2 = m_PlaneTransform.InverseTransformPoint(v2);
            v3 = m_PlaneTransform.InverseTransformPoint(v3);
            v4 = m_PlaneTransform.InverseTransformPoint(v4);

            s_Vertices.Add(v1);
            s_Vertices.Add(v2);
            s_Vertices.Add(v3);
            s_Vertices.Add(v4);

            s_standardUVs.AddRange(kStandardUVs);

            Vector2[] borderLengthUVs = new Vector2[] {
                new Vector2(0,1),
                new Vector2(borderLength,1),
                new Vector2(0,0),
                new Vector2(borderLength,0),
            };

            s_borderLengthUVs.AddRange(borderLengthUVs);

            borderVertIndex += 4;
        }

        mesh.SetVertices(s_Vertices);
        mesh.SetUVs(0, s_standardUVs);
        mesh.SetUVs(1, s_borderLengthUVs);

        int borderIndexCount = (vertexCount - 1) * 6;
        if (s_BorderIndices.Count < borderIndexCount) s_BorderIndices.Capacity = borderIndexCount;
        s_BorderIndices.Clear();

        borderVertIndex = vertexCount;
        for (int vertIndex = 0; vertIndex < vertexCount - 1; vertIndex++)
        {
            s_BorderIndices.Add(borderVertIndex + 0);
            s_BorderIndices.Add(borderVertIndex + 2);
            s_BorderIndices.Add(borderVertIndex + 1);
            s_BorderIndices.Add(borderVertIndex + 1);
            s_BorderIndices.Add(borderVertIndex + 2);
            s_BorderIndices.Add(borderVertIndex + 3);
            borderVertIndex += 4;
        }

        mesh.subMeshCount = 2;
        mesh.SetTriangles(s_BorderIndices, 1, false);
        mesh.UploadMeshData(false);
    }

    static readonly Vector2[] kStandardUVs = new Vector2[] {
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(0,0),
            new Vector2(1,0),
        };


    static List<Vector3> s_Vertices = new List<Vector3>();
    static List<Vector2> s_standardUVs = new List<Vector2>();
    static List<Vector2> s_borderLengthUVs = new List<Vector2>();
    static List<int> s_BorderIndices = new List<int>();
}
