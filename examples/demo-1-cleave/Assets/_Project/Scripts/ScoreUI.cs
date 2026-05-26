using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// IMGUI HUD — keeps the demo zero-dependency on TextMeshPro / uGUI prefabs.
    /// Score, combo, time. [GMTK 1-second rule] — readable in peripheral.
    /// In a real project, replace with TMP + Canvas.
    /// </summary>
    public class ScoreUI : MonoBehaviour
    {
        private GameManager _gm;
        private GUIStyle _big, _med, _small;
        private float _comboFlashUntil;
        private int _lastCombo;

        public void Bind(GameManager gm) => _gm = gm;

        private void OnGUI()
        {
            if (_gm == null) return;
            if (_big == null) InitStyles();

            float w = Screen.width;
            float h = Screen.height;

            // Time (centered top)
            float tRem = _gm.TimeRemaining;
            string timeStr = tRem >= 10f ? Mathf.CeilToInt(tRem).ToString() : tRem.ToString("F1");
            var timeColor = tRem < 10f ? new Color(1f, 0.55f, 0.45f) : Color.white;
            DrawCentered(timeStr, w / 2f, 40, _big, timeColor);

            // Score (top-left)
            DrawShadowed("SCORE", 24, 18, _small, new Color(0.7f, 0.7f, 0.75f));
            DrawShadowed(_gm.Score.ToString(), 24, 36, _med, Color.white);

            // Combo (top-right) — flash when increments
            if (_gm.Combo != _lastCombo && _gm.Combo > _lastCombo)
            {
                _comboFlashUntil = Time.realtimeSinceStartup + 0.15f;
            }
            _lastCombo = _gm.Combo;
            if (_gm.Combo >= 2)
            {
                bool flashing = Time.realtimeSinceStartup < _comboFlashUntil;
                var col = flashing ? new Color(1f, 0.85f, 0.30f) : Color.white;
                DrawShadowed($"x{_gm.Combo}", w - 100, 24, _med, col);
                DrawShadowed("COMBO", w - 100, 56, _small, new Color(0.7f, 0.7f, 0.75f));
            }

            // End state
            if (_gm.Ended)
            {
                // Dimmed full-screen overlay for legibility
                var prev = GUI.color;
                GUI.color = new Color(0f, 0f, 0f, 0.55f);
                GUI.DrawTexture(new Rect(0, 0, w, h), Texture2D.whiteTexture);
                GUI.color = prev;

                DrawCentered(_gm.EndState, w / 2f, h / 2f - 80, _big, Color.white);
                DrawCentered($"SCORE  {_gm.Score}", w / 2f, h / 2f + 10, _med, new Color(0.85f, 0.85f, 0.90f));
                DrawCentered($"BEST COMBO  {_gm.BestCombo}", w / 2f, h / 2f + 60, _small, new Color(0.65f, 0.65f, 0.7f));
                DrawCentered("press R to restart", w / 2f, h - 60, _small, new Color(0.75f, 0.75f, 0.8f));
            }
        }

        private void InitStyles()
        {
            _big = new GUIStyle { fontSize = 56, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            _med = new GUIStyle { fontSize = 28, fontStyle = FontStyle.Bold };
            _small = new GUIStyle { fontSize = 14, fontStyle = FontStyle.Normal };
        }

        private void DrawShadowed(string text, float x, float y, GUIStyle style, Color color)
        {
            var prev = style.normal.textColor;
            style.normal.textColor = new Color(0, 0, 0, 0.7f);
            GUI.Label(new Rect(x + 2, y + 2, 200, 60), text, style);
            style.normal.textColor = color;
            GUI.Label(new Rect(x, y, 200, 60), text, style);
            style.normal.textColor = prev;
        }

        private void DrawCentered(string text, float cx, float cy, GUIStyle style, Color color)
        {
            const float w = 400; const float h = 80;
            var prev = style.normal.textColor;
            style.normal.textColor = new Color(0, 0, 0, 0.7f);
            GUI.Label(new Rect(cx - w / 2 + 2, cy - h / 2 + 2, w, h), text, style);
            style.normal.textColor = color;
            GUI.Label(new Rect(cx - w / 2, cy - h / 2, w, h), text, style);
            style.normal.textColor = prev;
        }
    }
}
