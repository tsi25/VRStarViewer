﻿using UnityEngine;

namespace HFN.UniverseVR
{
    public class EngineAudio : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float offset = 0f;

        public float cutoffOn = 800;
        public float cutoffOff = 100;

        public bool engineOn = false;


        System.Random rand = new System.Random();
        AudioLowPassFilter lowPassFilter;

        void Awake()
        {
            lowPassFilter = GetComponent<AudioLowPassFilter>();
            Update();
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)(rand.NextDouble() * 2.0 - 1.0 + offset);
            }
        }

        void Update()
        {
            //lowPassFilter.cutoffFrequency = engineOn ? cutoffOn : cutoffOff;
        }
    }
}
