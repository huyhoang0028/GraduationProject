using UnityEngine;

public class PlayerSword : MonoBehaviour {

	[SerializeField] private float movementDistance;
    [SerializeField] private float speed;
	private bool movingLeft;
	private float leftEdge;
	private float rightEdge;

    public enum SWORD_SKIPESS
	{
		SWORD,
		SKIPES,
		SAW
	}

    private void Awake()
    {
		if (_SWORD_SKIPESS == SWORD_SKIPESS.SAW) {
            leftEdge = transform.position.x - movementDistance;
            rightEdge = transform.position.x + movementDistance;
        }
        
    }
	private void Update()
	{
		if (_SWORD_SKIPESS == SWORD_SKIPESS.SAW)
		{
            if (movingLeft)
            {
                if (transform.position.x > leftEdge)
                {
                    transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                else
                    movingLeft = false;
            }
            else
            {
                if (transform.position.x < rightEdge)
                {
                    transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
    
            }
                else
                    movingLeft = true;
            }
        }
	}

    public SWORD_SKIPESS _SWORD_SKIPESS;

	void OnTriggerEnter2D (Collider2D col)
	{
		var swordDamage = col.GetComponent<IItem> ();
		if (swordDamage != null) {
			swordDamage.OntouchSword ();
		}
		var swordDamage1 = col.GetComponent<IDame> ();
		if (swordDamage1 != null) {
			swordDamage1.Dame ();
		}

		if (GetComponent<Item> () != null) {
			gameObject.SetActive (false);
		}

	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (_SWORD_SKIPESS == SWORD_SKIPESS.SKIPES) {
			var swordDamage1 = col.GetComponent<IDame> ();
			if (swordDamage1 != null) {
				swordDamage1.Dame ();
			}
		}
        if (_SWORD_SKIPESS == SWORD_SKIPESS.SAW)
        {
            var swordDamage1 = col.GetComponent<IDame>();
            if (swordDamage1 != null)
            {
                swordDamage1.Dame();
            }
        }

    }
}
