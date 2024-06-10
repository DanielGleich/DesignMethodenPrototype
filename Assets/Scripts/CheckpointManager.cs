using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class CheckPoint {
    public string sceneName;
    public int bossHpCount;
    public float bossShootDelay;
    public float bossShootCooldown;
}

public class CheckpointManager : MonoBehaviour
{
    public List<CheckPoint> checkPoints;
    [SerializeField] GameObject cutsceneUI;
    [SerializeField] float cutsceneTime;

    private void Start()
    {
        if (CheckpointState.currentCheckPoint == null) {
            SetCheckPoint(3);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown("r")) {
            ReloadScene();
        }
    }

    public void ReloadScene() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().StopAllCoroutines();
        }

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            boss.GetComponent<BossScript>().StopAllCoroutines();
        }
        SceneManager.LoadScene(CheckpointState.currentCheckPoint.sceneName);
    }

    public void SetCheckPoint(int hp) {
        foreach (CheckPoint checkPoint in checkPoints)
            if (checkPoint.bossHpCount == hp)
                CheckpointState.currentCheckPoint = checkPoint;
    }

    public void StartCutscene() {
        StartCoroutine(waitForCutscene());
    }
    IEnumerator waitForCutscene()
    {
        cutsceneUI.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null )
        {
            player.GetComponent<PlayerMovement>().SaveHP();
        }
        yield return new WaitForSeconds(cutsceneTime);
        ReloadScene();
    }
}
