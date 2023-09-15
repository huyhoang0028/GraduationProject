using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager Instace {get; private set;}

	public Image playerHpImg;
	public Image endTxt;
	public Sprite pause, timesup;

	public Text coinText;
	public Text TimeText;
	public Text bulletTxt;

	private	void Awake ()
	{
		Instace = this;	
	}

	void Start ()
	{
		PlayerPrefs.SetInt ("bullet", 10);
		SetUIText ("coin");
		SetUIText ("bullet");
	}

	public void SetHealthUI()
	{
		playerHpImg.fillAmount = (GameManager.Instace.player.playerhp) / (GameManager.Instace.player.basichp);
	}

	public void SetUIText(string UIType)
	{
		switch (UIType) {
		case "coin":
			coinText.text = PlayerPrefs.GetInt ("coin").ToString ();
			break;
		case "bullet":
			bulletTxt.text = PlayerPrefs.GetInt ("bullet").ToString ();
			break;
		default:
			break;
		}
	}

	public void CoutDown(int value)
	{
		TimeText.text = value.ToString ();
	}

	public void TimeSup()
	{
		endTxt.sprite = timesup;
	}
}
