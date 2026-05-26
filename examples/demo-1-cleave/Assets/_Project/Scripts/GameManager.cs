using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// 60-second timer, score, combo, win/lose state.
    /// Combo escalates per [Vlambeer P22] — N consecutive hits without taking damage
    /// multiplies score and triggers UI intensity changes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public const float MatchDuration = 60f;

        private GameObject _player;
        private PlayerController _pc;
        private float _matchStart;
        private int _score;
        private int _combo;
        private int _bestCombo;
        private bool _running;
        private bool _ended;

        public bool IsRunning => _running && !_ended;
        public int Score => _score;
        public int Combo => _combo;
        public int BestCombo => _bestCombo;
        public float TimeRemaining => _ended
            ? _frozenTimeRemaining
            : Mathf.Max(0f, MatchDuration - (Time.time - _matchStart));
        public bool Ended => _ended;
        public string EndState { get; private set; }
        private float _frozenTimeRemaining;

        public void SetPlayer(GameObject player)
        {
            _player = player;
            _pc = player.GetComponent<PlayerController>();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Start()
        {
            _matchStart = Time.time;
            _running = true;
        }

        private void Update()
        {
            if (_ended) return;
            if (!_pc.IsAlive)
            {
                EndMatch("DOWN");
                return;
            }
            if (TimeRemaining <= 0f)
            {
                EndMatch(GetSurvivalGrade());
            }
        }

        private string GetSurvivalGrade()
        {
            if (_score >= 250) return "REAPER";
            if (_score >= 100) return "CLEAVER";
            return "SURVIVED";
        }

        public void RegisterCleave(int hitsLanded, CleaveAttack.CleaveTier tier)
        {
            if (_ended) return;
            // No hit = combo break? No — only player damage breaks combo.
            // Empty swing is fine (committed swings cost nothing here).
        }

        public void RegisterKill(Enemy enemy)
        {
            if (_ended) return;
            _combo++;
            if (_combo > _bestCombo) _bestCombo = _combo;
            int multiplier = 1 + (_combo / 5); // every 5-kill streak adds 1x
            _score += enemy.ScoreValue * multiplier;
        }

        public void OnPlayerDamaged()
        {
            _combo = 0;
        }

        private void EndMatch(string state)
        {
            _ended = true;
            _running = false;
            _frozenTimeRemaining = Mathf.Max(0f, MatchDuration - (Time.time - _matchStart));
            EndState = state;
            Debug.Log($"[Cleave] Match end: {state} — Score {_score}, Best combo {_bestCombo}. Press R to restart.");
        }
    }
}
