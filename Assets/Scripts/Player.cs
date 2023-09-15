using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour, IDame , IHeal
{
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 6f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public bool canDoubleJump;
    private bool isDoubleJumping = false;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

	public float basichp = 100;
	public float playerhp;

	public bool isProtect;
	public GameObject sheild;
	private float timer;

	public float[] basicEnemyHp;
	public float enemyhp;

public enum CHARACTER_TYPE
	{
		PLAYER,
		AI
	}

	public CHARACTER_TYPE _CHARACTER_TYPE;

	[Header("Enemy Type")]
	public int enemyType;

	public GameObject shotBtn;

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

		EntryAnim ();

		if (_CHARACTER_TYPE == CHARACTER_TYPE.PLAYER) {
			playerhp = basichp;
			UIManager.Instace.SetHealthUI ();
		} else if (_CHARACTER_TYPE == CHARACTER_TYPE.AI) {

			enemyhp = basicEnemyHp [enemyType];
		}
    }

    private void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

		if (isProtect) {
			sheild.SetActive (true);

			timer += Time.deltaTime;
			if (timer >= 5) {
				timer = 0;
				isProtect = false;
				sheild.SetActive (false);
			}
		}

		if (_CHARACTER_TYPE == CHARACTER_TYPE.PLAYER) {
			if (PlayerPrefs.GetInt ("bullet") > 0) {
				shotBtn.SetActive (true);
			} else {
				shotBtn.SetActive (false);
			}
		}
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
            isDoubleJumping = false;
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = false;
        }
        if (canDoubleJump && !controller.collisions.below && !isDoubleJumping && !wallSliding)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = true;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0f)
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;
                if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }

	void OnTriggerEnter2D (Collider2D col)
	{
		if (_CHARACTER_TYPE == CHARACTER_TYPE.PLAYER) {
			var item = col.GetComponent<IItem> ();
            var healing = col.GetComponent<IHeal>(); 
            if (item == null)
				return;
			item.OnTouchPlayer ();
            if (healing != null) // Điều kiện gọi đến 
            {
                healing.Heal();
            }
        }
	}

	private	void OnCollisionEnter2D (Collision2D col)
	{
		if (_CHARACTER_TYPE == CHARACTER_TYPE.PLAYER) {
			var item = col.gameObject.GetComponent<IItem> ();
			if (item == null)
				return;
			item.OnTouchPlayer ();
		}
	}

	public Animator anim;

	private void EntryAnim()
	{
		switch (_CHARACTER_TYPE) {
		case CHARACTER_TYPE.PLAYER:
			anim.Play ("player_" + Menu.selectedCharacter.ToString () + "_idle");
			break; 
		case CHARACTER_TYPE.AI:
			anim.Play ("enemy_" + enemyType.ToString () + "_move");
			break;
		default:
			break;
		}
	}

	public void AnimMove()
	{
		anim.Play ("player_" + Menu.selectedCharacter.ToString () + "_move");
	}

	public void AnimAttack()
	{
		anim.Play ("player_" + Menu.selectedCharacter.ToString () + "_attack");
	}

	public void AnimAttackAI()
	{
		if (_CHARACTER_TYPE == CHARACTER_TYPE.AI) {
//			if (Mathf.Abs (transform.position.y - GameManager.Instace.player.gameObject.transform.position.y)) {
//				anim.Play ("enemy_" + enemyType.ToString () + "_attack");
//			}
		}

	}

	public void AnimHurt()
	{
		anim.Play ("player_" + Menu.selectedCharacter.ToString () + "_hurt");
	}

	public void Dame()
	{
		switch (_CHARACTER_TYPE) {
		case CHARACTER_TYPE.PLAYER:
			if (!isProtect) {
				playerhp -= GameManager.Instace.EnemyDamePlayer [Menu.selectedCharacter];
				UIManager.Instace.SetHealthUI ();
				anim.Play ("player_" + Menu.selectedCharacter.ToString () + "_hurt");
				if (playerhp < 0) {
					GameManager.Instace.InGameFuntion (6);
				}
			}
			break;
		case CHARACTER_TYPE.AI:
			var ins = GetComponent<PlayerInput> ();
			ins.isDead = true;
			ins.anim.Play ("enemy_" + enemyType.ToString () + "_dead");
//			enemyhp -= GameManager.Instace.PlayerDameEnemy [Menu.selectedCharacter];
//
//			if (enemyhp < 0) {
//				var ins = GetComponent<PlayerInput> ();
//				ins.isDead = true;
//				ins.anim.Play ("enemy_" + enemyType.ToString () + "_dead");
//			}
			break;
		}
	}

    public void Heal()
    {
        // Hồi máu cho player
        playerhp += GameManager.Instace.PlayerHealing[Menu.selectedCharacter];
        if (playerhp > basichp)
            playerhp = basichp;
        UIManager.Instace.SetHealthUI();
    }


}
