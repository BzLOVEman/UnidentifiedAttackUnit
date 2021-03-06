﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
//ゲームシステム一括管理、シーン遷移
public class GameSystem : MonoBehaviour {
    private int EnemyCount;
    GameObject[] enemyObjects;
    private int bossCount;
    GameObject[] bossObjects;
    private int ObjectCount;
    GameObject[] Objects;
    private bool bossCome;
    private static int goal = 0;
    private Vector3 p;
    public GameObject boss;
    private int makes = 0;

    void Start() {
        makes = GetComponent<makeE>().makes;
        bossCome = false;
        goal++;
        transform.Find("Audio Source Enemy").gameObject.SetActive(true);
        transform.Find("Audio Source Boss").gameObject.SetActive(false);
    }
    void Update() {
        //エネミー数カウント
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyCount = enemyObjects.Length;
        if (makes == EnemyCount)
            makes = -1;

        bossObjects = GameObject.FindGameObjectsWithTag("boss");
        bossCount = bossObjects.Length;
        //オブジェクト数カウント
        Objects = GameObject.FindGameObjectsWithTag("WallObject");
        ObjectCount = Objects.Length;
        if (bossCount <= 0 && bossCome) {
            SceneChange(0);
        }
        if (EnemyCount <= 0 && !bossCome && makes < 0) {
            Debug.Log("run");
            BossBirth();
        }
        if (ObjectCount <= 0 && !bossCome) {
            SceneChange(1);
        }
    }
    void SceneChange(int key) {
        //0 ゲームクリア
        //1 ゲームオーバー
        //2 ゲームリロード
        if (key == 0) {
            SceneManager.LoadScene("StaffRole");
        }
        if (key == 1) {
            //カーソル表示
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
        if (key == 2) {
            SceneManager.LoadScene("game");
        }
    }
    void BossBirth() {
        //ボスが来たフラグ
        bossCome = true;
        //オブジェクト破壊
        var targetGOs = GameObject.FindGameObjectsWithTag("WallObject");
        foreach (var target in targetGOs) {
            Destroy(target);
        }
        //BOSS生成
        GameObject BOSS = GameObject.Instantiate(boss) as GameObject;
        // 弾丸の位置を調整
        p = new Vector3(150, 350, 150);
        BOSS.transform.position = p;
        //前方に向ける
        BOSS.transform.LookAt(transform.position);
        //BOSS.GetComponent<Renderer> ().material.color = Color.green;
        transform.Find("Audio Source Enemy").gameObject.SetActive(false);
        transform.Find("Audio Source Boss").gameObject.SetActive(true);
    }
}