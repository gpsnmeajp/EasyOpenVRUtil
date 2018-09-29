using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

public class test : MonoBehaviour {
    public GameObject obj;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;

    EasyOpenVRUtil eou = new EasyOpenVRUtil();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("!");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(eou.Init());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(eou.StartOpenVR());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(eou.TakeScreenShot("D:\\tmp\\test", "D:\\tmp\\test2"));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(eou.GetDeviceIndexBySerialNumber("LHR-72214A13"));
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            eou.Update();
            Debug.Log(eou.PutDeviceInfoListString());

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(eou.PutDeviceInfoListStringFromDeviceIndexList(eou.GetDeviceIndexListByRegisteredDeviceType("tracker")));
            Debug.Log(eou.PutDeviceInfoListStringFromDeviceIndexList(eou.GetDeviceIndexListByRenderModelName("tracker")));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(eou.TriggerHapticPulse(eou.GetLeftControllerIndex()));
            Debug.Log(eou.TriggerHapticPulse(eou.GetRightControllerIndex()));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Controller");
            Debug.Log(eou.PutDeviceInfoListStringFromDeviceIndexList(eou.GetViveControllerIndexList()));
            Debug.Log("Tracker");
            Debug.Log(eou.PutDeviceInfoListStringFromDeviceIndexList(eou.GetViveTrackerIndexList()));
            Debug.Log("BaseStation");
            Debug.Log(eou.PutDeviceInfoListStringFromDeviceIndexList(eou.GetBaseStationIndexList()));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {

        }

        /*
        Vector3 pos;
        Quaternion rot;
        eou.GetHMDPose(out pos, out rot);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        eou.GetPoseBySerialNumber("LHR-72214A13", out pos, out rot);
        obj2.transform.position = pos;
        obj2.transform.rotation = rot;

        Vector3 pos2;
        Quaternion rot2;
        eou.GetControllerPose(out pos, out rot, out pos2, out rot2);
        obj3.transform.position = pos;
        obj3.transform.rotation = rot;

        obj4.transform.position = pos2;
        obj4.transform.rotation = rot2;
*/
    }
}
