using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour {

    public MapCreateor map_createor = null;

	// Use this for initialization
	void Start () {
        map_createor = GameObject.Find("GameRoot").GetComponent<MapCreateor>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.map_createor.isDelete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
