using TMPro;
using UnityEngine;

namespace Wagenheimer.TimelineTypewriter
{
    /// <summary>
    /// Timeline-driven target: reveals a TextMeshProUGUI's characters. Bind this component to a
    /// SfxTypewriterTrack clip in Timeline — the track's mixer calls
    /// SetProgress01 as the clip plays or is scrubbed in the Editor. Play the actual SFX from an
    /// ordinary Unity Audio Track aligned to the clip; this component has no audio logic of its own.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SfxTypewriterText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void Reset()
        {
            label = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>Sets the reveal progress (0 = no characters visible, 1 = all visible).</summary>
        public void SetProgress01(float progress01)
        {
            if (label == null) return;

            label.ForceMeshUpdate();
            int totalChars = label.textInfo.characterCount;
            label.maxVisibleCharacters = Mathf.RoundToInt(Mathf.Clamp01(progress01) * totalChars);
        }
    }
}
