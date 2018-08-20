using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARSimExample
{
    [DefaultExecutionOrder(-1000)]
    public class SetSubsystemFilter : MonoBehaviour
    {
        [SerializeField]
        string m_SubsystemFilter;

        void Awake()
        {
            ARSubsystemManager.subsystemFilter = m_SubsystemFilter;
        }
    }
}
