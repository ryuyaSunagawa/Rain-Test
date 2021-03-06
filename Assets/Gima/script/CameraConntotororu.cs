﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConntotororu : MonoBehaviour
{

    //カメラオブジェクト
    public GameObject mainCamera;
    //プレイヤーオブジェクト
    private GameObject Player;
    private GameObject feda;
    //z軸を調整。正の数ならプレイヤーの前に、負の数ならプレイヤーの後ろに配置する
    [Header("Z軸の補正")]
    [SerializeField, Range(0f, 30f)]    private float zAdjust = -12.0f;
    [Header("Y軸の補正")]
    [SerializeField, Range(0f, 30f)]    private float yAdjust = 5.0f;

    //X座調整
    private float Y_camera = 0.0f;

    // カメラの移動につかうやつまとめ(Nodake)
    GameObject[] rains;
    private Vector3 pos_p;
    private bool fix; // 雨に触れたかどうか
    private bool fix2 = false;
    public bool CameraMoveSwitch = false;
    private bool fix3d;
    private bool tri = false;

    void Start()
    {
        Player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        feda = GameObject.Find("Fade");
    }

    void Update()
    {

        fix = false;

        pos_p = Player.gameObject.GetComponent<TestJump_ver2>().restartPoint;

        if (Player.gameObject.GetComponent<TestJump_ver2>().Debug_F == false)
        {
            PlayerOnGrand(); // プレイヤーが地面に立っているかどうか
            PlayerOnRain(); // プレイヤーが雨に当たっているかどうか
        }
        else
        {
            mainCamera.transform.position = new Vector3(Player.transform.position.x,
                Player.transform.position.y, zAdjust);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("triangle") || Input.GetKeyDown(KeyCode.T) ||
            feda.gameObject.GetComponent<FadeScript>().fed == true)
        {
            pos_p.z = zAdjust;
            mainCamera.transform.position = pos_p;
            tri = true;
        }

        CameraMoveOnRain(); // カメラが雨粒に乗ったときのうごき

        if (!fix2 && !tri)
        {
            mainCamera.transform.position = new Vector3(Player.transform.position.x,
               Y_camera, zAdjust);
        }
    }

    // プレイヤーが地面に触れているかどうか
    void PlayerOnGrand()
    {
        if (Player.transform.position.y <= -3.60f)
        {
            fix2 = false;
            if (Player.transform.position.y <= mainCamera.transform.position.y)
            {
                Y_camera = Player.transform.position.y + yAdjust;
            }
            if (Player.transform.position.y >= mainCamera.transform.position.y)
            {
                Y_camera = Player.transform.position.y + yAdjust;
            }
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "floor") {
            fix3d = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        fix3d = false;
    }

    // プレイヤーが雨に当たっているかどうか
    void PlayerOnRain()
    {
        rains = GameObject.FindGameObjectsWithTag("Rain");
        for (int i = 0; i < rains.Length; ++i)
        {
            if (rains[i].gameObject.GetComponent<RainOperation>().fix == true)
            {
                fix = true;
            }
        }

    }
    // カメラが雨粒に乗ったとき、移動する
    void CameraMoveOnRain()
    {
        CameraMoveSwitch = false;
        if (fix == true||fix3d == true)
        {
            CameraMoveSwitch = true; // カメラを移動する
            fix2 = true;
            tri = false;
        }

        // カメラを動かすスイッチがオンのとき
        if (CameraMoveSwitch == true)
        {
            if (Player.transform.position.y >= 0)
            {
                if (Player.transform.position.y >= mainCamera.transform.position.y)
                {
                    Y_camera = mainCamera.transform.position.y;
                    Y_camera += 0.10f;
                    mainCamera.transform.position = new Vector3(Player.transform.position.x, Y_camera, zAdjust);
                }
                if (Player.transform.position.y <= mainCamera.transform.position.y)
                {
                    Y_camera = mainCamera.transform.position.y;
                    Y_camera -= 0.10f;
                    mainCamera.transform.position = new Vector3(Player.transform.position.x, Y_camera, zAdjust);
                }
            }
        }
        else
        {
            fix2 = false;
        }
    }

}
