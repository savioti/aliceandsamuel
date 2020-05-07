using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* *****************************************************************************
 * File:    UnityExtensionMethods.cs
 * Authors:  Ângelo Savioti with contributions from Philip Pierce
 * Description: Extensions for Unity Classes
 * ****************************************************************************/

namespace MSavioti.UnityExtensionMethods
{
    public static class UnityExtensionMethods
    {
        #region Audio Source 
        public static void ForcePlay(this AudioSource a, float volume = 1f)
        {
            if (!a) return;
            a.volume = volume;
            
            if (a.isPlaying)
                a.Stop();

            a.Play();
        }
        public static void ForcePlay(this AudioSource a, AudioClip clip, float volume = 1f)
        {
            if (!a) return;
            a.clip = clip;
            a.volume = volume;

            if (a.isPlaying)
                a.Stop();

            a.Play();
        }
        #endregion

        #region Particle System Extensions
        public static void ForcePlay(this ParticleSystem p)
        {
            if (!p) return;

            if (p.isPlaying)
                p.Stop();

            p.Play();
        }
        #endregion

        #region Camera Extensions
        public static Vector3 GetWorldPositionOnPlane(this Camera cam, Vector3 screenPosition, float planeZ) {
            Ray ray = cam.ScreenPointToRay(screenPosition);
            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, planeZ));
            plane.Raycast(ray, out float distance);
            return ray.GetPoint(distance);
        }
        #endregion

        #region Game Object Extensions
        public static bool IsInLayerMask(this GameObject obj, LayerMask mask) 
        {
            return ((mask.value & (1 << obj.layer)) > 0);
        }
        #endregion

        #region Layer Mask Extensions
        //Author:  Philip Pierce
        public static LayerMask Create(params string[] layerNames)
        {
            return NamesToMask(layerNames);
        }
        //Author:  Philip Pierce
        public static LayerMask Create(params int[] layerNumbers)
        {
            return LayerNumbersToMask(layerNumbers);
        }
        //Author:  Philip Pierce
        public static LayerMask NamesToMask(params string[] layerNames)
        {
            LayerMask ret = (LayerMask)0;
            foreach (var name in layerNames)
            {
                ret |= (1 << LayerMask.NameToLayer(name));
            }
            return ret;
        }
        //Author:  Philip Pierce
        public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
        {
            LayerMask ret = (LayerMask)0;
            foreach (var layer in layerNumbers)
            {
                ret |= (1 << layer);
            }
            return ret;
        }
        //Author:  Philip Pierce
        public static LayerMask Inverse(this LayerMask original)
        {
            return ~original;
        }
        //Author:  Philip Pierce
        public static LayerMask AddToMask(this LayerMask original, params string[] layerNames)
        {
            return original | NamesToMask(layerNames);
        }
        //Author:  Philip Pierce
        public static LayerMask RemoveFromMask(this LayerMask original, params string[] layerNames)
        {
            LayerMask invertedOriginal = ~original;
            return ~(invertedOriginal | NamesToMask(layerNames));
        }
        //Author:  Philip Pierce
        public static string[] MaskToNames(this LayerMask original)
        {
            var output = new List<string>();

            for (int i = 0; i < 32; ++i)
            {
                int shifted = 1 << i;
                if ((original & shifted) == shifted)
                {
                    string layerName = LayerMask.LayerToName(i);
                    if (!string.IsNullOrEmpty(layerName))
                    {
                        output.Add(layerName);
                    }
                }
            }
            return output.ToArray();
        }
        //Author:  Philip Pierce
        public static string MaskToString(this LayerMask original)
        {
            return MaskToString(original, ", ");
        }
        //Author:  Philip Pierce
        public static string MaskToString(this LayerMask original, string delimiter)
        {
            return string.Join(delimiter, MaskToNames(original));
        }
        #endregion

        #region Transform Extensions
        /// <summary>
        /// Teleport the object forward in the direction it is rotated.
        /// If you rotate the object 90 degrees, it will now move forward in the direction
        /// it is now facing. This essentially translates local coordinates to 
        /// world coordinates to move object in direction and distance specified by vector.
        /// MUST be called from the Update() function
        /// NOTE: Does NOT handle collisions
        /// </summary>
        /// <param name="go"></param>
        /// <param name="speed"></param>
        //Author:  Philip Pierce
        public static void TeleportForward_NoPhysics(this Transform go, float speed)
        {
            go.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        // TeleportForward_NoPhysics
        #endregion

        #region Rigidbody Extensions

        /// <summary>
        /// Moves the object by pushing it from <paramref name="moveDirection"/>.
        /// Example. To move forward, use Vector3.Backward
        /// NOTE: Uses Physics
        /// NOTE: Must be called from FixedUpdate()
        /// </summary>
        /// <param name="go"></param>
        /// <param name="moveDirection"></param>
        /// <param name="force"></param>
        //Author:  Philip Pierce
        public static void MoveByForcePushing_WithPhysics(this Rigidbody go, Vector3 moveDirection, float force)
        {
            go.AddForce(moveDirection * force);
        }
        /// <summary>
        /// Chnages the rigidbody's velocity (Cause the object to act like it's being pushed).
        /// If not called on every update, the object will slow down due to friction.
        /// NOTE: Uses Physics
        /// NOTE: Must be called from FixedUpdate()
        /// </summary>
        /// <param name="go"></param>
        /// <param name="movementDirection"></param>
        /// <param name="speed"></param>
        //Author:  Philip Pierce
        public static void MoveByVelocity_WithPhysics(this Rigidbody go, Vector3 movementDirection, float speed)
        {
            go.velocity = movementDirection * speed;
        }
        /// <summary>
        /// Move the rigidbody's position (note this is not via the transform). 
        /// This method will push other objects out of the way
        /// NOTE: Uses Physics
        /// NOTE: Must be called from FixedUpdate()
        /// </summary>
        /// <param name="go"></param>
        /// <param name="movementDirection"></param>
        /// <param name="speed"></param>
        //Author:  Philip Pierce
        public static void MoveTowards_WithPhysics(this Rigidbody go, Vector3 movementDirection, float speed)
        {
            go.MovePosition(go.position + (movementDirection * speed * Time.deltaTime));
        }
        #endregion
        
        // #region Rigidbody2D Extensions
        // /// <summary>
        // /// Adds force to an object so it hits its target while moving in a parabola.
        // /// </summary>
        // /// <param name="initialPos"></param>
        // /// <param name="finalPos"></param>
        // public static void AddForceInParabola(this Rigidbody2D rb2d, Vector2 initialPos, Vector2 finalPos, float time)
        // {
        //     Vector2 force;
        //     force.x = MathLib.GetVelocity(initialPos.x, finalPos.x, time);
        //     force.y = MathLib.GetVelocity(initialPos.y, finalPos.y, time);
        //     force.y += 0.5f * Physics2D.gravity.y * MathLib.Square(time) / time;
        //     force.y *= -1;
        //     Debug.Log(force);
        //     rb2d.AddForce(force * rb2d.mass, ForceMode2D.Impulse);
        // }
        // #endregion
    }
}