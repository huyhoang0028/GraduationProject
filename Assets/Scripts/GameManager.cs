using UnityEngine;
using Tools;
public class GameManager : MonoBehaviour {
	public static GameManager Instace{ get; private set;}
	public Level[] level;

	public bool isDiamondCollected;
	public bool isGameComplete;
	public GameObject diamondUI;

	public Player player;
	public PlayerInput playerInput;
	public GameObject[] playerAvatar;

	public float[] EnemyDamePlayer, PlayerDameEnemy;
	public float[] PlayerHealing;

	public Animator uiAnim;
	private bool isGamePause;
	public GameObject pauseText;
	public GameObject txtGameOver;
	public GameObject resumeBtn;
	public GameObject coutDownObj;
	public GameObject endPanel;


	private	void Awake ()
	{
		Instace = this;	
	}
	void Start ()
	{
		player.gameObject.transform.position = level [Menu.selectedLevel].playerPos;	

		if (level [Menu.selectedLevel].coutDownMode) {
			InvokeRepeating ("CountDown", 0, 1);
		} else {
			coutDownObj.SetActive (false);
		}
	}

	public void InGameFuntion(int index)
	{
		switch (index) {
		case 0:
			if (!isGamePause) {
				isGamePause = true;
				txtGameOver.gameObject.SetActive (false);
				Time.timeScale = 0;
				endPanel.SetActive (true);
//				uiAnim.Play ("pauseIn");
				AdsControl.Instance.showAds ();
			} else {
				return;
			}
			break;
		case 1:
			Time.timeScale = 1;
			txtGameOver.gameObject.SetActive (false);
			Application.LoadLevel (0);
			break;
			print (1);
		case 2:
			Time.timeScale = 1;
//			AdsControl.Instance.showAds ();
//			print(2);
			txtGameOver.gameObject.SetActive (false);
			if (!isGameComplete) {
				isGamePause = false;
				endPanel.SetActive (false);
//				uiAnim.Play ("pauseOut");
			} else {
				Menu.selectedLevel += 1;
				Application.LoadLevel (Application.loadedLevel);
			}
			break;
		case 3:
			Time.timeScale = 1;
			txtGameOver.gameObject.SetActive (false);
			Application.LoadLevel (Application.loadedLevel);
//			print (3);
			break;
		case 4:
//			AdsControl.Instance.showAds ();
			pauseText.gameObject.SetActive (false);
			Time.timeScale = 0;
//			print (4);
			endPanel.SetActive(true);
//			uiAnim.Play ("pauseIn");
			break;
		case 5:
			resumeBtn.SetActive (false);
			Time.timeScale = 0;
			endPanel.SetActive (true);
//			uiAnim.Play ("pauseIn");
			txtGameOver.gameObject.SetActive (false);
			print (5);
			break;
		case 6:
			txtGameOver.gameObject.SetActive (true);
			pauseText.gameObject.SetActive (false);
			resumeBtn.SetActive (false);
			Time.timeScale = 0;
//			uiAnim.Play ("pauseIn");
			endPanel.SetActive(true);
			AdsControl.Instance.showAds ();
			print (6);
			break;
		}
	}

	public void CountDown()
	{
		level [Menu.selectedLevel].levelTime -= 1;
		//MusicAndSound.INSTANCE.PlaySoundEffect (0);
		UIManager.Instace.CoutDown (level [Menu.selectedLevel].levelTime);
		if (level [Menu.selectedLevel].levelTime == 0) {
			UIManager.Instace.TimeSup ();
			InGameFuntion (5);
		}
	}
}
[System.Serializable]
public class Level
{
	public Vector3 camPos_maxXY;
	public Vector3 camPos_minXY;
	public Vector3 playerPos;
	public bool coutDownMode;
	public int levelTime;
}

public interface IItem
{
	void OnTouchPlayer ();
	void OntouchSword ();
}
public interface IDame
{
	void Dame ();
}

public interface IHeal
{
    void Heal ();
}
