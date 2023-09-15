using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBManager : MonoBehaviour {

	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public GameObject DialogUsername;
	public GameObject DialogProfilePic;

	public GameObject ScoreEntryPanel;
	public GameObject ScrollScoreList;
	public Image scrollView;
	public GameObject loseLeaderBoard;
	void Awake()
	{
		FB.Init (SetInit, OnHideUnity);
	}

	void SetInit()
	{

		if (FB.IsLoggedIn) {
			Debug.Log ("FB is logged in");
		} else {
			Debug.Log ("FB is not logged in");
//			FBlogin ();
		}

		DealWithFBMenus (FB.IsLoggedIn);

	}

	void OnHideUnity(bool isGameShown)
	{

		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

	}

	public void FBlogin()
	{

		List<string> permissions = new List<string> ();
		permissions.Add ("public_profile");

		FB.LogInWithReadPermissions (permissions, AuthCallBack);
	}

	void AuthCallBack(IResult result)
	{

		if (result.Error != null) {
			Debug.Log (result.Error);
		} else {
			if (FB.IsLoggedIn) {
				Debug.Log ("FB is logged in");
//				SetScore ();
			} else {
				Debug.Log ("FB is not logged in");
			}

			DealWithFBMenus (FB.IsLoggedIn);
		}

	}

	void DealWithFBMenus(bool isLoggedIn)
	{

		if (isLoggedIn)
		{
			DialogLoggedIn.SetActive(false);
			DialogLoggedOut.SetActive(true);

			FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
			FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

		}
		else
		{
			DialogLoggedIn.SetActive(true);
			DialogLoggedOut.SetActive(false);
		}

	}

	void DisplayUsername(IResult result)
	{

		Text UserName = DialogUsername.GetComponent<Text>();

		if (result.Error == null)
		{

			UserName.text = "Hi there, " + result.ResultDictionary["first_name"];

		}
		else
		{
			Debug.Log(result.Error);
		}

	}


	void DisplayProfilePic(IGraphResult result)
	{
		if (result.Texture != null)
		{
			print("get image");
			Image ProfilePic = DialogProfilePic.GetComponent<Image>();

			ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

		}

	}

	public void LogOut()
	{
		FB.LogOut ();

		DealWithFBMenus (FB.IsLoggedIn);
	}
	//	#endregion

	#region Share
	public void Share()
	{
		if (FB.IsLoggedIn) {
			FB.ShareLink (contentTitle: "Play Monster Truck Games", 
				contentURL: new System.Uri ("https://play.google.com/store/"),
				contentDescription: "http://i.imgur.com/s57sMXx.jpg",
				callback: OnShare);
		} else {
			FBlogin ();
		}
	}


	private void OnShare(IShareResult resault)
	{
		if (resault.Cancelled || !string.IsNullOrEmpty (resault.Error)) {
			Debug.Log ("ShareLink Error" + resault.Error);
		} else if (!string.IsNullOrEmpty (resault.PostId)) {
			Debug.Log (resault.PostId);
		} else {
			Debug.Log ("Share Success");
		}
	}

	public void FacebookInVite()
	{
		FB.Mobile.AppInvite (new System.Uri ("https://fb.me/"));
	}

	#endregion
	///<Summary>
	///Leadeeboard Facebook
	///</Summary>
	/// All codes realated to FB Score API


	public void QueryScore()
	{
		loseLeaderBoard.SetActive(true);
		FB.API("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, getScoreCallBack);
	}

	void getScoreCallBack(IResult resault)
	{
		IDictionary<string, object> data = resault.ResultDictionary;
		List<object> scoreList = (List<object>)data["data"];


		foreach (object obj in scoreList)
		{
			var entry = (Dictionary<string, object>)obj;
			var user = (Dictionary<string, object>)entry["user"];

			Debug.Log(user["name"].ToString() + " , " + entry["score"].ToString());

			GameObject scorePanel;
			scorePanel = Instantiate(ScoreEntryPanel) as GameObject;
			scorePanel.transform.SetParent(ScrollScoreList.transform, false);

			Transform Fname = scorePanel.transform.Find("FriendsName");
			Transform Fscore = scorePanel.transform.Find("FriendsScore");
			Transform Favatar = scorePanel.transform.Find("FriendsAvatar");

			Text FName = Fname.GetComponent<Text>();
			Text FScore = Fscore.GetComponent<Text>();
			Image FAvatar = Favatar.GetComponent<Image>();

			FName.text = user["name"].ToString();
			FScore.text = entry["score"].ToString();

			FB.API(user["id"].ToString() + "/picture?width = 1280&height=120", HttpMethod.GET, delegate (IGraphResult result)
			{
				if (result.Error != null)
				{
					Debug.Log(result.RawResult);
				}
				else
				{
					FAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 120, 120), new Vector2(0, 0));
				}
			});

			scrollView.enabled = true;

		}
	}

	public void SetScore()
	{
		var scoreData = new Dictionary<string, string>();
		scoreData["score"] = PlayerPrefs.GetInt("best").ToString();
		FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult resault)
		{
			Debug.Log("Score Subbmited Successful" + resault.RawResult);
		}, scoreData);
	}

	public void CLoseLeaderBoard(GameObject leaderBoadr)
	{
		leaderBoadr.SetActive(false);
	}

}