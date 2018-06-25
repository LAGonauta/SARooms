using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCExit : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKey("escape"))
    {
      Application.Quit();
    }
  }
}
