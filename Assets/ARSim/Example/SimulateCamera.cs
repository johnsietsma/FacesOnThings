using System.Collections;
using UnityEngine.XR.ARSim;

namespace UnityEngine.XR.ARSimExample
{
    public class SimulateCamera : MonoBehaviour
    {
        [SerializeField]
        Camera m_ARCamera;

        [SerializeField]
        float m_MoveSpeed = 1f;

        [SerializeField]
        float m_TurnSpeed = 5f;

        Vector3 m_Position;

        Vector3 m_Euler;

        Vector3 m_PreviousMousePosition;

        void Update()
        {
            CameraApi.projectionMatrix = m_ARCamera.projectionMatrix;

            if (Input.GetMouseButtonDown(1))
                m_PreviousMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                var delta = (Input.mousePosition - m_PreviousMousePosition) * Time.deltaTime * m_TurnSpeed;
                m_Euler.x -= delta.y;
                m_Euler.y += delta.x;
                m_PreviousMousePosition = Input.mousePosition;
            }

            var deltaPosition = Time.deltaTime * m_MoveSpeed;

            var rotation = Quaternion.Euler(m_Euler);

            if (Input.GetKey(KeyCode.W))
                m_Position += (rotation * Vector3.forward) * deltaPosition;

            if (Input.GetKey(KeyCode.S))
                m_Position += (rotation * Vector3.back) * deltaPosition;

            if (Input.GetKey(KeyCode.A))
                m_Position += (rotation * Vector3.left) * deltaPosition;

            if (Input.GetKey(KeyCode.D))
                m_Position += (rotation * Vector3.right) * deltaPosition;

            if (Input.GetKey(KeyCode.Q))
                m_Position += (rotation * Vector3.down) * deltaPosition;

            if (Input.GetKey(KeyCode.E))
                m_Position += (rotation * Vector3.up) * deltaPosition;

            CameraApi.pose = new Pose(m_Position, rotation);
        }
    }
}
