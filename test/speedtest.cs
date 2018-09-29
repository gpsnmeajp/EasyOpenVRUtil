using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

public class speedtest : MonoBehaviour {
    public GameObject Tracker;
    public GameObject Tracker2;
    public string serialNumber;
    EasyOpenVRUtil eou;

    EasyOpenVRUtil.Transform old;
    // Use this for initialization
    void Start()
    {
        eou = new EasyOpenVRUtil();
        old = eou.GetTransformBySerialNumber(serialNumber);
    }

    // Update is called once per frame
    void Update()
    {
        eou.Init();

        var t = eou.GetTransformBySerialNumber(serialNumber);
        eou.SetGameObjectTransform(ref Tracker, t);

        var p = Tracker.transform.position;
        //Tracker.transform.position = p + t.position - old.position;

        var q = Tracker.transform.rotation;
        //Tracker.transform.rotation = q * t.rotation * Quaternion.Inverse(old.rotation);

        p = Tracker2.transform.position;
        p[0] = p[0] + t.velocity[0] * Time.deltaTime;
        p[1] = p[1] + t.velocity[1] * Time.deltaTime;
        p[2] = p[2] + t.velocity[2] * Time.deltaTime;
        Tracker2.transform.position = p;

        var r = Tracker2.transform.rotation.eulerAngles;
        r[0] = r[0] + Mathf.Rad2Deg * (t.angularVelocity[0] * Time.deltaTime);
        r[1] = r[1] + Mathf.Rad2Deg * (t.angularVelocity[1] * Time.deltaTime);
        r[2] = r[2] + Mathf.Rad2Deg * (t.angularVelocity[2] * Time.deltaTime);
        Tracker2.transform.rotation = Quaternion.Euler(r);

        old = t;
    }
}
