using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckBox : MonoBehaviour {
    public Toggle chx1;
    public Toggle chx2;
    public Toggle chx3;
    public Toggle chx4;
    public Toggle chx5;
    public Toggle chx6;

    public void Reset()
    {
        chx1.isOn = false;
        chx2.isOn = false;
        chx3.isOn = false;
        chx4.isOn = false;
        chx5.isOn = false;
        chx6.isOn = false;
    }
}
