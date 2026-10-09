# Arkham City save notes

What is known about the decompressed Arkham City save layout, and how sure we are.
All numbers are big endian. "Slot" means an index into an array.

Evidence used: the four saves in `TestData/City`, two more full saves (late game, one
with all physical challenges done), and a Cheat Engine structure dump of the game's
`RExperienceSystem` object (UE3 class and member names).

## Container

* A 40 byte wrapper header, then LZO segments that each start with `9E 2A 83 C1`.
* After the last segment the file is zeros until the end (294912 bytes total). No compressed data is left over.

## Experience / statistics block (high confidence)

These fields sit in the same order as the members of `RExperienceSystem`, and their values
match the in-memory struct for a loaded save.

| Field | Meaning | Evidence |
|---|---|---|
| `brokenGrundyHeadTally` | `BrokenGrundyHeadTally` | Matches the struct member order and value |
| `pickupCount` | `PickupCount` | 247 in the base game, 282 = 247 + 35 Catwoman pickups |
| `pickupRevealsLeft` | `PickupRevealsLeft` | Value matches memory |
| `interrogatesLeft` | `InterrogatesLeft` | Value matches memory |
| `numCamerasBroken` | `NumCamerasBroken` | Equals the sum of `camerasBrokenPerArea` |
| `interrogatesPerformed` | `InterrogatesPerformed` | Value matches memory |
| `interrogatesWithTutorialPerformed` | `InterrogatesWithTutorialPerformed` | Value matches memory |
| `xp`, `xpForPrevUpgrade`, `xpForNextUpgrade`, `xpLevel` | `Xp`, `XpForPrevUpgrade`, `XpForNextUpgrade`, `XpLevel` | Stored as floats. The save order is Xp first, which differs from the in-memory order |
| `xpOverUpgradeThreshold`, `upgradesBought`, `cwUpgradesBought`, `playerLevel` | Same names as the struct | Values match memory |

## Physical challenges (high confidence)

Three arrays of 46 entries, each stored as a 4 byte count followed by the values. They are
`BatChallengeCurrent`, `BatChallengeBest` and `BatChallengeBeaten` in memory.
`BatChallengeNeeded` (the goals) is not saved.

| Array | Type | Meaning |
|---|---|---|
| `batChallengeCurrent` | float | Latest attempt or running value |
| `batChallengeBest` | float | Best distance or count. Stays 0 for done/not-done challenges. Counters keep counting past their goal |
| `batChallengeBeaten` | **uint32**, not float | 1 once the challenge is beaten, else 0 |

* Slot 0 is never used (0 in every array, every save).
* Slots 1-40 are the 40 base game challenges. Slots 41-45 are the 5 Catwoman DLC challenges.
* The third array looks like all zeros if it is read as floats, because a float with the bit pattern of integer 1 is about 1.4e-45. Read it as `uint32`.
* Slot 3 is probably the 50 meter dive: best 22.84 is not beaten, while 52.59, 56.52 and 57.97 are.
* Which slot is which challenge is **not yet mapped**. The in-memory arrays are in the same order, so the names can be read off the game's list.

## Per-area collection counters (high confidence unless noted)

Nine entries each. Area order is OWA, OWB, OWC, OWD, OWE, Steel, Museum, Underworld, then one unused slot.

| Field | Evidence |
|---|---|
| `pickupsCollectedPerArea` | Sums to `pickupCount` for base game, per-area totals match the `Pickup_` collectibles (36, 37, 37, 26, 39, 24, 23, 25) |
| `catwomanPickupsCollectedPerArea` | 5 in each of 7 areas = 35 in a save with the Catwoman pickups (medium: area order is assumed) |
| `teethCollectedPerArea` | Only the OWD slot is used, 12 |
| `balloonsCollectedPerArea` | OWB and OWC, 12 each |
| `penguinsCollectedPerArea` | Only the Museum slot is used, 12 |
| `camerasBrokenPerArea` | Sums to `numCamerasBroken`, OWD has none |
| `riddlesSolvedPerArea` | 14, 9, 9, 5, 11, 7, 8, 6 = 69 riddle collectibles |

## Fields that turned out not to be padding

These were read as one uint32 "padding" value, but they are the length or count of a string or string array that is empty early in the game. Reading them as a uint32 only worked on saves where they were 0.

| Old name | New name | Seen holding |
|---|---|---|
| `unknown85` | `unknownStrings85` | `Steel_B1`, `Steel_B1_Fascade`, ... (a second list of level names, next to `worldStrings`) |
| `unknown87` | `unknownString87` | `Steel_B1` |
| `unknown92` | `unknownStrings92` | `Frontend.Objectives.CH2_Sub_2_Trail1` (active objectives) |

## Still unknown

* `unknown59`, `unknown60`: the two members before `BrokenGrundyHeadTally` (12 and 12 in later saves). Scroll up in the struct dump to name them.
* `unknownBytes35`, `unknownUints36`, `unknownUints37`: 9 entries each, 0-31 bit mask in places.
* `unknownUints38` to `unknownUints46`: 60 entries each (5 blocks of 12), values 0, 1 or 2. Possibly Riddler's Revenge challenge map states.
* `unknownUints50` (11 entries, grows with progress) and `unknownUints51` (always 0).
* `unknown76`, `unknown77`: 1 in every save seen.
* The nine 4 byte values just before the `-1, -1` marker in segment 3 (`unknownUints160`). They were read as a 3 float array plus a 4 int array, which only works for later saves. Early game values differ (`1, F, 11, 9, 0, 0, 0, 0, -1` right after the Courthouse versus `3, F, 11, 11, 4, 3, 0, 0, 8` in TestSave0), so they are a fixed block of nine and the first value is not a count.
* `progressStrings[0]` (`580/852`) and `[3]` (`x/94`), and what `unknownFloats1[0..14]` are. `unknownFloats1[15..17]` equal the numbers in `progressStrings[1..3]`, but without the Catwoman additions (400, 27, 94 instead of 440, 33, 94).
* The challenge map structs (`unknownStructs`). Their ids look like challenge map ids 11-22, 31-42 and 64-77, the same numbers as the `ChalAvail_N` flag strings.
* Almost the whole tail of segment 3 (about 39 KB in a late save).

## Parser robustness

* `new ArkhamCitySave(save, true)` is a tolerant mode. A parsing error is stored in `parseError` / `parseErrorPosition` instead of being thrown, and every field read before the error keeps its value.
* Checked against 60 saves from the 2025 100% route (intro to Done) plus the test saves and the other uploaded saves. All of them parse cleanly up to `unknownInts140`.
* The block after `unknownInts140` is now decoded for all 135 saves tried (2024/2025 routes, Any%, Glitchless, New Game Minus, NG+, NG+ with Catwoman, personal saves, the test saves). Layout:
  * `unknownBytes143` / `unknownStrings143`: a variable sized region (85 to 135 bytes). It holds small counters and optional strings (`SS_*` side story flags, `OW_*` main mission names, `Riddler_*`). Its end is found by looking for the two 15 entry flag arrays (`unknown155`, `unknown157`) that always sit 26 bytes after the end of the region.
  * `unknown150` (float) to `unknownBitFlags158`: fixed layout. `unknown151` is -1 in most saves but 14, 15 or 0 in early ones.
  * `unknownStrings159A/B`, `unknownInts159C/D`: four lists of equal length (0, 1 or 2 entries so far). The strings are always `assassinBlood` and `Museum`.
  * `unknownBytes159` (17 bytes, always 0), `unknownUints160` (9 words), the two 3 float arrays, two ints, a byte, and two arrays whose count varies (1, 5, 7 or 8 entries).
* The end of segment 3 is a fixed 830 byte block (`footerHeader` 89 bytes, six counted lists of 16 ints in `footerIntLists`, `footerTail` 333 bytes). `unknownBytes170` was wrong: floats 168/169 are followed by this block no matter how long they are.
  * `footerIntLists[0..3]` hold small indices and -1 (a slot table of 16 entries, filled from the front as the game goes on), `[4]` and `[5]` were all zero in every save so far.
  * `footerTail` repeats a 77 byte pattern (the bytes at offsets 549/626, 560/637, 571/648, 725/802, ... vary together), so it is probably an array of four 77 byte records. The Footer bytes tab in the viewer and `dump/footer_summary.txt` list every non-zero offset and its values across saves.
* Segment 3 can appear more than once. After the 830 byte block there is either nothing (normal saves) or another complete copy of segment 3 (starts with the 0x81 header again, ends with its own 830 byte block). New Game Plus / New Game Minus saves have two copies, New Game Plus with Catwoman has three; the last one is always `PDLCIN` / `PDLCIN_B3`. The parser reads them into `additionalSegment3s` (the second copy holds the third). All 137 saves tried end exactly at the end of their last 830 byte block.
* Left undecoded: what the individual bytes inside `unknownBytes143` (apart from the strings), `footerHeader` and `footerTail` mean, and the 17 bytes in `unknownBytes159`.
