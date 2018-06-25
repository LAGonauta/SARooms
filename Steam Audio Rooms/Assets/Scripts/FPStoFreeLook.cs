using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPStoFreeLook : MonoBehaviour {
  
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown("c"))
    {
      if ((GetComponent("FirstPersonController") as MonoBehaviour).enabled)
      {
        (GetComponent("FirstPersonController") as MonoBehaviour).enabled = false;
        (GetComponent("ExtendedFlyCam") as MonoBehaviour).enabled = true;
      }
      else
      {
        (GetComponent("FirstPersonController") as MonoBehaviour).enabled = true;
        (GetComponent("ExtendedFlyCam") as MonoBehaviour).enabled = false;
      }
    }
  }
}
