using UnityEngine;
using UnityEngine.Playables;

namespace Wagenheimer.TimelineTypewriter
{
    /// <summary>
    /// Drives the bound SfxTypewriterText's reveal progress from whichever SfxTypewriterClip is
    /// currently active. Progress is paced by that clip's own SFX length (not the clip's full
    /// on-track length), so dragging the clip longer than its SFX just holds the fully revealed text
    /// instead of slowing the reveal down. The label hides whenever no clip is active on this track.
    /// </summary>
    public class SfxTypewriterMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var target = playerData as SfxTypewriterText;
            if (target == null) return;

            int inputCount = playable.GetInputCount();
            int activeInput = -1;
            float bestWeight = 0f;
            float bestProgress = 0f;

            for (int i = 0; i < inputCount; i++)
            {
                float weight = playable.GetInputWeight(i);
                if (weight <= 0f) continue;

                var inputPlayable = (ScriptPlayable<SfxTypewriterBehaviour>)playable.GetInput(i);
                var behaviour = inputPlayable.GetBehaviour();
                double time = inputPlayable.GetTime();
                float revealDuration = behaviour.SfxClip != null
                    ? behaviour.SfxClip.length
                    : (float)inputPlayable.GetDuration();
                float progress = revealDuration > 0f ? Mathf.Clamp01((float)time / revealDuration) : 1f;

                if (weight >= bestWeight)
                {
                    bestWeight = weight;
                    bestProgress = progress;
                    activeInput = i;
                }
            }

            target.SetProgress01(activeInput >= 0 ? bestProgress : 0f);
        }
    }
}
