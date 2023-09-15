
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {
	public static Menu ins;
	public static int selectedLevel;
	public static int selectedCharacter;
	public enum LEVEL_TYPE
	{
		SINGLE_LEVEL,
		MULTIPLE_LEVEL
	}
	public LEVEL_TYPE _LevelType;
	public enum LEVEL_IMAGE_TYPE
	{
		LEVEL_IMAGE_WITH_INDEX,
		LEVEL_IMANE_WITHOUT_INDEX
	}

	public LEVEL_IMAGE_TYPE _LEVEL_IMAGE_TYPE;

	[Header("Level Number Index, If LEVEL_IMAGE_WITH_INDEX => Ignore This")]
	[SerializeField] Text[] _LevelButtonText;
	[Header("Variables For Multiple Level, If Single Level => Ignore Theme")]
	[SerializeField] Image[] _LevelButtonImage;
	[SerializeField] Sprite _levelLockSprite;
	[SerializeField] Sprite _levelUnlockedSprite;
	[SerializeField] Button[] _LevelButton;
	[SerializeField] GameObject _MainMenuPanel;
	[SerializeField] GameObject _LevelSelectorPanel;
	[SerializeField] GameObject _ShopPanel;
	[SerializeField] GameObject _CharacterSelector;
	[SerializeField] int[] charPrice;
	[SerializeField] Text[] charPriceTxt;



	void Start ()
	{
		ins = this;
		PlayerPrefs.SetInt("level", 9);
		PlayerPrefs.SetInt("coin", 1000);
//		PlayerPrefs.DeleteAll();
		if (_LevelType == LEVEL_TYPE.MULTIPLE_LEVEL) {
			LEVELSYSTEM ();
		}	

		PlayerPrefs.SetInt ("character0", 1);

		CheckCharacterUnlock ();

		SetMoneyText ();

		SelectChar (0);
	}

	//IF Level type is multiple level - i check locked and unlocked level
	public void LEVELSYSTEM()
	{
		for (int i = 0; i < _LevelButtonImage.Length; i++) {
			//IF LEVEL_IMANE_WITHOUT_INDEX = > Set Level Number Text
			if (_LEVEL_IMAGE_TYPE == LEVEL_IMAGE_TYPE.LEVEL_IMANE_WITHOUT_INDEX) {
				if (_LevelButtonText.Length == 0) {
					Debug.Log ("Level Text Array Is Now Null, Drag Them To Text Array Fist");
					return;
				}
				_LevelButtonText [i].text = (i + 1).ToString ();
			}
			//I Check levels, which were unlocked, and which are locking
			if (i > PlayerPrefs.GetInt ("level")) {
				if (_LevelButtonImage.Length == 0) {
					Debug.Log ("Level Image Array Is Now Null, Drag Them To Image Array Fist");
					return;
				}
				_LevelButtonImage [i].sprite = _levelLockSprite;
				if (_LevelButton.Length == 0) {
					Debug.Log ("Level Button Array Is Now Null, Drag Them To Button Array Fist");
					return;
				}
				_LevelButton [i].interactable = false;
				if (_LEVEL_IMAGE_TYPE == LEVEL_IMAGE_TYPE.LEVEL_IMANE_WITHOUT_INDEX) 
					_LevelButtonText [i].gameObject.SetActive (false);
			}
		}
	}

	public void SelectLevel(int index)
	{
		selectedLevel = index;
		_CharacterSelector.SetActive (true);
		_LevelSelectorPanel.SetActive (false);
	}

	[Header("Character")]
	[SerializeField] Image[] lockStt,charBg;
	[SerializeField] Sprite lockBg, unlockBg,charLockBg, charUnlockBg;
	[SerializeField] GameObject[] selected;
	private void CheckCharacterUnlock()
	{
		for (int i = 0; i < lockStt.Length; i++) {
			if (PlayerPrefs.GetInt ("character" + i.ToString ()) == 1) {
				lockStt [i].sprite = unlockBg;
				lockStt [i].SetNativeSize ();
				charPriceTxt [i].gameObject.transform.parent.gameObject.SetActive (false);
			} else {
				lockStt [i].sprite = lockBg;
				lockStt [i].SetNativeSize ();
			}

			charPriceTxt [i].text = charPrice [i].ToString ();
		}
	}

	public void SelectChar(int index)
	{
		if (PlayerPrefs.GetInt ("character" + index.ToString ()) == 1) {
			for (int i = 0; i < selected.Length; i++) {
				if (index == i) {
					selectedCharacter = index;
					selected [i].SetActive (true);
					charBg [i].sprite = charUnlockBg;
				} else {
					selected [i].SetActive (false);
					charBg [i].sprite = charLockBg;
				}
			}
		} else {
			if (PlayerPrefs.GetInt ("coin") > charPrice [index]) {
				PlayerPrefs.SetInt ("character" + index.ToString (), 1);
				PlayerPrefs.SetInt ("coin", PlayerPrefs.GetInt ("coin") - charPrice [index]);
				CheckCharacterUnlock ();
				SetMoneyText ();
			} else {
				print ("Not Enough Money");
				//_ShopPanel.SetActive (true);
			}
		}
	}

	public void PlayBack(int index)
	{
		switch (index) {
		case 0:
			string sceneName = "Level" + (selectedLevel + 1);
			SceneManager.LoadScene(sceneName);
			//SceneManager.LoadScene("Level1");
			//SceneManager.LoadScene("Game");
			break;
		case 1:
			_CharacterSelector.SetActive (false);
			_LevelSelectorPanel.SetActive (true);
			break;
		case 2:
			_LevelSelectorPanel.SetActive (false);
			_MainMenuPanel.SetActive (true);
			break;
		case 3:
			_LevelSelectorPanel.SetActive (true);
			_MainMenuPanel.SetActive (false);
			break;
		}
	}

	[SerializeField] Text moneyText;
	public void SetMoneyText()
	{
		moneyText.text = PlayerPrefs.GetInt ("coin").ToString ();
	}
	public void CloseRewardVideoPanel()
	{
		_ShopPanel.SetActive (false);
	}
	public void OpenShop()
	{
		_ShopPanel.SetActive (true);
	}
	public void ViewRewardVideo()
	{
		AdsControl.Instance.ShowRewardVideo ();
	}
}
