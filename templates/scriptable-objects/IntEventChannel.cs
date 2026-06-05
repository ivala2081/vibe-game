using UnityEngine;

namespace VibeGame.Templates
{
    /// <summary>
    /// [[unity-patterns#up2]] — ScriptableObject event channel for decoupled comms.
    /// One channel per "event type". Raiser doesn't know listeners; listeners don't know raiser.
    /// Drag the same .asset into both raiser and listener Inspector slots.
    ///
    /// Reference: Ryan Hipple, "Game Architecture with Scriptable Objects" (Unite 2017).
    /// </summary>
    [CreateAssetMenu(fileName = "NewIntEvent", menuName = "Vibe/Events/Int Event Channel")]
    public class IntEventChannel : ScriptableObject
    {
        public event System.Action<int> OnRaised;

        public void Raise(int value)
        {
            OnRaised?.Invoke(value);
        }
    }
}

// Variants you may want as separate files:
//
// VoidEventChannel    — public event System.Action OnRaised; / Raise()
// FloatEventChannel   — System.Action<float>
// Vec3EventChannel    — System.Action<Vector3>
// GameObjectChannel   — System.Action<GameObject>
//
// Common channels in a typical project:
//   ScoreChangedEvent   (IntEventChannel)
//   PlayerDamagedEvent  (IntEventChannel — sends remaining HP)
//   ComboBrokenEvent    (VoidEventChannel)
//   EnemyDeathEvent     (Vec3EventChannel — sends position for popup)
//   MatchEndedEvent     (IntEventChannel — sends final score)
