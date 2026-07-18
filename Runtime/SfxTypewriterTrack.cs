using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Wagenheimer.TimelineTypewriter
{
    [TrackClipType(typeof(SfxTypewriterClip))]
    [TrackBindingType(typeof(SfxTypewriterText))]
    public class SfxTypewriterTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SfxTypewriterMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
