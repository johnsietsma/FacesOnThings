using System;
using System.Collections.Generic;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Renders an <see cref="ARPointCloud"/> as a <c>ParticleSystem</c>. Differs from ARPointCloudParticleVisualizer in that it Emits() particles to they keep their module data.
    /// </summary>
    [RequireComponent(typeof(ARPointCloud))]
    [RequireComponent(typeof(ParticleSystem))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/api/UnityEngine.XR.ARFoundation.ARPointCloudParticleVisualizer.html")]
    public sealed class ARPointCloudParticleEmitVisualizer : MonoBehaviour
    {
        void OnPointCloudChanged(ARPointCloud pointCloud)
        {
            var points = s_Vertices;
            pointCloud.GetPoints(points, Space.Self);

            int numPoints = points.Count;

            // Get the number of existing particles
            int numParticles = m_ParticleSystem.GetParticles(m_Particles);

            // Count how many particles are alive
            int numAliveParticles = 0;
            for( int i=0; i< numParticles; i++)
                if( m_Particles[i].remainingLifetime > 0 ) numAliveParticles++;

            // Emit enough particles to match the number of points
            if (numAliveParticles < numPoints) 
            {
                var emitCount = numPoints-numAliveParticles;
                m_ParticleSystem.Emit(emitCount);
            }

            // Resize the particle array if there's not enough room
            if(m_Particles.Length < numPoints)
                m_Particles = new ParticleSystem.Particle[numPoints];

            // Get the particles again, so we have the newly emitted particles
            numParticles = m_ParticleSystem.GetParticles(m_Particles);

            // Update the positions of all the particles to match the points
            for (int i = 0; i < numParticles && i < numPoints; ++i)
            {
                m_Particles[i].position = points[i];
            }

            // Remove any existing particles by setting remainingLifetime
            // to a negative value.
            for (int i = numPoints; i < numParticles; ++i)
            {
                m_Particles[i].remainingLifetime = -1f;
            }

            m_ParticleSystem.SetParticles(m_Particles, numParticles);
        }

        void Awake()
        {
            m_PointCloud = GetComponent<ARPointCloud>();
            m_ParticleSystem = GetComponent<ParticleSystem>();
        }

        void OnEnable()
        {
            m_PointCloud.updated += OnPointCloudChanged;
            ARSubsystemManager.systemStateChanged += OnSystemStateChanged;
            UpdateVisibility();
        }

        void OnDisable()
        {
            m_PointCloud.updated -= OnPointCloudChanged;
            ARSubsystemManager.systemStateChanged -= OnSystemStateChanged;
            UpdateVisibility();
        }

        void OnSystemStateChanged(ARSystemStateChangedEventArgs eventArgs)
        {
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            var visible = enabled &&
                (ARSubsystemManager.systemState == ARSystemState.SessionTracking);

            SetVisible(visible);
        }

        void SetVisible(bool visible)
        {
            if (m_ParticleSystem == null)
                return;

            var renderer = m_ParticleSystem.GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = visible;
        }

        // Give the particle buffer a decent size to avoid reallocation
        const int k_StartParticleBufferCount = 1024;

        ARPointCloud m_PointCloud;

        ParticleSystem m_ParticleSystem;

        ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[k_StartParticleBufferCount];

        static List<Vector3> s_Vertices = new List<Vector3>();
    }
}
