using System.Collections;
using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// Walks toward player. [Vlambeer P15] anticipation telegraph before attack.
    /// [Vlambeer P19] white-flash on hit. [P23] death is a celebration.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour
    {
        public enum Kind { Heavy, Light }

        [Header("Stats")]
        [SerializeField] private int maxHp = 3;
        [SerializeField] private float speed = 2.2f;
        [SerializeField] private float damageRange = 1.1f;
        [SerializeField] private float attackTelegraphTime = 0.4f;
        [SerializeField] private float attackCooldown = 1.2f;

        [Header("Visual")]
        [SerializeField] private Color baseColor = new Color(0.55f, 0.18f, 0.22f);
        [SerializeField] private Color telegraphColor = new Color(1f, 0.55f, 0.45f);

        private int _hp;
        private Transform _target;
        private Rigidbody _rb;
        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;
        private float _nextAttackTime;
        private bool _telegraphing;
        private bool _dying;
        private Kind _kind;

        public int ScoreValue { get; private set; } = 10;

        public void Init(Kind kind, Transform target)
        {
            _kind = kind;
            _target = target;
            if (kind == Kind.Light)
            {
                maxHp = 1; speed = 3.6f; ScoreValue = 5;
                baseColor = new Color(0.85f, 0.45f, 0.35f);
                transform.localScale *= 0.85f;
            }
            else
            {
                maxHp = 3; speed = 1.8f; ScoreValue = 15;
                transform.localScale *= 1.1f;
            }
            _hp = maxHp;
            ApplyColor(baseColor);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            _rb.useGravity = false;
            _renderer = GetComponentInChildren<Renderer>();
            _mpb = new MaterialPropertyBlock();
        }

        private void FixedUpdate()
        {
            if (_dying || _target == null) return;

            Vector3 toPlayer = _target.position - transform.position;
            toPlayer.y = 0;
            float dist = toPlayer.magnitude;

            if (dist > damageRange + 0.1f)
            {
                Vector3 dir = toPlayer.normalized;
                Vector3 desired = dir * speed;
                _rb.SetVel(Vector3.MoveTowards(_rb.Vel(), desired, 12f * Time.fixedDeltaTime));
                transform.rotation = Quaternion.LookRotation(dir);
            }
            else
            {
                _rb.SetVel(Vector3.zero);
                if (Time.time >= _nextAttackTime && !_telegraphing)
                    StartCoroutine(TelegraphAndAttack());
            }
        }

        private IEnumerator TelegraphAndAttack()
        {
            _telegraphing = true;
            float t = 0;
            while (t < attackTelegraphTime)
            {
                float k = t / attackTelegraphTime;
                ApplyColor(Color.Lerp(baseColor, telegraphColor, k));
                t += Time.deltaTime;
                yield return null;
            }
            ApplyColor(telegraphColor);

            // Attack land
            if (_target != null)
            {
                Vector3 toP = _target.position - transform.position;
                toP.y = 0;
                if (toP.magnitude <= damageRange + 0.4f)
                {
                    var pc = _target.GetComponent<PlayerController>();
                    if (pc != null) pc.TakeHit();
                }
            }

            yield return new WaitForSeconds(0.08f);
            ApplyColor(baseColor);
            _nextAttackTime = Time.time + attackCooldown;
            _telegraphing = false;
        }

        public void TakeHit(int damage, Vector3 knockback)
        {
            if (_dying) return;
            _hp -= damage;
            _rb.AddForce(knockback, ForceMode.VelocityChange);
            StartCoroutine(WhiteFlash()); // [Vlambeer P19]
            if (_hp <= 0) StartCoroutine(Die());
        }

        private IEnumerator WhiteFlash()
        {
            ApplyColor(Color.white);
            yield return new WaitForSecondsRealtime(0.07f);
            if (!_dying) ApplyColor(_telegraphing ? telegraphColor : baseColor);
        }

        private IEnumerator Die()
        {
            _dying = true;
            GameManager.Instance?.RegisterKill(this);
            // [Vlambeer P23] death > spawn 3x
            Juice.Shake(0.3f, 0.2f);
            Juice.HitStop(0.05f);

            // Quick scale-down pop (P26 — pop, don't fade)
            float t = 0; const float dur = 0.18f;
            Vector3 s0 = transform.localScale;
            while (t < dur)
            {
                float k = t / dur;
                transform.localScale = Vector3.Lerp(s0 * 1.25f, Vector3.zero, k);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }

        private void ApplyColor(Color c)
        {
            if (_renderer == null) return;
            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", c);
            _mpb.SetColor("_Color", c); // legacy fallback
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}
