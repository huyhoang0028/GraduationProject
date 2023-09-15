using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
	public class MusicAndSountSetting : MonoBehaviour {
		[Header("Drag Music And Sound Image Here")]
		public Image musicImg;
		public Image soundImg;

		[Header("Drag Music And Sound Sprite ON and OFF Here")]
		public Sprite musicOn, musicOff;
		public Sprite soundOn, soundOff;

		void Start ()
		{
			CheckMusic ();
			CheckSound ();
		}

		//Set music and sound 
		public void SetMusicAndSound(string musicORsound)
		{
			if (musicORsound == "music") {
				if (PlayerPrefs.GetInt ("music") == 0) {
					PlayerPrefs.SetInt ("music", 1);
				} else if (PlayerPrefs.GetInt ("music") == 1) {
					PlayerPrefs.SetInt ("music", 0);
				}

				CheckMusic ();
			}
			if (musicORsound == "sound") {
				if (PlayerPrefs.GetInt ("sound") == 0) {
					PlayerPrefs.SetInt ("sound", 1);
				} else if (PlayerPrefs.GetInt ("sound") == 1) {
					PlayerPrefs.SetInt ("sound", 0);
				}

				CheckSound ();
			}
		}

		//check sound and musing setting
		private void CheckMusic()
		{
			if (MusicAndSound.INSTANCE == null) {
				Debug.Log ("MusicAndSound Reference is Null");
				return;
			}

			if (PlayerPrefs.GetInt ("music") == 0) {
				if (MusicAndSound.INSTANCE.musicSource == null) {
					Debug.Log ("Music Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.musicSource.volume = 1;

				if (musicImg == null) {
					Debug.Log ("Music Image Is Null, Drag Music Image And Continous");
					return;
				}
				if (musicOn == null) {
					Debug.Log ("Music Sprite On Is Null, Drag Music Image And Continous");
					return;
				}
				musicImg.sprite = musicOn;
			} else if (PlayerPrefs.GetInt ("music") == 1) {
				if (MusicAndSound.INSTANCE.musicSource == null) {
					Debug.Log ("Music Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.musicSource.volume = 0;

				if (musicImg == null) {
					Debug.Log ("Music Image Is Null, Drag Music Image And Continous");
					return;
				}

				if (musicOff == null) {
					Debug.Log ("Music Sprite Off Is Null, Drag Music Image And Continous");
					return;
				}
				musicImg.sprite = musicOff;
			}

			if (PlayerPrefs.GetInt ("sound") == 0) {
				if (MusicAndSound.INSTANCE.soundSource == null) {
					Debug.Log ("Sound Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.soundSource.volume = 1;

				if (soundImg == null) {
					Debug.Log ("Sound Image Is Null, Drag Music Image And Continous");
					return;
				}

				if (soundOn == null) {
					Debug.Log ("Sound Sprite On Is Null, Drag Music Image And Continous");
					return;
				}

				soundImg.sprite = soundOn;
			} else if (PlayerPrefs.GetInt ("sound") == 1) {
				if (MusicAndSound.INSTANCE.soundSource == null) {
					Debug.Log ("Sound Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.soundSource.volume = 0;

				if (soundImg == null) {
					Debug.Log ("Sound Image Is Null, Drag Music Image And Continous");
					return;
				}

				if (soundOn == null) {
					Debug.Log ("Sound Sprite Off Is Null, Drag Music Image And Continous");
					return;
				}
				soundImg.sprite = soundOff;
			}
		}

		private void CheckSound()
		{
			if (MusicAndSound.INSTANCE == null) {
				Debug.Log ("MusicAndSound Reference is Null");
				return;
			}

			if (PlayerPrefs.GetInt ("sound") == 0) {
				if (MusicAndSound.INSTANCE.soundSource == null) {
					Debug.Log ("Sound Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.soundSource.volume = 1;

				if (soundImg == null) {
					Debug.Log ("Sound Image Is Null, Drag Music Image And Continous");
					return;
				}

				if (soundOn == null) {
					Debug.Log ("Sound Sprite On Is Null, Drag Music Image And Continous");
					return;
				}

				soundImg.sprite = soundOn;
			} else if (PlayerPrefs.GetInt ("sound") == 1) {
				if (MusicAndSound.INSTANCE.soundSource == null) {
					Debug.Log ("Sound Source Is Now Null");
					return;
				}
				MusicAndSound.INSTANCE.soundSource.volume = 0;

				if (soundImg == null) {
					Debug.Log ("Sound Image Is Null, Drag Music Image And Continous");
					return;
				}

				if (soundOn == null) {
					Debug.Log ("Sound Sprite Off Is Null, Drag Music Image And Continous");
					return;
				}
				soundImg.sprite = soundOff;
			}
		}
	}
}
