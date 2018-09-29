# EasyOpenVRUtil

かゆいところに微妙に手が届かないSteam VR Pluginの仕様を見て、
補助ライブラリを作りたくなりました。

OpenVRに直接アクセスして、
Steam VR 2.0で色々おもしろいことになった入力システムを無視して
コントローラやトラッカーの座標を取得したり、
識別しにくいトラッカーをシリアル番号で識別できたり、
バッテリー残量を取得できたり、
VR内スクリーンショットを勝手に撮影したり、
デバイス一覧を取得したり、
非VRアプリケーションだけどトラッカーやコントローラーの姿勢を取得したりできます。

3時間で作ったのでデバッグ不足なところがあるかも知れません。不具合報告お願いします。

↓ダウンロード
EasyOpenVRUtil by gpsnmeajp v0.03 (2018/9/27)
https://sabowl.sakura.ne.jp/gpsnmeajp/unity/EasyOpenVRUtil/EasyOpenVRUtil.cs
CC0ライセンスです。
バグ報告などは、コメント、メール、もしくはDiscord:https://discord.gg/QSrDhE8まで

# 使い方

```C#
var eou = new EasyOpenVRUtil();
```

で初期化します。
VRアプリケーションや、Overlayが動作しているなど、すでにOpenVRが初期化済みの場合は
自動的にライブラリ内が初期化され、これで利用できます。

ライブラリが初期化されているかどうかを調べるには

```C#
if(eou.IsReady())
{
   Debug.Log("初期化済み");
}else{
   Debug.Log("未初期化");
}
```

OpenVRが利用可能な状態になったかはどうかは

```C#
if(eou.CanUseOpenVR())
{
   Debug.Log("OpenVR初期化済み");
}else{
   Debug.Log("OpenVR未初期化");
}
```

OpenVRが利用可能になったあと改めて初期化する場合は

```C#
if(eou.Init())
{
   Debug.Log("初期化成功");
}else{
   Debug.Log("失敗");
}
```

もし、このライブラリ以外にOpenVRを初期化できる存在が居ない場合
(コントローラーやTrackerの情報だけ欲しい場合など)は、以下のようにすると初期化できます。
この際、本ライブラリも自動的に初期化されます。

```C#
if(eou.StartOpenVR())
{
   Debug.Log("初期化成功");
}else{
   Debug.Log("失敗");
}
```


# 機能
後日編集します。

## 定数
### const uint InvalidDeviceIndex
無効なデバイスであることを示すindex

## ライブラリ・OpenVRの初期化
### public bool Init()
本ライブラリを再度初期化する。
本ライブラリをインスタンス化した時点ですでにOpenVRが初期化済みの場合は、自動で初期化されるため不要。
初期化成功でtrue、失敗でfalse

### public bool IsReady()
本ライブラリが初期化されているかチェックする。
初期化されていればtrue、されていなければfalse

### public bool StartOpenVR(EVRApplicationType type = EVRApplicationType.VRApplication_Overlay)
OpenVRを初期化する。非VRアプリケーションの場合に使用する。
typeは省略するとOverlayアプリケーションとなる。(基本問題ないはず)
他のライブラリなどで既に初期化されている場合は何もしない。
初期化成功か、既に初期化済みでtrue、失敗でfalse

### public bool CanUseOpenVR()
OpenVRが初期化され利用可能な状態かをチェックする。
利用可能な状態ならtrue、利用不可能でfalse。
利用不可能な場合は、StartOpenVR()を呼び出すことで利用できる。

## デバイスのindex取得
### public bool IsDeviceValid(uint index)
指定されたデバイスindexが有効かを調べる。
有効な場合はtrue、無効な場合はfalse

有効というのは、
1. indexが無効値ではない
2. 接続されている
3. 姿勢情報が有効である
のANDである。
なおSetAutoUpdateがtrueだとこの際にupdate()が入る仕様になっている。

### public uint GetDeviceIndexBySerialNumber(string serial)
指定されたシリアル番号と完全一致するデバイスindexを返す。
取得できなかった場合は、InvalidDeviceIndexが帰る。
トラッカーなど個体識別がし辛いデバイスに向いている。
GetPropertyStringWhenConnected()を使って予めシリアル番号を取得しておくと良い。

### public uint GetHMDIndex()
HMDのデバイスindexを取得する。
取得できなかった場合は、InvalidDeviceIndexが帰る。

### public uint GetLeftControllerIndex()
左コントローラーのデバイスindexを取得する。
接続されていないなど、取得できなかった場合はInvalidDeviceIndexが帰る。

### public uint GetRightControllerIndex()
右コントローラーのデバイスindexを取得する。
接続されていないなど、取得できなかった場合はInvalidDeviceIndexが帰る。

##接続デバイス管理
### public int ConnectedDevices()
現在接続されているデバイス数を返す。
これにはHMDやコントローラーの他、ベースステーションやトラッカーも含む。

### public List<uint> GetViveTrackerIndexList()
接続されている全てのコントローラー(何個でも)のリストを取得する

### public List<uint> GetViveControllerIndexList()
接続されている全てのトラッカー(何個でも)のリストを取得する

### public List<uint> GetBaseStationIndexList()
接続されている全てのベースステーション(何個でも)のリストを取得する

### public List<uint> GetDeviceIndexListByRegisteredDeviceType(string name)
RegisteredDeviceTypeにnameを含むデバイスの一覧を取得する
見つからなかった場合、空のリストを返す。
例えば"htc/vive_tracker"を指定すると、Viveトラッカーのリストとなる。

### public List<uint> GetDeviceIndexListByRenderModelName(string name)
RenderModelNameに指定した文字列を含むデバイスindexのリストを取得する。
見つからなかった場合、空のリストを返す。
例えば"tracker"を指定すると、Viveトラッカーのリストとなる。
一部のデバイスはこちらでしか検索できない(ベースステーションなど)。

### public string GetDeviceDebugInfo(uint idx)
指定したデバイスindexの主要なデバッグ情報を文字列で取得する。
デバッグ情報とは
1. RenderModelName
2. RegisteredDeviceType
3. バッテリー残量
である。
改行区切りであり、Debug.Logと組み合わせたり、Textと組み合わせることを想定している。

### public string PutDeviceInfoListString() 変更
全デバイスindexの主要なデバッグ情報(上と同じ)を文字列で取得する。
改行区切りであり、Debug.Logと組み合わせたり、Textと組み合わせることを想定している。

### public string PutDeviceInfoListStringFromDeviceIndexList(List<uint> devices)
リストに含まれるデバイスの主要なデバッグ情報(上と同じ)を文字列で取得する。
改行区切りであり、Debug.Logと組み合わせたり、Textと組み合わせることを想定している。
デバイスリスト取得系メソッドの結果を確認するのに利用できる。

##デバイスの姿勢と速度
※速度情報は、OpenVRの右手系のままです。なおUnityは左手系です。
　後々、互換性をもたせたまま修正します。
### public bool GetHMDPose(out Vector3 pos, out Quaternion rot)
HMDの現在の姿勢(位置、回転)情報を取得する。
取得できた場合はtrue、失敗した場合falseを返す。

### public bool GetHMDVelocity(out Vector3 velocity, out Vector3 angularVelocity)
HMDの現在の速度(速度、角速度)情報を取得する。※
取得できた場合はtrue、失敗した場合falseを返す。

### public bool GetControllerPose(out Vector3 Lpos, out Quaternion Lrot, out Vector3 Rpos, out Quaternion Rrot)
左右のコントローラの現在の姿勢(位置、回転)情報を取得する。
両方取得できた場合はtrue、片方でも失敗した場合falseを返す。

### public bool GetControllerVelocity(out Vector3 Lvelocity, out Vector3 LangularVelocity, out Vector3 Rvelocity, out Vector3 RangularVelocity)
コントローラの速度(速度、角速度)情報を取得する。※
両方取得できた場合はtrue、片方でも失敗した場合falseを返す。

### public bool GetPose(uint index, out Vector3 pos, out Quaternion rot)
指定したデバイスindexの現在の姿勢(位置、回転)情報を取得する。
取得できた場合はtrue、失敗した場合falseを返す。

### public bool GetVelocity(uint index, out Vector3 velocity, out Vector3 angularVelocity)
指定したデバイスindexの速度(速度、角速度)情報を取得する。※
取得できた場合はtrue、失敗した場合falseを返す。

### public bool GetPoseBySerialNumber(string serial, out Vector3 pos, out Quaternion rot)
指定したリアル番号のデバイスの現在の姿勢(位置、回転)情報を取得する。
取得できた場合はtrue、失敗した場合falseを返す。
シリアル番号を毎回検索するため重いので非推奨。(デバッグやプロトタイプ向け)
GetDeviceIndexBySerialNumber()でindexを取得後、GetPose()を使用することをおすすめする。

### public bool GetVelocityBySerialNumber(string serial, out Vector3 velocity, out Vector3 angularVelocity)
指定したシリアル番号のデバイスの速度(速度、角速度)情報を取得する。※
取得できた場合はtrue、失敗した場合falseを返す。
シリアル番号を毎回検索するため重いので非推奨。(デバッグやプロトタイプ向け)
GetDeviceIndexBySerialNumber()でindexを取得後、GetVelocity()を使用することをおすすめする。

##全姿勢情報
### public void Update(ETrackingUniverseOrigin origin = ETrackingUniverseOrigin.TrackingUniverseStanding)
全姿勢情報を更新する。この姿勢情報は内部で様々利用している。
既定では必要に応じ、概ねのメソッドの呼び出し時に自動で更新されるようになっているため不要。
originは省略可能で、既定でルームスケールだが座位にもできる。
ただし座位にする場合はSetAutoUpdateをfalseにし、必要な時にUpdateすること。

### public void SetAutoUpdate(bool autoupdate)
全姿勢情報の自動更新の有無を設定する。
trueで自動更新(既定)、falseで手動更新。
同フレーム内で何度も姿勢情報の取得を行ってほしくない時にfalseにする。
手動更新の場合は、毎フレームの開始時などにUpdate()を行うこと。
でなければ姿勢以外にも、接続状態・有効状態などが古い状態のままになる場合がある。

### public TrackedDevicePose_t[] GetAllDevicePose()
現在の全姿勢情報取得。このメソッドでの自動更新は行われない。

### public TrackedDevicePose_t GetDevicePose(uint i)
現在の指定したデバイスindexの姿勢情報取得。このメソッドでは自動更新が行われる。
無効なデバイスを指定した場合、空のTrackedDevicePose_tが帰る。

### public ETrackingResult GetDeviceTrackingResult(uint i)
現在の指定したデバイスindexの姿勢情報取得結果の取得。このメソッドでは自動更新が行われる。
デバイスの追従に成功しているか失敗しているか、追従範囲外に出たかなどがわかる。
無効なデバイスを指定した場合、ETrackingResult.Uninitializedが帰る。

##デバイスの詳細情報
### public bool IsDeviceConnected(uint idx)
指定したデバイスが接続されているか調べる
接続されていればture、されていなければfalse

### public string GetSerialNumber(uint idx)
指定したデバイスのシリアル番号を取得する。
デバイス側が取得に対応していなかったり、接続されていない場合はnull

### public string GetRenderModelName(uint idx)
指定したデバイスの外見名を取得する。
デバイス側が取得に対応していなかったり、接続されていない場合はnull

### public string GetRegisteredDeviceType(uint idx)
指定したデバイスの型式名を取得する。
デバイス側が取得に対応していなかったり、接続されていない場合はnull

### public float GetDeviceBatteryPercentage(uint idx)
指定したデバイスの電池残量を取得する(0～100)。
デバイス側が取得に対応していなかったり、接続されていない場合はfloat.NaN

### public bool IsCharging(uint idx, out bool result)
指定したデバイスが充電状態かを調べる。
取得に成功すればtrue、デバイス側が取得に対応していなかったり、接続されていない場合はfalseが帰る。
充電中ならばresult=true、充電中でなければresult=false

### public string GetPropertyStringWhenConnected(uint idx, ETrackedDeviceProperty prop)
デバイスのプロパティ情報(文字列)を、デバイスの接続状態を確認した上で取得する。
成功すると結果が、失敗するとnullが帰る。
(この際、そもそもデバイスがそのプロパティを持っていない場合もnullが帰る)

### public float GetPropertyFloatWhenConnected(uint idx, ETrackedDeviceProperty prop)
デバイスのプロパティ情報(float)を、デバイスの接続状態を確認した上で取得する。
成功すると結果が、失敗するとfloat.NaNが帰る。
(この際、そもそもデバイスがそのプロパティを持っていない場合もfloat.NaNが帰る)

### public bool GetPropertyString(uint idx, ETrackedDeviceProperty prop,out string result)
デバイスプロパティを取得する(文字列)
取得成功でtrue、失敗でfalse

### public bool GetPropertyFloat(uint idx, ETrackedDeviceProperty prop, out float result)
デバイスプロパティを取得する(float)
取得成功でtrue、失敗でfalse

### public bool GetPropertyBool(uint idx, ETrackedDeviceProperty prop, out bool result)
デバイスプロパティを取得する(bool)
取得成功でtrue、失敗でfalse

### public bool GetPropertyUint64(uint idx, ETrackedDeviceProperty prop, out ulong result)
デバイスプロパティを取得する(ulong)
取得成功でtrue、失敗でfalse

### public bool GetPropertyInt32(uint idx, ETrackedDeviceProperty prop, out int result)
デバイスプロパティを取得する(int)
取得成功でtrue、失敗でfalse

##デバイスのボタン
### public bool GetControllerButtonPressed(uint index, out ulong ulButtonPressed)
デバイスのボタン押されている情報を取得する

### public bool GetControllerState(uint index, out VRControllerState_t state)
デバイスのボタン押されている情報などのステータスを取得する

##デバイスの振動
### public bool TriggerHapticPulse(uint index, ushort us=3000)
指定したデバイスを振動させる。
us=0～3999 (省略した場合は3000)
成功すればtrue、デバイスが接続されていなければfalse

##スクリーンショット
### public bool TakeScreenShot(string path, string pathVR)
VRコンポジターにスクリーンショットを撮影を指示する。
pathは通常写真、pathVRはステレオ写真。それぞれフルpathを指定すること。
撮影開始に成功すればtrue、すでに撮影処理中であったり、撮影に失敗した場合はfalse。
現在のアプリケーションにかかわらず、きちんと現在のHMD内の視界が撮影される。
(コントローラーからスクリーンショット撮影を指示した場合と同様になる)

##OpenVR以外のおまけ
### public void Set90fps()
Unityを90fpsに設定する。
何も考えずにUnityでVRやろうとするとfpsがうなぎのぼりになっている場合があります。


#サンプルソース
##VRTracker
シリアル番号からトラッカーを検出して、GameObjectに反映します
![image.png](https://qiita-image-store.s3.amazonaws.com/0/191114/713ddf26-927c-7db5-c69d-99695742abf9.png)

```C#
/**
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
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

```

## VRTracker
コントローラーを検出して、GameObjectに反映します
![image.png](https://qiita-image-store.s3.amazonaws.com/0/191114/25842079-e67d-22f7-8492-68a05299d091.png)

```C#
/**
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
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

```

##雑多なサンプル
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour {
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

    }
}

```
