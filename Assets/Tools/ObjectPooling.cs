//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
	public class ObjectPooling : MonoBehaviour
	{
		public static ObjectPooling INSTANCE{ get; private set;}

		public GameObject sword;
		public List<GameObject> swordList;
		 
		private	void Awake ()
		{
			INSTANCE = this;
		}

		void Start ()
		{
			FillPoolObject (10, sword, swordList);
		}

		///<Summary>
		///Implement this method to instatiate pool object and fill them to a list object
		///</Summary>
		public void FillPoolObject(int _fillAmount, GameObject _objectToPool, List<GameObject> _pooledList)
		{
			for (int i = 0; i < _fillAmount; i++) {
				GameObject poolObj;
				poolObj = Instantiate (_objectToPool, Vector3.zero, Quaternion.identity);
				poolObj.transform.SetParent (transform);
				_pooledList.Add (poolObj);
				poolObj.SetActive (false);
			}
		}

		///<Summary>
		///Implement this method to get object from poolist
		///</Summary>
		public GameObject GetObjectFromPoolList(List<GameObject> _pooledList, Vector3 newObjectPos)
		{
			for (int i = 0; i < _pooledList.Count; i++) {
				if (!_pooledList [i].activeInHierarchy) {
					_pooledList [i].transform.position = newObjectPos;
					_pooledList [i].SetActive (true);
					return _pooledList [i];
				}
			}
			return null;
		}
	}
}
