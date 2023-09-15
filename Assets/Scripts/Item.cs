using UnityEngine;
using Tools;

public class Item : MonoBehaviour,IItem {

	public enum ITEM_TYPE
	{
		COIN,
		DOOR,
		TREASURE_BOX,
		FOOD_BOX,
		RICE_BOWL,
		SMOKE,
		COIN_BOX,
		DIAMOND,
		SWORD,
		SWORD_BULLET,
		SHEILD
	}

	public ITEM_TYPE _ITEM_TYPE;

	[Header("Finish Door")]
	public GameObject openDoor;
	public GameObject playerAvatar;
	private bool isDoorClose;

	[Header("Treasure Box")]
	public SpriteRenderer spr;
	public Sprite treasureBoxOpen;
	public GameObject diamond;

	[Header("Food & Coin Box")]
	public GameObject smoke;

	[Header("Smoke")]
	public GameObject riceBowl;
	public GameObject coin;
	public GameObject sheild;
	public GameObject swordBullet;
	public enum ItemToInstantiate
	{
		RICE_BOWL,
		COIN,
		SWORD,
		SHEILD
	}

	public ItemToInstantiate _ItemToInstantiate;

	[Header("Item's Sprite Renderer")]
	public SpriteRenderer coinSpr;
	public SpriteRenderer riceBowSpr;
	public SpriteRenderer diamondSpr;

	private bool isGot;

	[Header("Sword")]
	public int swordDirection;
	public float swordSpeed;
	public float swordRotate;
	Plane[] planes;
	public Collider2D col;


	private	void Awake ()
	{
		isGot = this.isGot;	
	}

	void OnEnable ()
	{
		InvokeRepeating ("RotateSword", 0, 0.1f);
	}

	void Update ()
	{
		switch (_ITEM_TYPE) {
		case ITEM_TYPE.COIN:
			if (this.isGot) {
				coinSpr.color = new Vector4 (1, 1, 1, coinSpr.color.a - 0.05f);
				transform.position = new Vector3 (transform.position.x - 0.05f, transform.position.y + 0.05f);
			}
				if (coinSpr.color.a <= 0)
					Destroy (gameObject);
			break;
		case ITEM_TYPE.DIAMOND:
			if (this.isGot) {
				diamondSpr.color = new Vector4 (1, 1, 1, diamondSpr.color.a - 0.05f);
				transform.position = new Vector3 (transform.position.x - 0.05f, transform.position.y + 0.05f);
			}
			if (diamondSpr.color.a <= 0)
				Destroy (gameObject);
			break;
		case ITEM_TYPE.RICE_BOWL:
			if (this.isGot) {
				riceBowSpr.color = new Vector4 (1, 1, 1, riceBowSpr.color.a - 0.05f);
				transform.position = new Vector3 (transform.position.x - 0.05f, transform.position.y + 0.05f);
			}
			if (riceBowSpr.color.a <= 0)
				Destroy (gameObject);
			break;
		case ITEM_TYPE.SWORD:
			if (!IsSeen ()) {
				gameObject.SetActive (false);
			}
			break;
		}


	}

	public void OnTouchPlayer()
	{
		switch (_ITEM_TYPE) {

		case ITEM_TYPE.COIN:
			this.isGot = true;
			PlayerPrefs.SetInt ("coin", PlayerPrefs.GetInt ("coin") + 1);
			UIManager.Instace.SetUIText ("coin");
			MusicAndSound.INSTANCE.PlaySoundEffect (2);
			break;

		case ITEM_TYPE.DOOR:
			if (GameManager.Instace.isDiamondCollected) {
				GameManager.Instace.isGameComplete = true;
				GetComponent<Collider2D> ().enabled = false;
				GameManager.Instace.player.gameObject.SetActive(false);
				playerAvatar = Instantiate (GameManager.Instace.playerAvatar[Menu.selectedCharacter], new Vector3 (transform.position.x - 0.5f, transform.position.y-1.6f, 0), Quaternion.identity);
				InvokeRepeating ("OpenDoor", 0, 0.1f);
				MusicAndSound.INSTANCE.PlaySoundEffect (4);
			} else {
				print ("You Must Collect Diamond");

			}
			break;

		case ITEM_TYPE.RICE_BOWL:
                this.isGot = true;
                var healing = col.GetComponent<IHeal>();
                if (healing != null)
                {
                    healing.Heal(); // Gọi phương thức Heal() của player
                }
                MusicAndSound.INSTANCE.PlaySoundEffect(5);
                break;

            case ITEM_TYPE.DIAMOND:
			this.isGot = true;
			GameManager.Instace.isDiamondCollected = true;
			GameManager.Instace.diamondUI.SetActive (true);
			MusicAndSound.INSTANCE.PlaySoundEffect (1);
			break;
		case ITEM_TYPE.SHEILD:
			GameManager.Instace.player.isProtect = true;
			gameObject.SetActive (false);
			break;
		case ITEM_TYPE.SWORD_BULLET:
			PlayerPrefs.SetInt ("bullet", PlayerPrefs.GetInt ("bullet") + 5);
			UIManager.Instace.SetUIText ("bullet");
			gameObject.SetActive (false);
			break;
		}
	}
	public void OntouchSword()
	{
		switch (_ITEM_TYPE) {
		case ITEM_TYPE.TREASURE_BOX:
			spr.sprite = treasureBoxOpen;
			diamond.SetActive (true);
			break;

		case ITEM_TYPE.FOOD_BOX:
			var Smoke = Instantiate (smoke, new Vector3(transform.position.x,transform.position.y-0.5f,0), Quaternion.identity);
			Smoke.GetComponent<Item> ()._ItemToInstantiate = ItemToInstantiate.RICE_BOWL;
			gameObject.SetActive (false);
			break;
		case ITEM_TYPE.COIN_BOX:
			var Smokee = Instantiate (smoke, new Vector3(transform.position.x,transform.position.y-0.35f,0), Quaternion.identity);
			switch (Random.Range(0,3)) {
			case 0:
				Smokee.GetComponent<Item> ()._ItemToInstantiate = ItemToInstantiate.COIN;
				break;
			case 1:
				Smokee.GetComponent<Item> ()._ItemToInstantiate = ItemToInstantiate.SHEILD;
				break;
			case 2:
				Smokee.GetComponent<Item> ()._ItemToInstantiate = ItemToInstantiate.SWORD;
				break;
			}


			gameObject.SetActive (false);
			break;
		}
	}

	public void OpenDoor()
	{
		openDoor.transform.localPosition = new Vector3 (openDoor.transform.localPosition.x, openDoor.transform.localPosition.y + 0.1f, 0);
		if (openDoor.transform.localPosition.y >= 3.2f) {
			CancelInvoke ("OpenDoor");
			InvokeRepeating ("CloseDoor", 0, 0.1f);

			var spr = playerAvatar.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
//			print (Menu.selectedCharacter);
//			spr.sprite = GameManager.Instace.playerAvatarSprite [Menu.selectedCharacter];
			spr.sortingLayerName = "Foreground";
			spr.sortingOrder = 1;
		}
	}
	public void CloseDoor()
	{
		openDoor.transform.localPosition = new Vector3 (openDoor.transform.localPosition.x, openDoor.transform.localPosition.y - 0.1f, 0);
		if (openDoor.transform.localPosition.y <= 1f) {
			CancelInvoke ("CloseDoor");
			GameManager.Instace.InGameFuntion (4);
			if (!isDoorClose) {
				isDoorClose = true;
				MusicAndSound.INSTANCE.PlaySoundEffect (8);

				if (Menu.selectedLevel == PlayerPrefs.GetInt ("level")) {
					PlayerPrefs.SetInt ("level", PlayerPrefs.GetInt ("level") + 1);
				}
			}
		}
	}
	private void RiceBowl()
	{
		switch (_ItemToInstantiate) {
		case ItemToInstantiate.RICE_BOWL:
			Instantiate (riceBowl, transform.position , Quaternion.identity);
			break;
		case ItemToInstantiate.COIN:
			Instantiate (coin, new Vector3(transform.position.x,transform.position.y+0.2f,0), Quaternion.identity);
			break;
		case ItemToInstantiate.SHEILD:
			Instantiate (sheild, new Vector3(transform.position.x,transform.position.y+0.2f,0), Quaternion.identity);
			break;
		case ItemToInstantiate.SWORD:
			Instantiate (swordBullet, new Vector3(transform.position.x,transform.position.y+0.2f,0), Quaternion.identity);
			break;
		}
		MusicAndSound.INSTANCE.PlaySoundEffect (7);
		gameObject.SetActive (false);
	}

	private void Disappear()
	{
		switch (_ITEM_TYPE) {
		case ITEM_TYPE.COIN:
			coinSpr.color = new Vector4 (1, 1, 1, coinSpr.color.a - 0.75f);
//			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y + 0.5f);
			if (coinSpr.color.a <= 0)
				CancelInvoke ("Disappear");
			break;
		case ITEM_TYPE.DIAMOND:
			diamondSpr.color = new Vector4 (1,1,1,diamondSpr.color.a-0.75f);
//			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y + 0.5f);
			if (diamondSpr.color.a <= 0)
				CancelInvoke ("Disappear");
			break;
		case ITEM_TYPE.RICE_BOWL:
			riceBowSpr.color = new Vector4 (1,1,1,riceBowSpr.color.a-0.75f);
//			transform.position = new Vector3 (transform.position.x - 0.25f, transform.position.y + 0.5f);
			if (riceBowSpr.color.a <= 0)
				CancelInvoke ("Disappear");
			break;
		}
	}


	private void RotateSword()
	{
		transform.position = new Vector3 (transform.position.x + (swordSpeed * swordDirection), transform.position.y, 0);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + swordRotate);
	}

	private bool IsSeen ()
	{
		planes = GeometryUtility.CalculateFrustumPlanes (Camera.main);
		if (GeometryUtility.TestPlanesAABB (planes, col.bounds)) {
			return true;
		} else {
			return false;
		}
	}

	private void OnDisable()
	{
		if (_ITEM_TYPE == ITEM_TYPE.SWORD) {
			CancelInvoke ("RotateSword");
		}
	}


}