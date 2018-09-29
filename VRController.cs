using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour {
    public GameObject LeftController;
    public GameObject RightController;
    EasyOpenVRUtil eou;

    void Start () {
        eou = new EasyOpenVRUtil();
    }

    void Update () {
        if (!eou.IsReady())
        {
            eou.Init();
            return;
        }

        Vector3 LeftPosition, RightPosition;
        Quaternion LeftRotation, RightRotation;

        eou.GetControllerPose(out LeftPosition, out LeftRotation, out RightPosition, out RightRotation);
        if (LeftController != null)
        {
            LeftController.transform.position = LeftPosition;
            LeftController.transform.rotation = LeftRotation;
        }
        if (RightController != null)
        {
            RightController.transform.position = RightPosition;
            RightController.transform.rotation = RightRotation;
        }

    }
}
