# Unity Patterns

> Encoded Unity-specific patterns that indie devs reach for again and again.
> This is the *Unity API how-to* layer — companion to the design-taste layers
> in [[vlambeer-juice]], [[game-feel-swink]], [[tyroller-mistakes]], [[gmtk-patterns]].

Skills cite these by ID: `UP1` (Unity Pattern 1), `UP2`, etc.

---

## UP1 — ScriptableObject configs over hardcoded numbers
**When:** Weapons, enemies, items, levels, abilities have tunable stats.
**Why:** Designers tune in Inspector, no recompile. Data lives outside code.

```csharp
[CreateAssetMenu(menuName = "Vibe/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [Header("Damage")]
    public int damage = 1;
    public float knockback = 8f;

    [Header("Timing")]
    public float fireRate = 0.2f;
    public float chargeMinTime = 0.18f;
    public float chargeMaxTime = 1.2f;

    [Header("Juice — Vlambeer P3, P4")]
    public float screenShakeAmplitude = 0.15f;
    public float hitStopSeconds = 0.06f;
}
```

Usage: `[SerializeField] WeaponConfig config;` — drag a `.asset` into the field.

**Anti-pattern:** Magic numbers scattered across multiple MonoBehaviours.

---

## UP2 — ScriptableObject Event Channels (decoupled communication)
**When:** Systems should not know about each other (UI doesn't reference GameManager, etc.).
**Why:** Drag-drop wiring in Inspector, no FindObjectOfType, no static singletons everywhere.

```csharp
[CreateAssetMenu(menuName = "Vibe/Events/Int Event")]
public class IntEventChannel : ScriptableObject
{
    public event System.Action<int> OnRaised;
    public void Raise(int value) => OnRaised?.Invoke(value);
}
```

Wire it: `GameManager` raises `ScoreChangedEvent.Raise(score)`. `ScoreUI` listens
via `_event.OnRaised += UpdateText`. The two never reference each other directly.

**Reference:** Ryan Hipple's *Game Architecture with ScriptableObjects* (Unite 2017).

---

## UP3 — Object pooling for any spawned object
**When:** Bullets, particles, damage numbers, enemies, audio sources.
**Why:** Instantiate/Destroy churn kills mobile perf and causes GC hitches.

Unity 6 has built-in `ObjectPool<T>`:

```csharp
private ObjectPool<Bullet> _pool;

void Awake()
{
    _pool = new ObjectPool<Bullet>(
        createFunc: () => Instantiate(bulletPrefab),
        actionOnGet: b => b.gameObject.SetActive(true),
        actionOnRelease: b => b.gameObject.SetActive(false),
        actionOnDestroy: Destroy,
        defaultCapacity: 32,
        maxSize: 128
    );
}

public void Fire() => _pool.Get().Launch(...);
public void ReturnBullet(Bullet b) => _pool.Release(b);
```

**Cite when applying:** *"Pooled bullets (Unity Pattern UP3) — eliminates instantiate
hitches during burst fire."*

---

## UP4 — Tween via DOTween OR coroutines (NOT linear animation)
**When:** UI animations, camera lerp, damage number float-up, anything that moves
between two values.

### DOTween (recommended, free, fast)

```csharp
transform.DOScale(1.2f, 0.18f).SetEase(Ease.OutBack);
canvasGroup.DOFade(0f, 0.35f).SetEase(Ease.InQuad);
```

Install: Unity Asset Store → DOTween (HOTween v2) → Free.

### Built-in coroutine tween (if avoiding DOTween)

```csharp
IEnumerator Tween(float dur, AnimationCurve ease, System.Action<float> setter)
{
    float t = 0;
    while (t < dur)
    {
        setter(ease.Evaluate(t / dur));
        t += Time.unscaledDeltaTime;  // unscaled so hit-stop doesn't pause UI
        yield return null;
    }
    setter(ease.Evaluate(1));
}
```

**Reference:** [[vlambeer-juice#p20--tweening-everything-eases]].
**Anti-pattern:** `Mathf.Lerp(a, b, Time.deltaTime)` in Update without proper t.

---

## UP5 — Cinemachine 3.0 Impulse for camera shake
**When:** Any meaningful impact (Vlambeer P3 — hit, death, big event).
**Why:** Cinemachine handles damping, falloff, and multi-source mixing for free.

### Setup (5 minutes)

1. Window → Package Manager → install `com.unity.cinemachine` 3.0+
2. GameObject → Cinemachine → CinemachineCamera (replaces virtual camera)
3. On the camera, add **CinemachineImpulseListener** component
4. On the player / hit source, add **CinemachineImpulseSource** component:
   - **Impulse Definition → Default Velocity:** `(0, 0.2, 0)` (vertical kick)
   - **Time Envelope:** Attack 0.05s, Decay 0.20s
   - **Impulse Shape:** Bump (or Recoil for directional kick)
5. In code:
   ```csharp
   [SerializeField] CinemachineImpulseSource impulse;
   void OnHit() => impulse.GenerateImpulseWithForce(0.5f);  // scales the magnitude
   ```

### Magnitude reference (Vlambeer P3 hierarchy)

| Event | Force argument |
|---|---|
| Bullet fire | 0.1 |
| Bullet hit | 0.3 |
| Enemy death | 0.6 |
| Player damage | 1.0 |
| Boss death | 1.5 |

**Anti-pattern:** Manually offsetting camera transform in `LateUpdate` — fragile
when multiple shake sources collide.

---

## UP6 — `Time.timeScale = 0` for hit-stop, with `unscaledDeltaTime` everywhere else
**When:** Hit impact (Vlambeer P4).
**Why:** Pausing time is the cleanest "weight" signal.

```csharp
public static class HitStop
{
    static float _until;

    public static void Freeze(float seconds)
    {
        _until = Mathf.Max(_until, Time.realtimeSinceStartup + seconds);
        Time.timeScale = 0f;
    }

    public static void Tick()
    {
        if (Time.timeScale == 0f && Time.realtimeSinceStartup >= _until)
            Time.timeScale = 1f;
    }
}
```

**CRITICAL:** any UI animation, audio, or coroutine that should keep running during
hit-stop must use `Time.unscaledDeltaTime`, `WaitForSecondsRealtime`, or
`AudioSource.ignoreListenerPause = true`.

**Reset:** Always set `Time.timeScale = 1` in `Awake` after scene reload.

---

## UP7 — Material Property Block for per-instance color flashes
**When:** White flash on hit (Vlambeer P19), damage tint, glow pulse.
**Why:** Setting `Material.color` creates a material instance per object →
draw-call explosion. MaterialPropertyBlock changes only the *property*, not the material.

```csharp
MaterialPropertyBlock _mpb;
Renderer _renderer;
void Awake() { _mpb = new MaterialPropertyBlock(); _renderer = GetComponent<Renderer>(); }

void Flash(Color c)
{
    _renderer.GetPropertyBlock(_mpb);
    if (_renderer.sharedMaterial.HasProperty("_BaseColor")) _mpb.SetColor("_BaseColor", c);
    if (_renderer.sharedMaterial.HasProperty("_Color")) _mpb.SetColor("_Color", c);
    _renderer.SetPropertyBlock(_mpb);
}
```

URP/Unlit uses `_BaseColor`. Built-in legacy uses `_Color`. Setting both is safe.

---

## UP8 — Animator parameters via hash, not string
**When:** Triggering animations from code.
**Why:** String parameter lookups happen every call. Hash once, use forever.

```csharp
static readonly int HashHit = Animator.StringToHash("Hit");
static readonly int HashSpeed = Animator.StringToHash("Speed");

animator.SetTrigger(HashHit);
animator.SetFloat(HashSpeed, currentSpeed);
```

**Anti-pattern:** `animator.SetTrigger("Hit")` in hot path.

---

## UP9 — InputSystem Action via code, NOT Inspector-bound PlayerInput
**When:** Prototype/jam scope. (For production, PlayerInput + action assets is fine.)
**Why:** Inspector-bound input requires .inputactions asset + PlayerInput component
wiring. Code-bound is 0-config, ships fast.

```csharp
InputAction _move;

void Awake()
{
    _move = new InputAction("Move", InputActionType.Value);
    _move.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
}

void OnEnable() => _move.Enable();
void OnDisable() => _move.Disable();

void Update()
{
    Vector2 dir = _move.ReadValue<Vector2>();
}
```

**Reference:** Cleave demo `PlayerController.cs` uses this pattern.

---

## UP10 — Audio mixer with ducking for combat tension
**When:** Music should dip when combat is hot, return when calm.

1. Audio Mixer → create 2 groups: `Music`, `SFX`
2. Music group → Inspector → "Add effect..." → **Duck Volume**
3. Sidechain: SFX group
4. Tune: Threshold -20dB, Ratio 4:1, Attack 50ms, Release 800ms

In code:
```csharp
[SerializeField] AudioMixer mixer;
[SerializeField] AudioMixerGroup musicGroup;

mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
```

**Reference:** [[vlambeer-juice#p8--music]].

---

## UP11 — Save/Load via `JsonUtility` + Application.persistentDataPath
**When:** Save state needed (most non-jam projects).
**Why:** Built-in, no dependencies, JSON is debuggable.

```csharp
[System.Serializable]
public class SaveData
{
    public int highScore;
    public string lastBrief;
}

void Save()
{
    var data = new SaveData { highScore = score, lastBrief = brief };
    var json = JsonUtility.ToJson(data, prettyPrint: true);
    File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.json"), json);
}

SaveData Load()
{
    var path = Path.Combine(Application.persistentDataPath, "save.json");
    if (!File.Exists(path)) return new SaveData();
    return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
}
```

**Anti-pattern:** PlayerPrefs for anything beyond settings.
**For production:** Consider Easy Save 3 (paid) or [Newtonsoft Json + IEnumerable<SaveItem>].

---

## UP12 — Coroutine vs async/await (Unity 6+)
**When:** Sequencing time-based effects.
**Default:** Coroutines for game loop, async for I/O.

```csharp
// Coroutine (Unity-aware, hooks Time.timeScale)
IEnumerator Cleave() {
    yield return new WaitForSeconds(0.05f);
    DoTheThing();
}

// async/await (cleaner code, requires UniTask for Unity context)
async UniTask CleaveAsync() {
    await UniTask.Delay(50);  // ms, respects Time.timeScale via PlayerLoopTiming
    DoTheThing();
}
```

**Anti-pattern:** mixing `await Task.Delay(50)` in Unity — does not respect timeScale.

---

## UP13 — URP shader for hit flash (no material instance)
**When:** Material Property Block isn't enough (need full-surface override).
**Why:** A custom Unlit shader with `_FlashAmount` lets you Lerp without
re-creating materials.

See `templates/hit-flash.shader` in this repo.

```hlsl
// Pseudocode — see actual file
half4 frag(v2f i) : SV_Target {
    half4 base = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _BaseColor;
    half4 flash = half4(1, 1, 1, 1);
    return lerp(base, flash, _FlashAmount);
}
```

Set via `MaterialPropertyBlock` → `_FlashAmount` from 0→1→0 over 70ms.

---

## UP14 — Build profiles + scene management
**When:** Shipping (`/ship` skill).
**Why:** Avoid manually reorganizing build settings every time.

Unity 6 Build Profiles (Window → Build Profiles):
- WebGL — Brotli compression, no decompression fallback
- StandaloneWindows64 — Mono backend for fast iteration, IL2CPP for ship

Programmatic build (used by `/ship`):

```csharp
public static void BuildWebGL()
{
    var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    var opts = new BuildPlayerOptions
    {
        scenes = scenes,
        locationPathName = "Builds/WebGL",
        target = BuildTarget.WebGL,
        options = BuildOptions.None
    };
    BuildPipeline.BuildPlayer(opts);
}
```

---

## UP15 — Editor-time validation with OnValidate
**When:** Inspector-set fields need sanity checking.
**Why:** Catches bad data at design time, not runtime.

```csharp
void OnValidate()
{
    if (chargeMinTime > chargeMaxTime)
    {
        Debug.LogWarning($"{name}: chargeMinTime must be <= chargeMaxTime");
        chargeMinTime = chargeMaxTime;
    }
    moveSpeed = Mathf.Max(0, moveSpeed);
}
```

**Anti-pattern:** Catching invalid configs at first frame of Play mode.

---

## How skills use this file

When `/prototype` builds a new system:
1. It checks if the system has data (stats, configs) → recommend **UP1** ScriptableObject
2. It checks for cross-system communication → recommend **UP2** event channels
3. It checks for spawned objects → recommend **UP3** pooling

When `/juice` applies polish:
1. Camera shake → **UP5** Cinemachine Impulse with magnitude from Vlambeer P3 table
2. Hit-stop → **UP6** `Time.timeScale` with `unscaledDeltaTime` discipline
3. White flash → **UP7** MaterialPropertyBlock, or **UP13** shader if needed
4. Tweening → **UP4** DOTween or coroutine
5. Audio → **UP10** mixer with sidechain

Citations land like:
> ✓ Bullet pool initialized (Unity Pattern UP3) — capacity 32, max 128.
> ✓ Cinemachine Impulse Source on player (UP5, P3 magnitude 1.0 for player damage).

## See also

- [[vlambeer-juice]] — what to apply, magnitude defaults
- [[game-feel-swink]] — why it works
- [[gmtk-patterns]] — structural design
- [[tyroller-mistakes]] — what kills projects (this file is *the how*, not *the why*)
