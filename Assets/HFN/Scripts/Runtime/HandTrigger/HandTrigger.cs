using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFN.UniverseVR
{
    public class HandTrigger : MonoBehaviour
    {
        /// <summary>
        /// Reference to the star builder present in the simulation
        /// </summary>
        [SerializeField, Tooltip("Reference to the star builder present in the simulation")]
        private StarBuilder starBuilder = null;

        private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        private List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        void OnParticleTrigger()
        {
            int numEnter = starBuilder.Particles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            int numExit = starBuilder.Particles.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

            for(int i = 0; i < numEnter; i++)
            {
                Debug.Log(starBuilder.GetDataFromParticle(enter[i].position, enter[i].velocity).ProperName + " has been hovered");
            }

            for (int i = 0; i < numExit; i++)
            {
                Debug.Log(starBuilder.GetDataFromParticle(exit[i].position, exit[i].velocity).ProperName + " has been exited");
            }
        }
    }
}

