# Unity Timeline Typewriter

A Unity package that provides a custom Timeline track for generating a TextMeshPro typewriter effect. The reveal rate of the characters is automatically paced and synchronized to match the length of an assigned `AudioClip` (sound effect).

## Features
- **Paced Reveal:** The character typing speed is calculated dynamically based on the sound effect's actual duration.
- **Timeline Scrubbing:** View and scrub the typewriter progress directly in the Unity Editor Timeline window.
- **Easy Alignment:** Custom Inspector helper button to automatically align/create the audio track clip.
- **Hold Region Support:** Dragging the clip longer than the SFX duration automatically holds the fully revealed text without stretching the reveal animation.

## Installation
Add the following line to your `Packages/manifest.json`:
```json
"com.wagenheimer.timelinetypewriter": "https://github.com/wagenheimer/UnityTimelineTypewriter.git"
```

## How to Use
1. Add the `SfxTypewriterText` component to your TextMeshPro UGUI label.
2. In your Timeline, add a new `Sfx Typewriter Track` and bind it to the `SfxTypewriterText` component.
3. Add a `SfxTypewriterClip` to the track.
4. Assign an `AudioClip` in the clip's inspector.
5. Click **"Create and Align Audio Clip in Timeline"** in the inspector to automatically set up the corresponding audio playback track and sync them.
