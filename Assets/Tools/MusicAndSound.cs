using UnityEngine;


namespace Tools
{
	public class MusicAndSound : MonoBehaviour {
		//Sigleton
		public static MusicAndSound INSTANCE{ get; private set;}

		public AudioSource musicSource;
		public AudioSource soundSource;

		public enum BackgroundMusic
		{
			singleBacground,
			multipleBackground
		}

		public BackgroundMusic _BackgroundMusic;

		[Header("If BackgroundMusic is singleBacground => Drag Clip Here")]
		public AudioClip backgroundClip;
		[Header("If BackgroundMusic is multipleBackground => Drag Clips Here")]
		public AudioClip[] bgMuscicLip;

		[Header("Drag sound effect in your game to this array")]
		public AudioClip[] soundEffect;

		private	void Awake ()
		{
			if (INSTANCE == null) {
				INSTANCE = this;
				DontDestroyOnLoad (this.gameObject);
			} else {
				Destroy (gameObject);
			}
		}


		void Start ()
		{
			PlayBackgroundMusic ();
		}


		///<Summary>
		///Call this function to play background music
		///</Summary>
		private void PlayBackgroundMusic()
		{
			// if the music source is null i do nothing
			if (musicSource == null) {
				Debug.Log ("Music Source Is Now Null");
				return;
			}

			// i play the background music accord to the music bg mode
			switch (_BackgroundMusic) {
			case BackgroundMusic.singleBacground:
				// if there is no clip, i do no thing
				if (backgroundClip == null) {
					Debug.Log ("Background music clip is now null");
					return;
				}
				musicSource.clip = backgroundClip;
				musicSource.Play ();
				musicSource.loop = true;

				break;
			
			case BackgroundMusic.multipleBackground:
				// if there are no clips, i do no thing
				if (bgMuscicLip.Length == 0) {
					Debug.Log ("Background music clip array is now null, drag clips in to the array and cont");
					return;
				}
				musicSource.clip = bgMuscicLip [Random.Range (0, bgMuscicLip.Length)];
				musicSource.Play ();
				musicSource.loop = true;

				break;
			}
		}


		///<Summary>
		///Call this function to play soundEffect
		///</Summary>
		public void PlaySoundEffect(int effectIndex)
		{
			// if the sound source is null i do nothing
			if (soundSource == null) {
				Debug.Log ("Sound Source Is Now Null");
				return;
			}
			if (soundEffect.Length == 0) {
				Debug.Log ("There is no clip in soundeffec array");
				return;
			}

			soundSource.PlayOneShot (soundEffect [effectIndex]);
		}
	}
}
