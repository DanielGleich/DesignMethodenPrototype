using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class CheckPoint {
    public int hpCount;
    public Transform PlayerSpawnPoint;
    public Transform BossSpawnPoint;
}

public class CheckpointManager : MonoBehaviour
{
    public List<CheckPoint> checkPoints;
    public CheckPoint currentState;
    // Start is called before the first frame update
    void Start()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.hpCount == CheckpointState.bossHp)
            {
                currentState = checkPoint;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r")) {
            ReloadScene();
        }
    }

    public void ReloadScene() { 
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().StopAllCoroutines();
        GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>().StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
}
