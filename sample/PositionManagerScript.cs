/*
 * EasyOpenVRUtil ScreenMove And Cursor Sample
 * 
 * gpsnmeajp 2019/01/04
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

//もっともーっとコメントつけて！！！！！！！！

public class PositionManagerScript : MonoBehaviour {
    [SerializeField]
    private EasyOpenVROverlayForUnity EOVRO;
    [SerializeField]
    private RectTransform LeftCursorTextRectTransform;
    [SerializeField]
    private RectTransform RightCursorTextRectTransform;
    [SerializeField]
    private RectTransform CanvasRectTransform;

    EasyOpenVRUtil util = new EasyOpenVRUtil();

    public Vector3 OverlayPosition = new Vector3(0.03f, -0.25f, 0.5f);
    public Vector3 OverlayRotation = new Vector3(-20f, 0, 0);

    private bool isScreenMoving = false;
    private bool screenMoveWithRight = false;

    // Use this for initialization
    void Start () {
        util.Init();
        setPosition(); //とりあえずUnityスタート時の位置に設定(適切に設定してください)
    }

    // Update is called once per frame
    void Update () {
        //初期化されていないとき初期化する
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        //カーソル位置更新
        LeftCursorTextRectTransform.anchoredPosition = new Vector2(EOVRO.LeftHandU - CanvasRectTransform.sizeDelta.x / 2f, EOVRO.LeftHandV - CanvasRectTransform.sizeDelta.y / 2f);
        RightCursorTextRectTransform.anchoredPosition = new Vector2(EOVRO.RightHandU - CanvasRectTransform.sizeDelta.x / 2f, EOVRO.RightHandV - CanvasRectTransform.sizeDelta.y / 2f);


        //移動モード
        if (isScreenMoving)
        {
            ulong button = 0;
            EasyOpenVRUtil.Transform pos = util.GetHMDTransform();
            EasyOpenVRUtil.Transform cpos = null;
            if (screenMoveWithRight)
            {
                if (util.GetControllerButtonPressed(util.GetRightControllerIndex(), out button))
                {
                    cpos = util.GetRightControllerTransform();
                }
            }
            else
            {
                if (util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out button))
                {
                    cpos = util.GetLeftControllerTransform();
                }
            }
            if (button == 0)
            {
                isScreenMoving = false;
            }

            if (pos != null && cpos != null)
            {
                var z = 0;
                Vector3 ang = (cpos.rotation * Quaternion.AngleAxis(45, Vector3.right)).eulerAngles;
                //常にこっちに向き、ゆっくり追従する
                Vector3 BillboardPosition = cpos.position; //これが難しい...
                Vector3 BillboardRotation = new Vector3(-ang.x, -ang.y, ang.z); //こっち向く。これでオッケー

                EOVRO.Position = BillboardPosition;
                EOVRO.Rotation = BillboardRotation;
            }
        }
    }

    //移動モードにはいる
    public void MoveMode()
    {
        //コントローラのボタン入力と取得可能かをチェック
        ulong Leftbutton, Rightbutton;
        if (util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out Leftbutton))
        {
            if (Leftbutton != 0)
            {
                isScreenMoving = true;
                screenMoveWithRight = false;
                return;
            }
        }
        if (util.GetControllerButtonPressed(util.GetRightControllerIndex(), out Rightbutton))
        {
            if (Rightbutton != 0)
            {
                isScreenMoving = true;
                screenMoveWithRight = true;
                return;
            }
        }
    }

    //HMDからの基準位置に更新する
    public void setPosition()
    {
        var pos = util.GetHMDTransform();
        if (pos == null)
        {
            return; //更新しない
        }

        var z = 0;


        //常にこっちに向き、ゆっくり追従する
        Vector3 BillboardPosition = pos.position + pos.rotation * OverlayPosition; //これが難しい...
        Vector3 BillboardRotation = (new Vector3(-pos.rotation.eulerAngles.x, -pos.rotation.eulerAngles.y, z)) + OverlayRotation; //こっち向く。これでオッケー

        EOVRO.Position = BillboardPosition;
        EOVRO.Rotation = BillboardRotation;

    }
}
