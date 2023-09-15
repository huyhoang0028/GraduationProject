using UnityEngine;
using Tools;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
	public enum CONTROL_TYPE
	{
		MANUAL,
		AI
	}

	public enum INPUT_METHOD
	{
		KEYBOARD,
		TOUCH_SCREEN
	}

	public INPUT_METHOD _INPUT_METHOD;

	public CONTROL_TYPE _CONTROL_TYPE;

    private Player player;

	[Header ("Enemy Target")]
	public GameObject target;
	public Animator anim;
	private int enemyIndex;
	public bool isDead;
	public float enemySpeed;

	public int HorizontalDir, VerticalDir;
	Vector2 directionalInput;

	private bool stop;



    private void Start()
    {
        player = GetComponent<Player>();
		if (_CONTROL_TYPE == CONTROL_TYPE.AI) {
			target = GameManager.Instace.player.gameObject;
		}
		enemyIndex = gameObject.GetComponent<Player> ().enemyType;
    }

    private void Update()
    {
		if (moveRight) {
			HorizontalDir = 1;
		} else if (moveLeft) {
			HorizontalDir = -1;
		} else {
			HorizontalDir = 0;
		}
		if (moveUp) {
			VerticalDir = 1;
		} else if (moveDown) {
			VerticalDir = -1;
		} else {
			VerticalDir = 0;
		}
			
		switch (_CONTROL_TYPE) {
		case CONTROL_TYPE.MANUAL:
			switch (_INPUT_METHOD) {
			case INPUT_METHOD.KEYBOARD:
				directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
				break;
			case INPUT_METHOD.TOUCH_SCREEN:
				directionalInput = new Vector2 (HorizontalDir, VerticalDir);
				break;
			}

			player.SetDirectionalInput (directionalInput);
			//---------------Player Direction---------------------------------------------
			if (directionalInput.x > 0) {
				player.AnimMove ();
				transform.localScale = new Vector3 (1, 1, 1);
			} else if (directionalInput.x < 0) {
				player.AnimMove ();
				transform.localScale = new Vector3 (-1, 1, 1);
			}


			if (Input.GetButtonDown ("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				player.OnJumpInputDown ();
			}

			if (Input.GetButtonUp ("Jump")) {
				player.OnJumpInputUp ();
			}
				

//			if (Input.GetMouseButtonDown (0)) {
//				MusicAndSound.INSTANCE.PlaySoundEffect (3);
//				player.AnimAttack ();
//			}

//			if (Input.GetMouseButtonDown (1)) {
//				if (PlayerPrefs.GetInt ("bullet") > 0) {
//					PlayerPrefs.SetInt ("bullet", PlayerPrefs.GetInt ("bullet") - 1);
//					UIManager.Instace.SetUIText ("bullet");
//
//					var sword = ObjectPooling.INSTANCE.GetObjectFromPoolList (ObjectPooling.INSTANCE.swordList, new Vector3 (transform.position.x, transform.position.y + 1f, 0));
//					if (transform.localScale.x > 0) {
//						sword.GetComponent<Item> ().swordDirection = 1;
//					} else if (transform.localScale.x < 0) {
//						sword.GetComponent<Item> ().swordDirection = -1;
//					}
//
//				}
//			}

			if (!IsEnemySeen()) {
				if (!stop) {
					stop = true;
					print ("out");
					GameManager.Instace.InGameFuntion (6);
				}
			}
			break;
		case CONTROL_TYPE.AI:

			if (IsEnemySeen ()) {
				if (!GameManager.Instace.isGameComplete) {
					if (transform.position.x > target.transform.position.x + 1) {
						anim.enabled = true;
						transform.localScale = new Vector3 (1, 1, 1);
						transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.transform.position.x + 1, transform.position.y, 0), Time.deltaTime * enemySpeed);
					} else if (transform.position.x < target.transform.position.x - 1) {
						anim.enabled = true;
						transform.localScale = new Vector3 (-1, 1, 1);
						transform.position = Vector3.MoveTowards (transform.position, new Vector3 (target.transform.position.x - 1, transform.position.y, 0), Time.deltaTime * enemySpeed);
					} else {
						if (!isDead) {
							if (Mathf.Abs (transform.position.y - GameManager.Instace.player.gameObject.transform.position.y) < 0.1f) {
								anim.enabled = true;
								anim.Play ("enemy_" + enemyIndex.ToString () + "_attack");
							} else {
								anim.Play ("enemy_" + enemyIndex.ToString () + "_idle");
							}
						}
					}
				}
			}
			break;
		}




    }

	Plane[] planes;
	public Collider2D col;
	private bool IsEnemySeen ()
	{
		planes = GeometryUtility.CalculateFrustumPlanes (Camera.main);
		if (GeometryUtility.TestPlanesAABB (planes, col.bounds)) {
			return true;
		} else {
			return false;
		}
	}

	///<Summary>
	///Mobile Input
	///</Summary>
	private bool moveRight,moveLeft,moveUp,moveDown;
	private bool shot,jump;
	public void Move(int dir)
	{
		switch (dir) {
		case 0:
			moveRight = true;
			break;
		case 1:
			moveLeft = true;
			break;
		case 2:
			moveUp = true;
			break;
		case 3:
			moveDown = true;
			break;
		}
	}

	public void StopMove(int dir)
	{
		switch (dir) {
		case 0:
			moveRight = false;
			break;
		case 1:
			moveLeft = false;
			break;
		case 2:
			moveUp = false;
			break;
		case 3:
			moveDown = false;
			break;
		}
	}

	public void StartJump()
	{
		player.OnJumpInputDown ();
	}
	public void StopJump()
	{
		player.OnJumpInputUp ();
	}
	public void Attack()
	{
		//MusicAndSound.INSTANCE.PlaySoundEffect (3);
		player.AnimAttack ();
	}
	public void ShotSword()
	{
		if (PlayerPrefs.GetInt ("bullet") > 0) {
			PlayerPrefs.SetInt ("bullet", PlayerPrefs.GetInt ("bullet") - 1);
			UIManager.Instace.SetUIText ("bullet");

			var sword = ObjectPooling.INSTANCE.GetObjectFromPoolList (ObjectPooling.INSTANCE.swordList, new Vector3 (transform.position.x, transform.position.y + 1f, 0));
			if (transform.localScale.x > 0) {
				sword.GetComponent<Item> ().swordDirection = 1;
			} else if (transform.localScale.x < 0) {
				sword.GetComponent<Item> ().swordDirection = -1;
			}
		}
	}
}
