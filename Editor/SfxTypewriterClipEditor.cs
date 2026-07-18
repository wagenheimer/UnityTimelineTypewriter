using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Wagenheimer.TimelineTypewriter.Editor
{
    [CustomEditor(typeof(SfxTypewriterClip))]
    public class SfxTypewriterClipEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SfxTypewriterClip clipAsset = (SfxTypewriterClip)target;
            
            // Find the sfxClip serialized property
            SerializedProperty sfxClipProp = serializedObject.FindProperty("sfxClip");
            AudioClip sfxClip = sfxClipProp != null ? sfxClipProp.objectReferenceValue as AudioClip : null;

            if (sfxClip == null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Assign an AudioClip to pace the typewriter effect and enable audio alignment options.", MessageType.Info);
                return;
            }

            TimelineAsset timelineAsset = TimelineEditor.inspectedAsset;
            if (timelineAsset == null)
            {
                return;
            }

            TimelineClip targetClip = null;
            TrackAsset targetTrack = null;

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                foreach (var c in track.GetClips())
                {
                    if (c.asset == clipAsset)
                    {
                        targetClip = c;
                        targetTrack = track;
                        break;
                    }
                }
                if (targetClip != null) break;
            }

            if (targetClip == null)
            {
                return;
            }

            // Check if there is an AudioTrack with a matching clip at the same start time
            bool hasMatchingAudio = false;
            AudioTrack existingAudioTrack = null;

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is AudioTrack audioTrack)
                {
                    existingAudioTrack = audioTrack; // Keep track of the first AudioTrack found
                    foreach (var c in audioTrack.GetClips())
                    {
                        if (c.asset is AudioPlayableAsset audioPlayable && audioPlayable.clip == sfxClip)
                        {
                            // Compare start times with a small epsilon
                            if (System.Math.Abs(c.start - targetClip.start) < 0.05)
                            {
                                hasMatchingAudio = true;
                                break;
                            }
                        }
                    }
                }
                if (hasMatchingAudio) break;
            }

            if (!hasMatchingAudio)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("No matching Audio Track clip found for this SFX. The sound will not play during Timeline playback.", MessageType.Warning);

                if (GUILayout.Button("Create and Align Audio Clip in Timeline"))
                {
                    Undo.RecordObject(timelineAsset, "Create and Align Audio Clip");

                    // If no audio track exists, create one
                    AudioTrack trackToUse = existingAudioTrack;
                    if (trackToUse == null)
                    {
                        trackToUse = timelineAsset.CreateTrack<AudioTrack>(null, "Sfx Audio Track");
                    }

                    // Create the audio clip
                    TimelineClip audioClip = trackToUse.CreateClip(sfxClip);
                    audioClip.start = targetClip.start;
                    audioClip.duration = sfxClip.length;

                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
                    EditorUtility.SetDirty(timelineAsset);
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Matching Audio Track clip is aligned in Timeline.", MessageType.Info);
            }
        }
    }
}
