using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HFN.UniverseVR
{
    [RequireComponent(typeof(AudioLowPassFilter))]
    public class ProceduralAudio : MonoBehaviour
    {
        private float sampling_frequency = 48000;

        [SerializeField, Range(0f, 1f)]
        private float noiseRatio = 0.5f;

        //for noise part
        [SerializeField, Range(-1f, 1f)]
        private float offset;

        private float cutoffOn = 800;
        private float cutoffOff = 100;

        private bool cutOff = false;

        //for tonal part

        private float frequency = 440f;
        private float gain = 0.05f;

        private float increment = 0f;
        private float phase = 0f;


        System.Random rand = new System.Random();
        AudioLowPassFilter lowPassFilter;


        private void Awake()
        {
            sampling_frequency = AudioSettings.outputSampleRate;

            lowPassFilter = GetComponent<AudioLowPassFilter>();
            Update();
        }


        private void OnAudioFilterRead(float[] data, int channels)
        {
            float tonalPart = 0;
            float noisePart = 0;

            // update increment in case frequency has changed
            increment = frequency * 2f * Mathf.PI / sampling_frequency;

            for (int i = 0; i < data.Length; i++)
            {

                //noise
                noisePart = noiseRatio * (float)(rand.NextDouble() * 2.0 - 1.0 + offset);

                phase = phase + increment;
                if (phase > 2 * Mathf.PI) phase = 0;


                //tone
                tonalPart = (1f - noiseRatio) * (float)(gain * Mathf.Sin(phase));


                //together
                data[i] = noisePart + tonalPart;

                // if we have stereo, we copy the mono data to each channel
                if (channels == 2)
                {
                    data[i + 1] = data[i];
                    i++;
                }
            }
        }


        void Update()
        {
            lowPassFilter.cutoffFrequency = cutOff ? cutoffOn : cutoffOff;
        }
    }
}