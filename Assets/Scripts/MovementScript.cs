using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MovePoint {
    public Vector3 position;
    public float moveTime;
    public float waitTime;
}
public class MovementScript : MonoBehaviour
{
    [SerializeField] List<MovePoint> movePoints;
    [SerializeField] int movingToPointNo = 0;
    [SerializeField] float arrivedAccuracy;
    [SerializeField] bool isMoving = false;
    Vector3 moveProgression;
    // Start is called before the first frame update
    void Start()
    {
        StartMove();
    }

    void StartMove() {
        movingToPointNo = 0;

        if (movePoints.Count <= 1) {
            Debug.LogError("MovePoint-Liste hat zu wenig Punkte!");
            return;
        }

        transform.position = movePoints[0].position;
        if (movingToPointNo + 1 == movePoints.Count)
        {
            movingToPointNo = 0;
        }
        else
        {
            movingToPointNo++;
        }
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) { 
            transform.position = Vector3.SmoothDamp(transform.position, movePoints[movingToPointNo].position, ref moveProgression, movePoints[movingToPointNo].moveTime);
            if (Vector3.Distance(transform.position, movePoints[movingToPointNo].position) <= arrivedAccuracy)
            {
                StartCoroutine(WaitForNextPoint());
            }
        }

    }

    public void ChangeMoveList(List<MovePoint> movePoints) {
        isMoving = false;
        this.movePoints = movePoints;
        StartMove();
    }

    IEnumerator WaitForNextPoint() {
        isMoving = false;
        yield return new WaitForSeconds(movePoints[movingToPointNo].waitTime);
        if (movingToPointNo+ 1 == movePoints.Count)
        {
            movingToPointNo = 0;
        }
        else { 
            movingToPointNo++;
        }
        isMoving = true;
    }
}
