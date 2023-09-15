using UnityEngine;
using Tools;
public class CharacterDIe : MonoBehaviour {

	private void EnemyDead()
	{
		MusicAndSound.INSTANCE.PlaySoundEffect (6);
		gameObject.SetActive (false);
	}
}
