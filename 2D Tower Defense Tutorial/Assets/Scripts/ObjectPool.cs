using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	[SerializeField]
	private GameObject[] objectPrefabs;

	private List<GameObject> pooledObjects = new List<GameObject>();

	/// <summary>
	/// Returns the requested object.	
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="type">The Type we want to get a GameObject for.</param>
	public GameObject getObject(string type){
		foreach (GameObject go in pooledObjects) {
			if (go.name == type && !go.activeInHierarchy) {
				go.SetActive (true);
				return go;
			}
		}


		//create a new one if no other is available of the requested type
		for (int i = 0; i < objectPrefabs.Length; i++) {
			if (objectPrefabs [i].name.Equals (type)) {
				GameObject newObject = (GameObject)Instantiate (objectPrefabs [i]);
				newObject.name = type;
				pooledObjects.Add(newObject);
				return newObject;
			}
		}
		return null;
	}

	public void ReleaseObject(GameObject releaseObject){
		releaseObject.SetActive (false);
	}
}