using UnityEngine;

namespace VibeGame.Templates
{
    /// <summary>
    /// [[unity-patterns#up1]] — ScriptableObject for enemy stats and behavior tuning.
    /// One asset per enemy archetype. Drop into spawner / enemy MonoBehaviour.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Vibe/Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Identity")]
        public string displayName = "Grunt";
        public Color baseColor = new Color(0.55f, 0.18f, 0.22f);

        [Header("Stats")]
        public int maxHp = 3;
        [Tooltip("Walking speed in units/second")]
        public float speed = 2.2f;
        [Tooltip("Distance at which the enemy begins its attack")]
        public float damageRange = 1.1f;

        [Header("Anticipation telegraph (Vlambeer P15 / GMTK Telegraph)")]
        [Tooltip("Time to lerp from baseColor to telegraphColor before attack lands")]
        [Range(0.1f, 1.5f)] public float attackTelegraphTime = 0.4f;
        public Color telegraphColor = new Color(1f, 0.55f, 0.45f);

        [Header("Combat pacing")]
        public float attackCooldown = 1.2f;
        public int damageToPlayer = 1;

        [Header("Death")]
        [Tooltip("Score awarded for kill")]
        public int scoreValue = 10;
        [Tooltip("Vlambeer P23 — death effect intensity (death > spawn × 3)")]
        public float deathShakeForce = 0.3f;

        [Header("Visual scale (vs. base prefab)")]
        [Range(0.5f, 2.5f)] public float scaleMultiplier = 1f;

        // [UP15] guard against bad inspector data
        void OnValidate()
        {
            maxHp = Mathf.Max(1, maxHp);
            speed = Mathf.Max(0f, speed);
            attackCooldown = Mathf.Max(0.1f, attackCooldown);
        }
    }
}
