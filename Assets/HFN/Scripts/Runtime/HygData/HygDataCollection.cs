using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFN.Common;

namespace HFN.UniverseVR
{
    /// <summary>
    /// A collection of data regarding every star present in the simulation
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HygDataCollection), menuName = "HFN/UniverseVR/Collections/" + nameof(HygDataCollection))]
    public class HygDataCollection : ScriptableObject
	{
        /// <summary>
        /// Reference to the Hyg database
        /// </summary>
        [SerializeField, Tooltip("Reference to the Hyg database")]
        private TextAsset hygCsv = null;
        /// <summary>
        /// List of parsed HygData 
        /// </summary>
        [SerializeField, Tooltip("List of parsed hyg data")]
        private List<HygData> hygData = new List<HygData>();
        /// <summary>
        /// List of constellation data
        /// </summary>
        [SerializeField, Tooltip("List of constellation data")]
        private List<ConstellationData> constellationData = new List<ConstellationData>();

        /// <summary>
        /// List of parsed hyg data
        /// </summary>
        public List<HygData> HygData
        {
            get { return hygData; }
        }

        /// <summary>
        /// List of constellation data
        /// </summary>
        public List<ConstellationData> ConstellationData
        {
            get { return constellationData; }
        }


        /// <summary>
        /// Refreshes this object's HygData and ConstellationData based on the referenced HygDatabase csv
        /// </summary>
        [ContextMenu("Refresh")]
        private void Refresh()
        {
            hygData.Clear();

            List<Dictionary<string, string>> rows = CsvParser.Parse(hygCsv.text);

            Debug.Log("Parsing....");

            int successes = 0;
            int failures = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                HygData newHygData = new HygData(rows[i]);
                hygData.Add(newHygData);

                for (int j = 0; j < constellationData.Count; j++)
                {
                    constellationData[j].TrySetData(newHygData);
                }

                if (newHygData.IsValid)
                {
                    successes++;
                }
                else
                {
                    failures++;
                }
            }

            Debug.Log("Parsing completed with " + successes + " successes and "+failures+" failures");
        }
	}
}