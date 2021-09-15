using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFN.UniverseVR
{
    /// <summary>
    /// Handles the creation, initialization, and lookup of stars in the simulation
    /// </summary>
    public class StarBuilder : MonoBehaviour
    {
        /// <summary>
        /// Particle system responsible for spawning and tracking stars
        /// </summary>
        [SerializeField, Tooltip("Particle system responsible for spawning and tracking stars")]
        private ParticleSystem particles = null;
        /// <summary>
        /// Data collection from which stars will be spawned
        /// </summary>
        [SerializeField, Tooltip("Data collection from which stars will be spawned")]
        private HygDataCollection hygDataCollection = null;
        /// <summary>
        /// Objects which handles displaying information about individual stars in focus
        /// </summary>
        [SerializeField, Tooltip("Objects which handles displaying information about individual stars in focus")]
        private HygDataDisplay hygDisplay = null;
        /// <summary>
        /// Line renderer prefab for drawing constellations
        /// </summary>
        [SerializeField, Tooltip("Line renderer prefab for drawing constellations")]
        private LineRenderer linePrefab = null;

        [Header("Movement Config")]
        /// <summary>
        /// Whether or not stars should move in the simulation according to their recorded linear velocity NOTE: labels will not display on moving stars
        /// </summary>
        [SerializeField, Tooltip("Whether or not stars should move in the simulation according to their recorded linear velocity NOTE: labels will not display on moving stars")]
        private bool showMovement = false;
        /// <summary>
        /// Movement speed multiplier to make the movement of the stars more apparent
        /// </summary>
        [SerializeField, Tooltip("Movement speed multiplier to make the movement of the stars more apparent")]
        private float speedMultiplier = 500f;

        /// <summary>
        /// Dictionary of particle datas correlated with their spawn positions in space for faster lookup
        /// </summary>
        private Dictionary<Vector3, HygData> particleData = new Dictionary<Vector3, HygData>();
        /// <summary>
        /// Recorded ongoing particle enter events
        /// </summary>
        private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        /// <summary>
        /// Recorded ongoing particle exit events
        /// </summary>
        private List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        /// <summary>
        /// amount of time since the beginning of the simulation
        /// </summary>
        private float elapsedTime = 0f;

        /// <summary>
        /// Particle system responsible for spawning and tracking stars
        /// </summary>
        public ParticleSystem Particles
        {
            get { return particles; }
        }


        /// <summary>
        /// Gets the current position and velocity of a particle, and returns the HygData that it correlates to
        /// NOTE: Does not currently work for stars in motion
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public HygData GetDataFromParticle(Vector3 currentPosition, Vector3 velocity)
        {
            Vector3 startingPosition = currentPosition;
            Vector3 distanceTraveled = velocity * elapsedTime;

            startingPosition.x -= distanceTraveled.x;
            startingPosition.y -= distanceTraveled.y;
            startingPosition.z -= distanceTraveled.z;

            return particleData[currentPosition];
        }


        /// <summary>
        /// Takes a HygData and instantiates a particle based on the information within that data
        /// </summary>
        /// <param name="hygData"></param>
        /// <param name="debug"></param>
        private void CreateStar(HygData hygData, bool debug = false)
        {
            //generate position and velocity
            Vector3 position = new Vector3(hygData.X, hygData.Y, hygData.Z);
            Vector3 velocity = Vector3.zero;

            if(showMovement)
            {
                velocity = new Vector3(hygData.VX, hygData.VY, hygData.VZ);
                velocity *= speedMultiplier;
            }

            //initialize the start color
            Color starColor = Color.white;
            if (hygData.CI != 0f) starColor = CIToColor(hygData.CI);

            //skip caching the star data if the position overlaps with another star
            if(!particleData.ContainsKey(position)) particleData.Add(position, hygData);

            //initialize emission parameter
            ParticleSystem.EmitParams emissionParameters = new ParticleSystem.EmitParams();

            emissionParameters.position = position;
            emissionParameters.velocity = velocity;
            emissionParameters.startSize = 0.1f;//TODO base this on size data if available
            emissionParameters.startLifetime = Mathf.Infinity;
            emissionParameters.startColor = starColor;

            //emit particle
            particles.Emit(emissionParameters, 1);
        }


        /// <summary>
        /// Iterates through the given ConstellationData and sets up visible constellations
        /// </summary>
        /// <param name="constellation"></param>
        private void CreateConstellation(ConstellationData constellation)
        {
            for(int i = 0; i < constellation.Pairs.Count; i++)
            {
                LineRenderer lineRenderer = Instantiate(linePrefab);
                Vector3[] positions = new Vector3[]
                {
                    constellation.Pairs[i].starCoordinates1,
                    constellation.Pairs[i].starCoordinates2
                };
                lineRenderer.SetPositions(positions);
            }
        }


        /// <summary>
        /// Takes the CI(color index) of a star, calculates temperature, and converts that to an RGB value
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        private Color CIToColor(float ci)
        {
            //TODO: find a cleaner way to do this

            Color color = Color.white;
            float temperature = 0f;
            float r = 0f;
            float g = 0f;
            float b = 0f;

            Mathf.Clamp(ci, -0.4f, 2.0f);

            //Calculate red values based on temperature
            if ((ci >= -0.40) && (ci < 0.00))
            {
                temperature = (ci + 0.40f) / (0.00f + 0.40f);
                r = 0.61f + (0.11f * temperature) + (0.1f * temperature * temperature);
            }
            else if ((ci >= 0.00f) && (ci < 0.40f))
            {
                temperature = (ci - 0.00f) / (0.40f - 0.00f);
                r = 0.83f + (0.17f * temperature);
            }
            else if ((ci >= 0.40) && (ci < 2.10))
            {
                temperature = (ci - 0.40f) / (2.10f - 0.40f);
                r = 1.00f;
            }

            //Calculate green values based on temperature
            if ((ci >= -0.40f) && (ci < 0.00f))
            {
                temperature = (ci + 0.40f) / (0.00f + 0.40f);
                g = 0.70f + (0.07f * temperature) + (0.1f * temperature * temperature);
            }
            else if ((ci >= 0.00f) && (ci < 0.40f))
            {
                temperature = (ci - 0.00f) / (0.40f - 0.00f);
                g = 0.87f + (0.11f * temperature);
            }
            else if ((ci >= 0.40f) && (ci < 1.60f))
            {
                temperature = (ci - 0.40f) / (1.60f - 0.40f);
                g = 0.98f - (0.16f * temperature);
            }
            else if ((ci >= 1.60f) && (ci < 2.00f))
            {
                temperature = (ci - 1.60f) / (2.00f - 1.60f);
                g = 0.82f - (0.5f * temperature * temperature);
            }

            //Calculate blue values based on temperature
            if ((ci >= -0.40f) && (ci < 0.40f))
            {
                temperature = (ci + 0.40f) / (0.40f + 0.40f);
                b = 1.00f;
            }
            else if ((ci >= 0.40f) && (ci < 1.50f))
            {
                temperature = (ci - 0.40f) / (1.50f - 0.40f);
                b = 1.00f - (0.47f * temperature) + (0.1f * temperature * temperature);
            }
            else if ((ci >= 1.50f) && (ci < 1.94f))
            {
                temperature = (ci - 1.50f) / (1.94f - 1.50f);
                b = 0.63f - (0.6f * temperature * temperature);
            }

            color.r = r * 255f;
            color.g = g * 255f;
            color.b = b * 255f;

            return color;
        }


        /// <summary>
        /// Handles displaying and hiding the HygDisplay based on particle collision triggers
        /// </summary>
        private void OnParticleTrigger()
        {
            int numEnter = Particles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            int numExit = Particles.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

            for (int i = 0; i < numEnter; i++)
            {
                HygData data = GetDataFromParticle(enter[i].position, enter[i].velocity);
                hygDisplay.Initialize(enter[i].position, data);
                Debug.Log("on enter");
            }

            for (int i = 0; i < numExit; i++)
            {
                hygDisplay.FadeOut();
                Debug.Log("on exit");
            }
        }


        private void Update()
        {
            //iterate elapsed time
            elapsedTime += Time.deltaTime;
        }


        private void Start()
        {
            //Initialzie stars from hyg data collection
            for(int i = 0; i < hygDataCollection.HygData.Count; i++)
            {
                CreateStar(hygDataCollection.HygData[i], i<25);
            }

            //Initialize constellations from constellation data
            for(int i = 0; i < hygDataCollection.ConstellationData.Count; i++)
            {
                CreateConstellation(hygDataCollection.ConstellationData[i]);
            }
        }
    }
}