# MLTDMusicExperiments
My music experiment/s for [THE iDOLM@STER Million Live! Theater Days](https://millionlive.idolmaster.jp/theaterdays/).
Supposedly plural, but I have only done one so far.

## MirishitaMusicPlayer
Plays songs directly from the game assets via [AssetStudio](https://github.com/Perfare/AssetStudio).
Supports per-idol singing (歌い分け / utaiwake) songs.

### Usage
```
MirishitaMusicPlayer.exe <path-to-assets>
```
Required assets include:
* `scrobj`: Scenario files used for "mute" events (which idols should be singing), facial expressions, lipsync, and timing.
* `song3`: Files that contain the CRIWARE audio banks for the actual audio.

You can either grab the assets from your device and painstakingly rename them yourself,
or you can download them with the **Manifest Tools** project from [OpenMLTD](https://github.com/OpenMLTD/MLTDTools).

For songs with swappable singers, there will be a prompt for you to select an order:
```
Available singers:
 0 Iori
 1 Mirai
 2 Miya
 3 Momoko
 4 Kaori

Select in order (5 max): 0 3 2 4 1
```
Pictured above: default order for `sjewel` (シークレットジュエル　～魅惑の金剛石～).

When only 1 singer is selected, behavior will be identical to a solo. Otherwise, any count less than the available ones will result in no singing for unused positions.
Let's say only two singers were selected. Iori is in position 0, and Mirai in position 1. During position 2-4's parts, only the background music will be heard.

NOTE: Putting the same idol on different positions is not supported (for now at least).

Visualization for mute events, facial expression, lipsync, and lyrics will be shown during playback.
```
 19.1206s elapsed

 [1]  [ ]  [ ]  [ ]  [ ]

   ___        ___
 --              --
  ---          ---
 | O |        | O |    Expression ID: 0
 -----        -----    Eye close: 0


     \________/
                       Sound: -, ID: 54




 飛び込んで 今宵もミッション
```
If used in the Windows Terminal, the window must be resized to fit the contents of the output.
Otherwise, it will crash with an out-of-range exception due to the buffer size being too small.

### Keyboard shortcuts
* `V` to mute the voices
* `B` to mute the background music
* `R` to reset to the beginning
* `Space` to pause
* `Left Arrow` to rewind 3 seconds
* `Right Arrow` to fast forward 3 seconds

## Credits
* The [vgmstream](https://github.com/vgmstream/vgmstream) project for the CRIWARE UTF and ACB/AWB reader code.
* The [HCADecoder](https://github.com/Nyagamon/HCADecoder) project and by extension, vgmstream, for the HCA decoder code.
* The [AssetStudio](https://github.com/Perfare/AssetStudio) library for reading Unity AssetBundles.
* BANDAI NAMCO for the iDOLM@STER series and for giving me SSRs.
