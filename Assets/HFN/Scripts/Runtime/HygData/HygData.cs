using System;
using System.Collections.Generic;
using UnityEngine;
using HFN.Common;

namespace HFN.UniverseVR
{
    /// <summary>
    /// serialized object object holding data regarding a particular star in the simulation
    /// </summary>
    [Serializable]
	public class HygData
	{
        private const string KEY_ID = "id";
        private const string KEY_HIP = "hip";
        private const string KEY_HD = "hd";
        private const string KEY_HR = "hr";
        private const string KEY_GL = "gl";
        private const string KEY_BF = "bf";
        private const string KEY_PROPER = "proper";
        private const string KEY_RA = "ra";
        private const string KEY_DEC = "dec";
        private const string KEY_DIST = "dist";
        private const string KEY_PMRA = "pmra";
        private const string KEY_PMDEC = "pmdec";
        private const string KEY_RV = "rv";
        private const string KEY_MAG = "mag";
        private const string KEY_ABSMAG = "absmag";
        private const string KEY_SPECT = "spect";
        private const string KEY_CI = "ci";
        private const string KEY_X = "x";
        private const string KEY_Y = "y";
        private const string KEY_Z = "z";
        private const string KEY_VX = "vx";
        private const string KEY_VY = "vy";
        private const string KEY_VZ = "vz";
        private const string KEY_RARAD = "rarad";
        private const string KEY_DECRAD = "decrad";
        private const string KEY_PMRARAD = "pmrarad";
        private const string KEY_PMDECRAD = "pmdecrad";
        private const string KEY_BAYER = "bayer";
        private const string KEY_FLAM = "flam";
        private const string KEY_CON = "con";
        private const string KEY_COMP = "comp";
        private const string KEY_COMP_PRIMARY = "comp_primary";
        private const string KEY_BASE = "base";
        private const string KEY_LUM = "lum";
        private const string KEY_VAR = "var";
        private const string KEY_VAR_MIN = "var_min";
        private const string KEY_VAR_MAX = "var_max";

        [SerializeField]
        private string properName = "";
        [SerializeField]
        private float ci = 0f;
        [SerializeField]
        private float x = 0f;
        [SerializeField]
        private float y = 0f;
        [SerializeField]
        private float z = 0f;
        [SerializeField]
        private float vx = 0f;
        [SerializeField]
        private float vy = 0f;
        [SerializeField]
        private float vz = 0f;
        [SerializeField]
        private float dist = 0f;
        [SerializeField]
        private bool isValid = false;


        public string ProperName
        {
            get { return properName; }
            private set { properName = value; }
        }

        public float CI
        {
            get { return ci; }
            private set { ci = value; }
        }

        public float X
        {
            get { return x; }
            private set { x = value; }
        }

        public float Y
        {
            get { return y; }
            private set { y = value; }
        }

        public float Z
        {
            get { return z; }
            private set { z = value; }
        }

        public float VX
        {
            get { return vx; }
            private set { vx = value; }
        }

        public float VY
        {
            get { return vy; }
            private set { vy = value; }
        }

        public float VZ
        {
            get { return vz; }
            private set { vz = value; }
        }

        public float Dist
        {
            get { return dist; }
            private set { dist = value; }
        }

        public bool IsValid
        {
            get { return isValid; }
            private set { isValid = value; }
        }

        public HygData() { }

        public HygData(Dictionary<string,string> row)
        {
            FromCSV(row);
        }


        /// <summary>
        /// Handles parsing information from the csv into sanitized rows in a dictionary
        /// </summary>
        /// <param name="row"></param>
        public void FromCSV(Dictionary<string, string> row)
        {
            IsValid = true;

            if (row.ContainsKey(KEY_X))
            {
                X = Convert.ToSingle(row[KEY_X]);
            }
            else
            {
                IsValid = false;
            }

            if (row.ContainsKey(KEY_Y))
            {
                Y = Convert.ToSingle(row[KEY_Y]);
            }
            else
            {
                IsValid = false;
            }

            if (row.ContainsKey(KEY_Z))
            {
                Z = Convert.ToSingle(row[KEY_Z]);
            }
            else
            {
                IsValid = false;
            }

            if (row.ContainsKey(KEY_VX))
                VX = Convert.ToSingle(row[KEY_VX]);

            if (row.ContainsKey(KEY_VY))
                VY = Convert.ToSingle(row[KEY_VY]);

            if (row.ContainsKey(KEY_VZ))
                VZ = Convert.ToSingle(row[KEY_VZ]);

            if ((row.ContainsKey(KEY_CI)) && !row[KEY_CI].IsNullOrEmpty())
            {
                CI = Convert.ToSingle(row[KEY_CI]);
            }

            if (row.ContainsKey(KEY_DIST))
            {
                Dist = Convert.ToSingle(row[KEY_DIST]);
            }
            else
            {
                IsValid = false;
            }

            if(row.ContainsKey(KEY_PROPER) && !string.IsNullOrEmpty(row[KEY_PROPER]))
            {
                ProperName = row[KEY_PROPER];
            }
            else if(row.ContainsKey(KEY_BF) && !string.IsNullOrEmpty(row[KEY_BF]))
            {
                ProperName = row[KEY_BF];
            }
            else if (row.ContainsKey(KEY_GL) && !string.IsNullOrEmpty(row[KEY_GL]))
            {
                ProperName = row[KEY_GL];
            }
            else if (row.ContainsKey(KEY_HR) && !string.IsNullOrEmpty(row[KEY_HR]))
            {
                ProperName = row[KEY_HR];
            }
            else if (row.ContainsKey(KEY_HD) && !string.IsNullOrEmpty(row[KEY_HD]))
            {
                ProperName = row[KEY_HD];
            }
            else
            {
                ProperName = "Unnamed";
                IsValid = false;
            }

            if (Dist == 10000000)
            {
                //A value of 10000000 indicates missing or dubious (e.g., negative) parallax data in Hipparcos.
                IsValid = false;
            }
        }
    }
}