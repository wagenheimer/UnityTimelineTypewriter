using UnityEngine;
using UnityEngine.Playables;

namespace Wagenheimer.TimelineTypewriter
{
    /// <summary>
    /// Per-clip playable for SfxTypewriterTrack. Carries this clip's own SFX reference purely as a
    /// timing source — SfxTypewriterMixerBehaviour uses its length to pace the reveal so it finishes
    /// exactly when the audio does, even if the clip itself is dragged longer on the track (the extra
    /// length becomes a Timeline "Hold" region, keeping the text visible without re-animating it).
    /// Actual audio playback belongs on a separate, ordinary Timeline Audio Track.
    /// </summary>
    public class SfxTypewriterBehaviour : PlayableBehaviour
    {
        public AudioClip SfxClip;
    }
}
