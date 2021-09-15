using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HFN.UniverseVR
{
    /// <summary>
    /// Handles displaying information about stars within the simulation
    /// </summary>
    public class HygDataDisplay : MonoBehaviour
    {
        /// <summary>
        /// Label used to show the name of the selected star
        /// </summary>
        [SerializeField, Tooltip("Label used to show the name of the selected star")]
        private TextMeshProUGUI nameLabel = null;
        /// <summary>
        /// Canvas group through which alpha is controlled
        /// </summary>
        [SerializeField, Tooltip("Canvas group through which alpha is controlled")]
        private CanvasGroup canvasGroup;

        /// <summary>
        /// Duration over which the fade should take place
        /// </summary>
        [SerializeField, Tooltip("Duration over which the fade should take place")]
        private float fadeDuration = 1f;
        /// <summary>
        /// Curve along which the fade should take place
        /// </summary>
        [SerializeField, Tooltip("Curve along which the fade should take place")]
        private AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        /// <summary>
        /// Cached reference to the active fade coroutine
        /// </summary>
        private Coroutine fadeCoroutine = null;


        /// <summary>
        /// Shows the relevant data for the given HygData at the given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="data"></param>
        public void Initialize(Vector3 position, HygData data)
        {
            transform.position = position;
            transform.LookAt(Camera.main.transform);
            nameLabel.text = data.ProperName;
            FadeIn();
        }


        /// <summary>
        /// Handles gradually fading the display in
        /// </summary>
        public void FadeIn()
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCoroutine(1f));
        }


        /// <summary>
        /// Handles gradually fading the display out
        /// </summary>
        public void FadeOut()
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCoroutine(0f));
        }


        /// <summary>
        /// Coroutine performing the alpha lerp
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        private IEnumerator FadeCoroutine(float to)
        {
            float elapsedTime = 0f;
            float from = canvasGroup.alpha;

            while(elapsedTime <= fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, fadeCurve.Evaluate(elapsedTime / fadeDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = to;
            fadeCoroutine = null;
        }
    }
}