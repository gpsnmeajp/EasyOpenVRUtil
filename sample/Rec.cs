using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;
//gpsnmeajp


public class Rec : MonoBehaviour {
    EasyOpenVRUtil eovru = new EasyOpenVRUtil();
    StreamWriter fp;

    void Start () {
        string mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        fp = new StreamWriter(Path.Combine(mydoc, "move.csv"));

        eovru.StartOpenVR();
        fp.WriteLine("time,serial,x,y,z,vx,vy,vz,rx,ry,rz,rdx,rdy,rdz,eTrackingResult");

    }

    void Update () {
        bool run = false;
        foreach (var x in eovru.GetViveControllerIndexList()) {
            var rez = eovru.GetDevicePose(x);

            var t = eovru.GetTransform(x);
            var a = t.angularVelocity;
            var v = t.velocity;
            var p = t.position;
            var r = t.rotation;
            var s = eovru.GetSerialNumber(x);

            fp.Write(Time.time); fp.Write(",");
            fp.Write(s); fp.Write(",");
            fp.Write(p.x); fp.Write(",");
            fp.Write(p.y); fp.Write(",");
            fp.Write(p.z); fp.Write(",");
            fp.Write(v.x); fp.Write(",");
            fp.Write(v.y); fp.Write(",");
            fp.Write(v.z); fp.Write(",");
            fp.Write(r.eulerAngles.x); fp.Write(",");
            fp.Write(r.eulerAngles.y); fp.Write(",");
            fp.Write(r.eulerAngles.z); fp.Write(",");
            fp.Write(a.x); fp.Write(",");
            fp.Write(a.y); fp.Write(",");
            fp.Write(a.z); fp.Write(",");
            fp.Write(rez.eTrackingResult.ToString()); fp.Write(",");
            fp.WriteLine();
            run = true;

            if (rez.eTrackingResult != Valve.VR.ETrackingResult.Running_OK) {
                eovru.TriggerHapticPulse(x);
            }
        }
        if (!run) {
            fp.Write(Time.time);
            fp.WriteLine();
        }
    }

    void OnApplicationQuit()
    {
        fp.Close();
    }
}
