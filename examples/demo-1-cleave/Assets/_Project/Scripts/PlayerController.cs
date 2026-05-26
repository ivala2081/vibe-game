using UnityEngine;
using UnityEngine.InputSystem;

namespace Cleave
{
    /// <summary>
    /// WASD movement + facing toward mouse, with [Vlambeer P11] smoothed lerp position
    /// and [Vlambeer P9] camera-friendly speed. Charge is delegated to CleaveAttack.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CleaveAttack))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6.5f;
        [SerializeField] private float acceleration = 28f;
        [SerializeField] private float deceleration = 36f;

        [Header("Anim")]
        [SerializeField] private float squashOnMove = 0.93f;
        [SerializeField] private float stretchOnMove = 1.07f;

        private Rigidbody _rb;
        private CleaveAttack _cleave;
        private Vector3 _baseScale;
        private InputAction _moveAction;
        private InputAction _attackAction;
        private InputAction _restartAction;
        private Camera _cam;
        private int _hitsTaken;
        private const int MaxHits = 3;

        public bool IsAlive => _hitsTaken < MaxHits;
        public int HitsTaken => _hitsTaken;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _cleave = GetComponent<CleaveAttack>();
            _baseScale = transform.localScale;

            _moveAction = new InputAction("Move", InputActionType.Value);
            _moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            _moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");

            _attackAction = new InputAction("Attack", InputActionType.Button,
                "<Mouse>/leftButton");

            _restartAction = new InputAction("Restart", InputActionType.Button,
                "<Keyboard>/r");
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _attackAction.Enable();
            _restartAction.Enable();
            _attackAction.started += OnAttackStarted;
            _attackAction.canceled += OnAttackReleased;
            _restartAction.performed += OnRestart;
        }

        private void OnDisable()
        {
            _attackAction.started -= OnAttackStarted;
            _attackAction.canceled -= OnAttackReleased;
            _restartAction.performed -= OnRestart;
            _moveAction.Disable();
            _attackAction.Disable();
            _restartAction.Disable();
        }

        private void FixedUpdate()
        {
            if (!IsAlive)
            {
                _rb.SetVel(Vector3.zero);
                return;
            }
            var input = _moveAction.ReadValue<Vector2>();
            var desired = new Vector3(input.x, 0, input.y).normalized * moveSpeed;
            var current = _rb.Vel();
            var rate = desired.sqrMagnitude > 0.01f ? acceleration : deceleration;
            var next = Vector3.MoveTowards(current, desired, rate * Time.fixedDeltaTime);
            _rb.SetVel(new Vector3(next.x, 0, next.z));

            // [Vlambeer P14] subtle squash-stretch on movement direction
            float speedT = Mathf.Clamp01(_rb.Vel().magnitude / moveSpeed);
            float sx = Mathf.Lerp(_baseScale.x, _baseScale.x * stretchOnMove, speedT);
            float sy = Mathf.Lerp(_baseScale.y, _baseScale.y * squashOnMove, speedT);
            transform.localScale = new Vector3(sx, sy, sx);
        }

        private void Update()
        {
            if (!IsAlive) return;
            FaceMouse();
        }

        private void FaceMouse()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;
            var mouse = Mouse.current?.position.ReadValue() ?? Vector2.zero;
            var ray = _cam.ScreenPointToRay(mouse);
            var ground = new Plane(Vector3.up, transform.position);
            if (ground.Raycast(ray, out float dist))
            {
                var p = ray.GetPoint(dist);
                var dir = (p - transform.position);
                dir.y = 0;
                if (dir.sqrMagnitude > 0.01f)
                {
                    var target = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, target, 18f * Time.deltaTime);
                }
            }
        }

        private void OnAttackStarted(InputAction.CallbackContext ctx)
        {
            if (IsAlive) _cleave.BeginCharge();
        }

        private void OnAttackReleased(InputAction.CallbackContext ctx)
        {
            if (IsAlive) _cleave.ReleaseCleave();
        }

        private void OnRestart(InputAction.CallbackContext ctx)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void TakeHit()
        {
            if (!IsAlive) return;
            _hitsTaken++;
            Juice.HitStop(0.12f);
            Juice.Shake(0.8f, 0.4f);
            GameManager.Instance?.OnPlayerDamaged();
            if (!IsAlive) Debug.Log("[Cleave] Player down. Press R to restart.");
        }
    }
}
