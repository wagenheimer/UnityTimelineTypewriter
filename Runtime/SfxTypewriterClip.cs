using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Wagenheimer.TimelineTypewriter
{
    /// <summary>
    /// A Timeline clip that reveals a bound SfxTypewriterText's characters, paced to match sfxClip's
    /// length. Assign sfxClip below, then right-click the clip in the Timeline window and choose
    /// "Match Content Length" to snap it to the exact SFX duration. Drag the clip's end further to
    /// keep the text visible longer afterward — Timeline shows the extra length as a "Hold" region;
    /// the reveal itself always finishes at sfxClip's natural length, never stretching to fill it.
    /// Play the actual SFX from a separate Audio Track aligned to this clip; this one only drives text.
    /// </summary>
    public class SfxTypewriterClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private AudioClip sfxClip;

        public ClipCaps clipCaps => ClipCaps.None;

        // PlayableAsset's own default duration is a near-infinite sentinel (used for "no intrinsic
        // length" assets); falling back to it here made "Match Content Length" blow the clip up to a
        // huge size whenever sfxClip wasn't assigned yet. 1 second is a sane, harmless default.
        public override double duration => sfxClip != null ? sfxClip.length : 1.0;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SfxTypewriterBehaviour>.Create(graph);
            playable.GetBehaviour().SfxClip = sfxClip;
            return playable;
        }
    }
}
