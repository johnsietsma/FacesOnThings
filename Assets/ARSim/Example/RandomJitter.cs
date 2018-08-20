using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.XR.ARSimExample
{
    public class RandomJitter : MonoBehaviour
    {
        [SerializeField]
        float m_Probability = .1f;

        [SerializeField]
        float m_JitterMultiplier = .1f;

        // Update is called once per frame
        void Update()
        {
            if (Random.value < m_Probability)
                transform.localPosition += Random.insideUnitSphere * m_JitterMultiplier;
        }
    }
}
