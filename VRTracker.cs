using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTracker : MonoBehaviour {
    public GameObject Tracker;
    public string serialNumber;
    EasyOpenVRUtil eou;

    // Use this for initialization
    void Start () {
        eou = new EasyOpenVRUtil();
    }
	
	// Update is called once per frame
	void Update () {
        if (!eou.IsReady())
        {
            eou.Init();
            return;
        }

        if (Tracker != null)
        {
            Vector3 pos;
            Quaternion rot;
            if (eou.GetPoseBySerialNumber(serialNumber, out pos, out rot))
            {
                Tracker.transform.position = pos;
                Tracker.transform.rotation = rot;
            }
        }
    }
}
