using System.Collections;
using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// Centralized game feel. Static API so any system can call without refs.
    /// Implements [Vlambeer P3] shake (Perlin noise), [P4] hit-stop, [P17] slo-mo.
    /// Mood-aware tuning: this demo's brief mood is "tense, weighty" — defaults are
    /// restrained vs. a "frenetic" mood which would 2-3x these magnitudes.
    /// </summary>
    public class Juice : MonoBehaviour
    {
        private static Juice _instance;

        private Vector3 _camBasePos;
        private Camera _cam;
        private float _shakeAmp;
        private float _shakeRemaining;
        private float _shakeDuration;
        private float _hitStopUntil;
        private float _slowMoUntil;
        private float _slowMoScale = 1f;
        private float _seedX, _seedY;

        private void Awake()
        {
            _instance = this;
            _cam = GetComponent<Camera>();
            _camBasePos = transform.localPosition;
            _seedX = Random.value * 100f;
            _seedY = Random.value * 100f;
            // Reset on scene reload — hit-stop active at death-time would otherwise persist.
            Time.timeScale = 1f;
        }

        private void LateUpdate()
        {
            // Hit-stop — handled via Time.timeScale, expires here
            if (Time.realtimeSinceStartup >= _hitStopUntil && Time.realtimeSinceStartup >= _slowMoUntil)
            {
                Time.timeScale = 1f;
            }
            else if (Time.realtimeSinceStartup < _slowMoUntil)
            {
                Time.timeScale = _slowMoScale;
            }

            // Shake — Perlin noise offset, decays over duration
            if (_shakeRemaining > 0f)
            {
                float t = _shakeRemaining / _shakeDuration;
                // [Vlambeer P20] OutExpo decay
                float decay = Mathf.Pow(t, 1.6f);
                float ox = (Mathf.PerlinNoise(_seedX + Time.realtimeSinceStartup * 28f, 0) - 0.5f) * 2f * _shakeAmp * decay;
                float oy = (Mathf.PerlinNoise(0, _seedY + Time.realtimeSinceStartup * 28f) - 0.5f) * 2f * _shakeAmp * decay;
                transform.localPosition = _camBasePos + new Vector3(ox, oy, 0);
                _shakeRemaining -= Time.unscaledDeltaTime;
            }
            else
            {
                transform.localPosition = _camBasePos;
            }
        }

        public static void Shake(float amplitude, float duration)
        {
            if (_instance == null) return;
            // Take max of current vs incoming — bigger event wins
            _instance._shakeAmp = Mathf.Max(_instance._shakeAmp * (_instance._shakeRemaining / Mathf.Max(_instance._shakeDuration, 0.01f)), amplitude);
            _instance._shakeDuration = duration;
            _instance._shakeRemaining = duration;
        }

        public static void HitStop(float seconds)
        {
            if (_instance == null || seconds <= 0f) return;
            float until = Time.realtimeSinceStartup + seconds;
            if (until > _instance._hitStopUntil)
            {
                _instance._hitStopUntil = until;
                Time.timeScale = 0f;
            }
        }

        public static void SlowMo(float scale, float duration)
        {
            if (_instance == null) return;
            _instance._slowMoScale = Mathf.Clamp(scale, 0.05f, 1f);
            _instance._slowMoUntil = Time.realtimeSinceStartup + duration;
            Time.timeScale = _instance._slowMoScale;
        }
    }
}
