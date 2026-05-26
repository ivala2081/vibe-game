using System.Collections;
using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// Click-charge-release radial attack. Crit window per [GMTK risk-reward].
    /// Charge timing window: 0.5s-1.2s (green), miss = weak swing.
    /// Each landed hit: [Vlambeer P4] hit-stop, [P3] shake, [P5] particles, [P13] knockback.
    /// </summary>
    public class CleaveAttack : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private float minChargeTime = 0.18f;
        [SerializeField] private float critWindowStart = 0.5f;
        [SerializeField] private float critWindowEnd = 1.2f;
        [SerializeField] private float maxChargeTime = 2.0f;

        [Header("Cleave")]
        [SerializeField] private float weakRadius = 2.0f;
        [SerializeField] private float strongRadius = 3.6f;
        [SerializeField] private float critRadius = 5.0f;
        [SerializeField] private float knockbackForce = 14f;
        [SerializeField] private int weakDamage = 1;
        [SerializeField] private int strongDamage = 2;
        [SerializeField] private int critDamage = 4;

        [Header("Visuals")]
        [SerializeField] private Color critColor = new Color(0.95f, 0.85f, 0.30f);
        [SerializeField] private Color strongColor = new Color(0.95f, 0.95f, 0.97f);

        private float _chargeStart;
        private bool _charging;
        private GameObject _chargeRing;
        private LineRenderer _chargeRingLr;

        public bool IsCharging => _charging;
        public float ChargeNormalized
            => _charging ? Mathf.Clamp01((Time.time - _chargeStart) / maxChargeTime) : 0f;

        public void BeginCharge()
        {
            _charging = true;
            _chargeStart = Time.time;
            SpawnChargeRing();
        }

        private void Update()
        {
            if (_charging) UpdateChargeRing();
        }

        private void SpawnChargeRing()
        {
            if (_chargeRing != null) Destroy(_chargeRing);
            _chargeRing = new GameObject("ChargeRing");
            _chargeRing.transform.SetParent(transform, worldPositionStays: false);
            _chargeRing.transform.localPosition = new Vector3(0, -0.45f, 0);
            _chargeRingLr = _chargeRing.AddComponent<LineRenderer>();
            const int segments = 32;
            _chargeRingLr.positionCount = segments + 1;
            _chargeRingLr.useWorldSpace = false;
            _chargeRingLr.widthMultiplier = 0.08f;
            _chargeRingLr.loop = false;
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                         ?? Shader.Find("Unlit/Color")
                         ?? Shader.Find("Sprites/Default");
            _chargeRingLr.material = new Material(shader);
        }

        private void UpdateChargeRing()
        {
            if (_chargeRing == null) return;
            float held = Time.time - _chargeStart;
            float t = Mathf.Clamp01(held / maxChargeTime);

            // Color: white → gold during crit window → white again past it
            Color c;
            if (held < critWindowStart) c = Color.Lerp(new Color(1, 1, 1, 0.5f), strongColor, t);
            else if (held <= critWindowEnd) c = critColor;
            else c = new Color(strongColor.r, strongColor.g, strongColor.b, 0.8f);

            if (_chargeRingLr.material.HasProperty("_BaseColor")) _chargeRingLr.material.SetColor("_BaseColor", c);
            if (_chargeRingLr.material.HasProperty("_Color")) _chargeRingLr.material.SetColor("_Color", c);
            _chargeRingLr.startColor = _chargeRingLr.endColor = c;

            // Radius previews the cleave that would land right now
            float r = held >= critWindowStart && held <= critWindowEnd ? critRadius
                    : held > critWindowEnd ? strongRadius
                    : weakRadius * (0.4f + t * 0.6f);
            int segments = _chargeRingLr.positionCount - 1;
            for (int i = 0; i <= segments; i++)
            {
                float a = (i / (float)segments) * Mathf.PI * 2f;
                _chargeRingLr.SetPosition(i, new Vector3(Mathf.Cos(a) * r, 0, Mathf.Sin(a) * r));
            }
        }

        private void DestroyChargeRing()
        {
            if (_chargeRing != null) Destroy(_chargeRing);
            _chargeRing = null;
            _chargeRingLr = null;
        }

        public void ReleaseCleave()
        {
            if (!_charging) return;
            float held = Time.time - _chargeStart;
            _charging = false;
            DestroyChargeRing();

            if (held < minChargeTime) return;

            CleaveTier tier;
            float radius;
            int damage;
            Color flashColor;

            if (held >= critWindowStart && held <= critWindowEnd)
            {
                tier = CleaveTier.Crit;
                radius = critRadius;
                damage = critDamage;
                flashColor = critColor;
            }
            else if (held > critWindowEnd)
            {
                tier = CleaveTier.Strong;
                radius = strongRadius;
                damage = strongDamage;
                flashColor = strongColor;
            }
            else
            {
                tier = CleaveTier.Weak;
                radius = weakRadius;
                damage = weakDamage;
                flashColor = strongColor;
            }

            Execute(tier, radius, damage, flashColor);
        }

        private void Execute(CleaveTier tier, float radius, int damage, Color flash)
        {
            int hitsLanded = 0;
            var hits = Physics.OverlapSphere(transform.position, radius);
            foreach (var h in hits)
            {
                var enemy = h.GetComponentInParent<Enemy>();
                if (enemy == null) continue;
                Vector3 dir = (enemy.transform.position - transform.position);
                dir.y = 0;
                enemy.TakeHit(damage, dir.normalized * knockbackForce);
                hitsLanded++;
            }

            // Juice scaled by tier and hits-landed (Vlambeer P3 hierarchy)
            float shakeAmp = tier switch
            {
                CleaveTier.Crit => 0.45f,
                CleaveTier.Strong => 0.22f,
                _ => 0.10f
            } + hitsLanded * 0.03f;

            float hitStop = tier switch
            {
                CleaveTier.Crit => 0.10f + hitsLanded * 0.015f,
                CleaveTier.Strong => 0.06f,
                _ => 0.03f
            };

            Juice.HitStop(hitStop);
            Juice.Shake(shakeAmp, 0.25f + hitsLanded * 0.02f);
            if (tier == CleaveTier.Crit && hitsLanded >= 3)
                Juice.SlowMo(0.25f, 0.4f); // [Vlambeer P17] reserve for moments

            StartCoroutine(RadialFlash(radius, flash));

            GameManager.Instance?.RegisterCleave(hitsLanded, tier);
        }

        private IEnumerator RadialFlash(float radius, Color color)
        {
            // Simple ring of unlit quads, expand+fade. [Vlambeer P5] particles substitute.
            int segments = 24;
            var ring = new GameObject("CleaveRing");
            ring.transform.position = transform.position + Vector3.up * 0.05f;
            var lr = ring.AddComponent<LineRenderer>();
            lr.positionCount = segments + 1;
            lr.loop = false;
            lr.useWorldSpace = false;
            lr.widthMultiplier = 0.15f;
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                         ?? Shader.Find("Unlit/Color")
                         ?? Shader.Find("Sprites/Default");
            lr.material = new Material(shader);
            if (lr.material.HasProperty("_BaseColor")) lr.material.SetColor("_BaseColor", color);
            if (lr.material.HasProperty("_Color")) lr.material.SetColor("_Color", color);

            float t = 0f;
            const float dur = 0.35f;
            while (t < dur)
            {
                float k = t / dur;
                // [Vlambeer P20] OutExpo expansion
                float ease = 1f - Mathf.Pow(1f - k, 4f);
                float r = Mathf.Lerp(0.2f, radius, ease);
                for (int i = 0; i <= segments; i++)
                {
                    float a = (i / (float)segments) * Mathf.PI * 2f;
                    lr.SetPosition(i, new Vector3(Mathf.Cos(a) * r, 0, Mathf.Sin(a) * r));
                }
                var c = color;
                c.a = Mathf.Lerp(1f, 0f, ease);
                lr.startColor = lr.endColor = c;
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            Destroy(ring);
        }

        public enum CleaveTier { Weak, Strong, Crit }
    }
}
