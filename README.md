# Unity Timeline Typewriter

[![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg?logo=unity)](https://unity.com/)
[![Version](https://img.shields.io/github/v/tag/wagenheimer/UnityTimelineTypewriter?label=version&color=cyan)](https://github.com/wagenheimer/UnityTimelineTypewriter/releases)
[![UPM](https://img.shields.io/badge/UPM-com.wagenheimer.timelinetypewriter-green.svg)](package.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A Unity Timeline track that reveals a **TextMeshPro** label character by character, paced by the
length of an assigned **AudioClip** — so the text finishes exactly when the sound effect does.

Instead of a fixed characters-per-second speed, the reveal is derived from the SFX duration. A
two-second clip types at a different rate than a five-second one, and both land on the last
character at the right moment. Scrubbing the Timeline updates the text in the editor as well.

---

## Features

- **SFX-paced reveal** — progress is `elapsed / audioClip.length`, so the typewriter always ends
  with the audio, regardless of the clip length.
- **Timeline scrubbing** — scrub or preview the Timeline and the label updates to the matching
  character count.
- **Hold regions** — drag the clip longer than the SFX and the extra length holds the fully
  revealed text instead of slowing the reveal down.
- **One-click audio alignment** — the clip inspector can create a matching `AudioTrack` clip and
  align it to the typewriter clip.
- **Multiple clips per track** — the mixer uses the clip with the highest weight, so overlapping
  entries blend predictably.
- **Safe defaults** — the label is hidden on `Awake` and when no clip is active, and an unassigned
  SFX falls back to the clip's own duration (instead of Timeline's near-infinite sentinel).
- **Package Hub integration** — update checks are routed to the Wagenheimer Package Hub, so the
  package appears in the unified ecosystem dashboard.

---

## Requirements

| Requirement | Version |
| :--- | :--- |
| Unity | 2021.3 or newer |
| Timeline (`com.unity.timeline`) | 1.7.6+ — installed automatically |
| TextMeshPro (`com.unity.textmeshpro`) | 3.0.6+ — installed automatically |
| Wagenheimer Package Hub | Installed automatically as an unpinned dependency |

The track targets **TextMeshPro UGUI** (`TextMeshProUGUI`). TextMeshPro 3D labels are not supported
by `SfxTypewriterText`.

---

## Installation

### Via Unity Package Manager (recommended)

1. Open **Window ▸ Package Manager**.
2. Click **`+`** ▸ **Add package from git URL...**.
3. Enter:

   ```text
   https://github.com/wagenheimer/UnityTimelineTypewriter.git
   ```

### Via `Packages/manifest.json`

```json
{
  "dependencies": {
    "com.wagenheimer.timelinetypewriter": "https://github.com/wagenheimer/UnityTimelineTypewriter.git"
  }
}
```

To pin a specific release, append the tag:

```text
https://github.com/wagenheimer/UnityTimelineTypewriter.git#vX.Y.Z
```

---

## Quick start

1. Add **`SfxTypewriterText`** to the GameObject that has your `TextMeshProUGUI` label.
2. In the Timeline window, right-click the track area and add **Sfx Typewriter Track**.
3. Drag the label's `SfxTypewriterText` into the track's binding slot.
4. Right-click the track and add an **Sfx Typewriter Clip**.
5. Select the clip and assign the **Sfx Clip** (the audio that paces the reveal).
6. Click **Create and Align Audio Clip in Timeline** in the clip inspector. This creates an
   `AudioTrack` with the same clip at the same position, so the sound plays while the text types.
7. Press play — or scrub — and the label reveals in sync with the audio.

> The typewriter track only drives the text. Actual audio playback belongs on the generated
> `AudioTrack`; this track has no audio logic of its own.

---

## How pacing works

```text
progress = clamp01(clipLocalTime / sfxClip.length)
visibleCharacters = round(progress * label.textInfo.characterCount)
```

- `clipLocalTime` is the time inside the typewriter clip on the track.
- `sfxClip.length` is what defines the reveal duration — not the clip's on-track length.
- If the clip extends past the SFX, Timeline shows a **Hold** region: the text stays fully visible
  and is not re-animated.
- If no clip is active on the track, the label is hidden (progress `0`).
- When clips overlap, the one with the highest input weight drives the label.

Because Timeline evaluates the graph in edit mode too, the reveal is visible while scrubbing — no
play mode required.

---

## Inspector reference

**`SfxTypewriterClip`**

| Field / action | Description |
| :--- | :--- |
| **Sfx Clip** | AudioClip used as the timing source. Leave empty to fall back to the clip's own duration. |
| **Create and Align Audio Clip in Timeline** | Creates (or reuses) an `AudioTrack` and places the same audio clip aligned with this typewriter clip. Shown only when an `Sfx Clip` is assigned. |

The inspector also warns when the matching audio clip is missing from the Timeline, so it is clear
why a scene is silent.

---

## Project structure

```text
UnityTimelineTypewriter/
├── Runtime/
│   ├── SfxTypewriterText.cs          # Bound target: reveals TextMeshProUGUI characters
│   ├── SfxTypewriterTrack.cs         # Timeline track and mixer factory
│   ├── SfxTypewriterClip.cs          # Clip asset, exposes the SFX and intrinsic duration
│   ├── SfxTypewriterBehaviour.cs     # Per-clip playable carrying the SFX reference
│   └── SfxTypewriterMixerBehaviour.cs# Evaluates progress and drives the target
└── Editor/
    ├── SfxTypewriterClipEditor.cs    # Audio alignment helper and warnings
    └── UpdateChecker.cs              # Routes update checks to the Package Hub
```

Namespace: `Wagenheimer.TimelineTypewriter`.

---

## Troubleshooting

| Symptom | Fix |
| :--- | :--- |
| Text does not change during playback | Make sure the track is bound to an object with `SfxTypewriterText` and that the label reference is assigned (it auto-fills from the same GameObject). |
| Text types too fast or too slow | The reveal follows the assigned `Sfx Clip` length. Replace the clip or trim the audio. |
| No sound during the typewriter clip | Expected — add the audio with **Create and Align Audio Clip in Timeline**, or place the clip on a normal `AudioTrack` yourself. |
| Clip becomes extremely long when added | Assign an `Sfx Clip`; without one the clip falls back to one second. Use Timeline's **Match Content Length** to snap it to the SFX. |
| Text is already visible before the track starts | Confirm the label uses `SfxTypewriterText` (it hides on `Awake`) and that no other script writes `maxVisibleCharacters`. |

---

## Changelog & license

Release history lives in [CHANGELOG.md](CHANGELOG.md). This package is **MIT licensed** — see
[LICENSE](LICENSE).

---

### Wagenheimer Unity ecosystem

[Package Hub](https://github.com/wagenheimer/UnityPackageHub) ·
[Unity Utils](https://github.com/wagenheimer/UnityUtils) ·
[Build Pipeline](https://github.com/wagenheimer/UnityBuildPipeline) ·
[Cloud Save](https://github.com/wagenheimer/UnityCloudSave) ·
[Rate Control](https://github.com/wagenheimer/UnityRateControl) ·
[IAP Helper](https://github.com/wagenheimer/UnityIAPHelper) ·
[LevelPlay Helper](https://github.com/wagenheimer/UnityLevelPlayHelper) ·
[Native Social](https://github.com/wagenheimer/UnityNativeSocial) ·
[Rewired Helper](https://github.com/wagenheimer/RewiredHelper) ·
[Timeline Typewriter](https://github.com/wagenheimer/UnityTimelineTypewriter) ·
[Tk2d Porter](https://github.com/wagenheimer/UnityTk2dPorter) ·
[Tracking](https://github.com/wagenheimer/UnityTracking) ·
[I2 Localization](https://github.com/wagenheimer/UnityI2Localization)

Explore every project at **[wagenheimer.com/opensource](https://wagenheimer.com/opensource)**.
