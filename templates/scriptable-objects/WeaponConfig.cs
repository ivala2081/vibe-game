using UnityEngine;

namespace VibeGame.Templates
{
    /// <summary>
    /// [[unity-patterns#up1]] — ScriptableObject for tunable weapon stats.
    /// Designers tune in Inspector, no recompile. Drag a .asset reference into MonoBehaviours.
    ///
    /// To use: Assets → Create → Vibe → Configs → Weapon
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Vibe/Configs/Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Identity")]
        public string displayName = "Unnamed Weapon";
        [TextArea(2, 4)] public string flavorText = "";

        [Header("Damage")]
        public int damage = 1;
        [Tooltip("Vlambeer P13 — magnitude of knockback applied to hit target")]
        public float knockback = 8f;

        [Header("Charge timing (release-window mechanics)")]
        public float chargeMinTime = 0.18f;
        public float chargeCritWindowStart = 0.5f;
        public float chargeCritWindowEnd = 1.2f;
        public float chargeMaxTime = 2f;

        [Header("Crit multipliers")]
        public float critDamageMultiplier = 4f;
        public float critRadiusMultiplier = 2.5f;

        [Header("Juice — Vlambeer principles cited")]
        [Tooltip("Vlambeer P3 — base screen shake amplitude (Cinemachine Impulse force)")]
        [Range(0f, 2f)] public float screenShakeForce = 0.3f;

        [Tooltip("Vlambeer P4 — hit-stop duration in seconds")]
        [Range(0f, 0.5f)] public float hitStopSeconds = 0.06f;

        [Tooltip("Vlambeer P19 — flash duration on damaged target")]
        [Range(0f, 0.3f)] public float flashSeconds = 0.07f;

        [Header("Audio")]
        public AudioClip[] swingClips;
        public AudioClip[] hitClips;
        [Range(0f, 0.3f)] public float pitchVariation = 0.1f;

        // [UP15] Editor-time validation
        void OnValidate()
        {
            chargeMinTime = Mathf.Max(0f, chargeMinTime);
            chargeCritWindowStart = Mathf.Max(chargeMinTime, chargeCritWindowStart);
            chargeCritWindowEnd = Mathf.Max(chargeCritWindowStart + 0.01f, chargeCritWindowEnd);
            chargeMaxTime = Mathf.Max(chargeCritWindowEnd, chargeMaxTime);
        }
    }
}
