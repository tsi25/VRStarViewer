using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using WebXR;

namespace HFN.UniverseVR
{
    /// <summary>
    /// Handles moving the player through space
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {
        /// <summary>
        /// Transform of the player's head
        /// </summary>
        [SerializeField, Tooltip("Transform of the player's head")]
        protected Transform head = null;
        /// <summary>
        /// Transform of the player's left hand
        /// </summary>
        [SerializeField, Tooltip("Transform of the player's left hand")]
        protected NVRHand leftHand = null;
        /// <summary>
        /// Transform of the player's right hand
        /// </summary>
        [SerializeField, Tooltip("Transform of the player's right hand")]
        protected NVRHand rightHand = null;

        /// <summary>
        /// Player's movement speed
        /// </summary>
        [SerializeField, Tooltip("Player's movement speed")]
        protected float speed = 2f;


        private void Update()
        {
#if !UNITY_WEBGL
            if (rightHand.CurrentHandState == HandState.GripDownInteracting || rightHand.CurrentHandState == HandState.GripDownNotInteracting)
            {
                //Right hand is gripping, move the player through space in the direction of the right hand
                transform.Translate((rightHand.transform.localPosition - head.localPosition) * speed * Time.deltaTime);
            }
            else if(leftHand.CurrentHandState == HandState.GripDownInteracting || leftHand.CurrentHandState == HandState.GripDownNotInteracting)
            {
                //Left hand is gripping, move the player through space in the direction of the left hand
                transform.Translate((leftHand.transform.localPosition - head.localPosition) * speed * Time.deltaTime);
            }
#endif
        }
    }
}