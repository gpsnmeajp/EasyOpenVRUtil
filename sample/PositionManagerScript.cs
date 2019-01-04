/*
 * PositionManagerScript.cs
 * 
 * ScreenMove And Cursor Sample for
 *  EasyOpenVRUtil 
 *  https://github.com/gpsnmeajp/EasyOpenVRUtil
 *  EasyOpenVROverlayForUnity
 *  https://sabowl.sakura.ne.jp/gpsnmeajp/unity/EasyOpenVROverlayForUnity/
 * 
 * gpsnmeajp 2019/01/04 v0.02
 * v0.02: ビルドすると位置が原点になる問題に対処
 * v0.01: 公開
 * 
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyLazyLibrary;

public class PositionManagerScript : MonoBehaviour {
    [SerializeField]
    private EasyOpenVROverlayForUnity EasyOpenVROverlay; //オーバーレイ表示用ライブラリ
    [SerializeField]
    private RectTransform LeftCursorTextRectTransform; //左手カーソル表示用Text
    [SerializeField]
    private RectTransform RightCursorTextRectTransform; //右手カーソル表示用Text
    [SerializeField]
    private RectTransform CanvasRectTransform; //全体サイズ計算用Canvas位置情報

    EasyOpenVRUtil util = new EasyOpenVRUtil(); //姿勢取得ライブラリ

    public Vector3 OverlayPosition = new Vector3(0.03f, -0.25f, 0.5f); //HMDの前方50cm、25cm下の位置に表示
    public Vector3 OverlayRotation = new Vector3(-20f, 0, 0); //操作しやすいよう-20°傾ける

    private bool isScreenMoving = false; //画面を移動させようとしているか？
    private bool screenMoveWithRight = false; //それが右手で行われているか？

    private bool PositionInitialize = true; //位置を初期化するフラグ(完了するとfalseになる)

    void Start () {
        //姿勢取得ライブラリを初期化
        util.Init();
   }

    void Update () {
        //姿勢取得ライブラリが初期化されていないとき初期化する
        //(OpenVRの初期化はEasyOpenVROverlayの方で行われるはずなので待機)
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        //HMDの位置情報が使えるようになった & 初期位置が初期化されていないとき
        if (util.GetHMDTransform() != null && PositionInitialize) {
            //とりあえずUnityスタート時のHMD位置に設定
            //(サンプル用。より適切なタイミングで呼び直してください。
            // OpenVRが初期化されていない状態では原点になってしまいます)
            setPosition();

            //初期位置初期化処理を停止
            PositionInitialize = false;
        }

        //カーソル位置を更新
        //オーバーレイライブラリが返す座標系をCanvasの座標系に変換している。
        //オーバーレイライブラリの座標サイズ(RenderTexture依存)と
        //Canvasの幅・高さが一致する必要がある。
        LeftCursorTextRectTransform.anchoredPosition = new Vector2(EasyOpenVROverlay.LeftHandU - CanvasRectTransform.sizeDelta.x / 2f, EasyOpenVROverlay.LeftHandV - CanvasRectTransform.sizeDelta.y / 2f);
        RightCursorTextRectTransform.anchoredPosition = new Vector2(EasyOpenVROverlay.RightHandU - CanvasRectTransform.sizeDelta.x / 2f, EasyOpenVROverlay.RightHandV - CanvasRectTransform.sizeDelta.y / 2f);

        //移動モード処理
        if (isScreenMoving)
        {
            ulong button = 0;
            EasyOpenVRUtil.Transform pos = util.GetHMDTransform(); //HMDが有効か調べる
            EasyOpenVRUtil.Transform cpos = null; //任意の手の姿勢情報
            if (screenMoveWithRight) //右手で操作されたなら
            {
                //右手のボタンが押されているか取得しながら、右手が有効か調べる
                if (util.GetControllerButtonPressed(util.GetRightControllerIndex(), out button))
                {
                    //有効なら右手の姿勢情報を取得する(瞬間的に通信が切れnullの可能性もある)
                    cpos = util.GetRightControllerTransform();
                }
            }
            else
            {
                //左手のボタンが押されているか取得しながら、右手が有効か調べる
                if (util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out button))
                {
                    //有効なら左手の姿勢情報を取得する(瞬間的に通信が切れnullの可能性もある)
                    cpos = util.GetLeftControllerTransform();
                }
            }

            //ボタンが一切押されなくなったならば移動モードから抜ける
            if (button == 0)
            {
                isScreenMoving = false;
            }

            //HMDも取得したコントローラ姿勢も有効ならば
            if (pos != null && cpos != null)
            {
                //コントローラの姿勢クォータニオンを45度傾けて、オイラー角に変換(しないと意図しない向きになってしまう)
                Vector3 ang = (cpos.rotation * Quaternion.AngleAxis(45, Vector3.right)).eulerAngles;

                //コントローラの位置をそのままOverlayの位置に反映
                EasyOpenVROverlay.Position = cpos.position; //これが難しい...

                //コントローラの回転を適時反転させてOverlayの回転に反映(こちら向きにする)
                EasyOpenVROverlay.Rotation = new Vector3(-ang.x, -ang.y, ang.z);
            }
        }
    }

    //コントローラによる画面移動モードにはいる
    public void MoveMode()
    {
        //コントローラのボタン入力と取得可能かをチェック
        ulong Leftbutton, Rightbutton;
        if (util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out Leftbutton))
        {
            //もし左手のコントローラが利用可能であり、何らかのボタンが押されているならば
            if (Leftbutton != 0)
            {
                //画面移動モードに遷移、コントローラは左手
                isScreenMoving = true;
                screenMoveWithRight = false;
                return;
            }
        }
        if (util.GetControllerButtonPressed(util.GetRightControllerIndex(), out Rightbutton))
        {
            //もし右手のコントローラが利用可能であり、何らかのボタンが押されているならば
            if (Rightbutton != 0)
            {
                //画面移動モードに遷移、コントローラは右手
                isScreenMoving = true;
                screenMoveWithRight = true;
                return;
            }
        }

        //どちらのコントローラも利用不可能であったか、なんのボタンも押されていない
    }

    //HMDの位置を基準に操作しやすい位置に画面を出す
    public void setPosition()
    {
        //HMDの姿勢情報を取得する
        var pos = util.GetHMDTransform();

        //HMDの姿勢情報が無効な場合は
        if (pos == null)
        {
            return; //更新しない
        }

        //HMDの位置に、基準位置とHMD角度を加算したものを、表示位置とする(でないと明後日の方向に移動するため)
        EasyOpenVROverlay.Position = pos.position + pos.rotation * OverlayPosition;

        //HMDの角度を一部反転したものに、基準角度を加算したものを、表示角度とする
        EasyOpenVROverlay.Rotation = (new Vector3(-pos.rotation.eulerAngles.x, -pos.rotation.eulerAngles.y, 0)) + OverlayRotation;

    }
}
