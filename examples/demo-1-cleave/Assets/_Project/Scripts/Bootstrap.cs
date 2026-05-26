using UnityEngine;

namespace Cleave
{
    /// <summary>
    /// Builds the entire Cleave scene programmatically.
    /// Attach to a single empty GameObject in a blank scene. Press Play.
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        [Header("Arena")]
        [SerializeField] private float arenaRadius = 12f;
        [SerializeField] private Color floorColor = new Color(0.10f, 0.10f, 0.12f);
        [SerializeField] private Color wallColor = new Color(0.18f, 0.18f, 0.22f);

        [Header("Player")]
        [SerializeField] private Color playerColor = new Color(0.95f, 0.95f, 0.97f);
        [SerializeField] private float playerSize = 0.6f;

        private void Awake()
        {
            BuildArena();
            BuildLighting();
            var player = BuildPlayer();
            var game = BuildGameManager(player);
            var ui = BuildUI(game);
            BuildSpawner(player, game);
            BuildJuiceCamera();
            Debug.Log("[Cleave] Bootstrap complete. WASD to move, hold LMB to charge cleave.");
        }

        private void BuildArena()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.localScale = Vector3.one * (arenaRadius * 0.2f);
            SetFlatColor(floor.GetComponent<Renderer>(), floorColor);
            Destroy(floor.GetComponent<MeshCollider>());

            for (int i = 0; i < 32; i++)
            {
                float a = (i / 32f) * Mathf.PI * 2f;
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.name = $"Wall_{i}";
                wall.transform.position = new Vector3(Mathf.Cos(a) * arenaRadius, 0.5f, Mathf.Sin(a) * arenaRadius);
                wall.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
                SetFlatColor(wall.GetComponent<Renderer>(), wallColor);
            }
        }

        private void BuildLighting()
        {
            var lightGo = new GameObject("KeyLight");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(55f, -30f, 0f);
            light.intensity = 1.1f;
            light.color = new Color(1f, 0.96f, 0.88f);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.08f, 0.08f, 0.12f);
        }

        private GameObject BuildPlayer()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.localScale = Vector3.one * playerSize;
            player.transform.position = new Vector3(0, 0.5f, 0);
            SetFlatColor(player.GetComponent<Renderer>(), playerColor);
            Destroy(player.GetComponent<CapsuleCollider>());

            var rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var col = player.AddComponent<CapsuleCollider>();
            col.center = Vector3.zero;
            col.radius = 0.5f;
            col.height = 2f;

            // [Vlambeer P15 / Game Feel M5] forward indicator — capsule alone has no readable facing.
            var nose = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nose.name = "FacingNose";
            nose.transform.SetParent(player.transform, worldPositionStays: false);
            nose.transform.localPosition = new Vector3(0, 0, 0.55f);
            nose.transform.localScale = new Vector3(0.18f, 0.18f, 0.45f);
            Destroy(nose.GetComponent<BoxCollider>());
            SetFlatColor(nose.GetComponent<Renderer>(), new Color(0.95f, 0.82f, 0.28f));

            player.AddComponent<PlayerController>();
            player.AddComponent<CleaveAttack>();
            player.tag = "Player";
            return player;
        }

        private GameManager BuildGameManager(GameObject player)
        {
            var gmGo = new GameObject("GameManager");
            var gm = gmGo.AddComponent<GameManager>();
            gm.SetPlayer(player);
            return gm;
        }

        private ScoreUI BuildUI(GameManager gm)
        {
            var uiGo = new GameObject("ScoreUI");
            var ui = uiGo.AddComponent<ScoreUI>();
            ui.Bind(gm);
            return ui;
        }

        private void BuildSpawner(GameObject player, GameManager gm)
        {
            var spawnGo = new GameObject("EnemySpawner");
            var spawner = spawnGo.AddComponent<EnemySpawner>();
            spawner.Configure(player.transform, arenaRadius - 1.5f, gm);
        }

        private void BuildJuiceCamera()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera");
                cam = camGo.AddComponent<Camera>();
                camGo.tag = "MainCamera";
                camGo.AddComponent<AudioListener>();
            }
            cam.transform.position = new Vector3(0, 18, -8);
            cam.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
            cam.backgroundColor = new Color(0.04f, 0.04f, 0.06f);
            cam.clearFlags = CameraClearFlags.SolidColor;

            cam.gameObject.AddComponent<Juice>();
        }

        private static void SetFlatColor(Renderer r, Color c)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Standard");
            var mat = new Material(shader);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", c);
            r.sharedMaterial = mat;
        }
    }
}
