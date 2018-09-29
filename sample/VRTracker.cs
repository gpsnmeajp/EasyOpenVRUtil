using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

public class VRTracker : MonoBehaviour {
    public GameObject Tracker;
    public string serialNumber;
    EasyOpenVRUtil eou;

    EasyOpenVRUtil.Transform offset;
    // Use this for initialization
    void Start () {
        eou = new EasyOpenVRUtil();
    }

    // Update is called once per frame
    void Update () {
        eou.Init();
        eou.AutoExitOnQuit();

        var t = eou.GetTransformBySerialNumber(serialNumber);
        if (offset == null)
        {
            offset = t;
        }

        eou.SetGameObjectLocalTransformWithOffset(ref Tracker, t, offset);
    }
}
