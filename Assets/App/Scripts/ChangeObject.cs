using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeObject : MonoBehaviour {
    public GameObject hideObject;
    public Text buttonText;
    bool show = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Toggle ()
    {
        if (show)
        {
            show = false;
            buttonText.text = "Show Light (setup)";
            hideObject.SetActive(false);
        }
        else
        {
            show = true;
            buttonText.text = "Hide Light (setup)";
            hideObject.SetActive(true);
        }
    }
}
