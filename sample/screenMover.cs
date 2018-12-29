/*
 * 部分的実装のためこのままでは動きません
 * isScreenMoving : ボタンを押しながらtrueで移動、falseで移動終了(ボタンを離すと自動でfalseになる。)
 * screenMoveWithRight : trueで右手に追従、falseで左手に追従
 *
 * gpsnmeajp 2018/12/29
 * These codes are licensed under CC0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 */
    void Update() {
        //初期化されていないとき初期化する
        if (!util.IsReady())
        {
            util.Init();
            return;
        }

        //移動モード
        if (isScreenMoving) {
            ulong button = 0;
            EasyOpenVRUtil.Transform cpos = null;
            if (screenMoveWithRight)
            {
                if (util.GetControllerButtonPressed(util.GetRightControllerIndex(), out button))
                {
                    cpos = util.GetRightControllerTransform();
                }
            }
            else {
                if (util.GetControllerButtonPressed(util.GetLeftControllerIndex(), out button))
                {
                    cpos = util.GetLeftControllerTransform();
                }
            }
            if (button == 0)
            {
                isScreenMoving = false;
            }

            if (cpos != null)
            {
                Vector3 ang = (cpos.rotation * Quaternion.AngleAxis(45,Vector3.right)).eulerAngles;
                //常にこっちに向き、ゆっくり追従する
                EOVRO.Position = cpos.position; //これが難しい...
                EOVRO.Rotation = new Vector3(-ang.x, -ang.y, ang.z); //こっち向く。これでオッケー
            }
        }
    }
