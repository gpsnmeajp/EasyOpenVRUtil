using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

public class VRController : MonoBehaviour {
    public GameObject LeftController;
    public GameObject RightController;
    EasyOpenVRUtil eou;

    void Start () {
        eou = new EasyOpenVRUtil();
    }

    void Update () {
        eou.Init();
        eou.AutoExitOnQuit();

        var l = eou.GetLeftControllerTransform();
        var r = eou.GetRightControllerTransform();
        eou.SetGameObjectLocalTransform(ref LeftController, l);
        eou.SetGameObjectLocalTransform(ref RightController, r);
    }
}
