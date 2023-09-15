using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.GetComponent<Player> () == null)
			return;
		if (col.gameObject.transform.position.y > transform.position.y) {
			transform.parent.gameObject.GetComponent<PlatformController> ().check = true;
		}
	}
}
