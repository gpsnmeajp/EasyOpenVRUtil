
/*
 * AllScan.cs
 * デバイスのプロパティを全部出す
 * 
 * gpsnmeajp 2021/06/04 v0.01
 * v0.01: 公開
 * 
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;
using Valve.VR;

public class AllScan : MonoBehaviour
{
    public string serial = "";
    EasyOpenVRUtil eou;
    string log = "";

    void Start()
    {
        eou = new EasyOpenVRUtil();
        eou.StartOpenVR();


        uint idx = eou.GetDeviceIndexBySerialNumber(serial);

        foreach (ETrackedDeviceProperty prop in Enum.GetValues(typeof(ETrackedDeviceProperty)))
        {
            bool ok = false;
            var name = prop.ToString();
            bool resultBool;
            if (eou.GetPropertyBool(idx, prop, out resultBool))
            {
                log += (name + " : " + resultBool);
                ok = true;
            }
            float resultFloat;
            if (eou.GetPropertyFloat(idx, prop, out resultFloat))
            {
                log += (name + " : " + resultFloat);
                ok = true;
            }
            int resultInt32;
            if (eou.GetPropertyInt32(idx, prop, out resultInt32))
            {
                log += (name + " : " + resultInt32);
                ok = true;
            }
            ulong resultUint64;
            if (eou.GetPropertyUint64(idx, prop, out resultUint64))
            {
                log += (name + " : " + resultUint64);
                ok = true;
            }
            string resultString;
            if (eou.GetPropertyString(idx, prop, out resultString))
            {
                log += (name + " : " + resultString);
                ok = true;
            }

            if (ok) {
                log += "\n";
            }
        }
        Debug.Log(log);
    }

    void Update()
    {
    }
}
