using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// Bridges Unity 6 (linearVelocity) and 2022.3 LTS (velocity) so the same source
    /// builds against both. Use Vel() and SetVel() instead of touching Rigidbody.velocity directly.
    /// </summary>
    public static class UnityCompat
    {
        public static Vector3 Vel(this Rigidbody rb)
        {
#if UNITY_6000_0_OR_NEWER
            return rb.linearVelocity;
#else
            return rb.velocity;
#endif
        }

        public static void SetVel(this Rigidbody rb, Vector3 v)
        {
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = v;
#else
            rb.velocity = v;
#endif
        }
    }
}
