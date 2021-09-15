using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFN.UniverseVR
{
    /// <summary>
    /// Scriptable object holding data regarding the positioning of a particular constellation in the simulation
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ConstellationData), menuName = "HFN/UniverseVR/Data/" + nameof(ConstellationData))]
    public class ConstellationData : ScriptableObject
    {
        /// <summary>
        /// The given identifiable name of the constellation
        /// </summary>
        [SerializeField, Tooltip("The given identifiable name of the constellation")]
        private string constellationName = "";
        /// <summary>
        /// List of linked stars comprising this constellation
        /// </summary>
        [SerializeField, Tooltip("List of linked stars comprising this constellation")]
        private List<ConstellationPair> pairs = new List<ConstellationPair>();


        /// <summary>
        /// List of linked stars comprising this constellation
        /// </summary>
        public List<ConstellationPair> Pairs
        {
            get { return pairs; }
        }


        /// <summary>
        /// Attempts to locate a star within the database to grab its cartesian coordinates for later initialization
        /// </summary>
        /// <param name="hygData"></param>
        public void TrySetData(HygData hygData)
        {
            for(int i = 0; i < pairs.Count; i++)
            {
                if(pairs[i].starName1 == hygData.ProperName)
                {
                    pairs[i].starCoordinates1 = new Vector3(hygData.X, hygData.Y, hygData.Z);
                }

                if (pairs[i].starName2 == hygData.ProperName)
                {
                    pairs[i].starCoordinates2 = new Vector3(hygData.X, hygData.Y, hygData.Z);
                }
            }
        }
    }
}