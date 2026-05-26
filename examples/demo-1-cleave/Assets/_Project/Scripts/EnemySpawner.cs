using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// 60-second wave spawner. Difficulty curve follows GMTK Z-curve —
    /// easy → spike → easier → bigger spike → boss-rush at 50-60s.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn config")]
        [SerializeField] private float spawnRadius = 10f;
        [SerializeField] private float baseInterval = 2.4f;   // generous early — Tense methodical mood
        [SerializeField] private float minInterval = 0.45f;
        [SerializeField] private float firstSpawnDelay = 2.5f; // breathing room at match start

        [Header("Visual")]
        [SerializeField] private Color heavyColor = new Color(0.55f, 0.18f, 0.22f);
        [SerializeField] private Color lightColor = new Color(0.85f, 0.45f, 0.35f);

        private Transform _player;
        private GameManager _gm;
        private float _nextSpawn;
        private float _start;

        public void Configure(Transform player, float spawnRadius, GameManager gm)
        {
            _player = player;
            this.spawnRadius = spawnRadius;
            _gm = gm;
        }

        private void Start()
        {
            _start = Time.time;
            _nextSpawn = Time.time + firstSpawnDelay;
        }

        private void Update()
        {
            if (_player == null || _gm == null || !_gm.IsRunning) return;
            if (Time.time < _nextSpawn) return;

            float elapsed = Time.time - _start;
            SpawnFromCurve(elapsed);

            float interval = Mathf.Lerp(baseInterval, minInterval, elapsed / 60f);
            // Z-curve: brief lulls at 20s and 40s
            if ((elapsed > 18f && elapsed < 22f) || (elapsed > 38f && elapsed < 42f))
                interval *= 1.8f;
            _nextSpawn = Time.time + interval;
        }

        private void SpawnFromCurve(float elapsed)
        {
            // 0-15s: heavies only, slow
            // 15-30s: heavies + 30% light
            // 30-45s: 50/50 mix, faster spawning
            // 45-60s: triple-spawn waves
            Enemy.Kind kind = Enemy.Kind.Heavy;
            int batchSize = 1;

            if (elapsed >= 15f && elapsed < 30f) kind = Random.value < 0.3f ? Enemy.Kind.Light : Enemy.Kind.Heavy;
            else if (elapsed >= 30f && elapsed < 45f) kind = Random.value < 0.5f ? Enemy.Kind.Light : Enemy.Kind.Heavy;
            else if (elapsed >= 45f)
            {
                kind = Random.value < 0.6f ? Enemy.Kind.Light : Enemy.Kind.Heavy;
                batchSize = Random.Range(2, 4);
            }

            for (int i = 0; i < batchSize; i++)
                SpawnOne(kind);
        }

        private void SpawnOne(Enemy.Kind kind)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = $"Enemy_{kind}";
            Destroy(go.GetComponent<CapsuleCollider>());

            var col = go.AddComponent<CapsuleCollider>();
            col.radius = 0.5f; col.height = 2f;

            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
            pos.y = 0.5f;
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * 0.6f;

            go.AddComponent<Rigidbody>();
            var enemy = go.AddComponent<Enemy>();
            enemy.Init(kind, _player);

            // Color from spawner so taste palette stays consistent
            var r = go.GetComponent<Renderer>();
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                         ?? Shader.Find("Unlit/Color")
                         ?? Shader.Find("Standard");
            var mat = new Material(shader);
            var tint = kind == Enemy.Kind.Heavy ? heavyColor : lightColor;
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", tint);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", tint);
            r.sharedMaterial = mat;
        }
    }
}
