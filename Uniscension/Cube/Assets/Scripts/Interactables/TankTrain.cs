using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TankTrain class is a component attached to moving trains
 * Trains can have multiple stations and will follow along their path when moving until the end
*/

[ExecuteInEditMode]
public class TankTrain : MonoBehaviour {

    public float speed;
    public Vector3[] stations;
    public int currentSpeed { get; private set; }
    public int currentStation { get; private set; }
    [Range(0, 1)]
    public float smoothApproach = 0;

    private int lastDirection = 0;
    private Vector3 originPosition;
    private Vector3 lastPosition;
    private GameObject platformObject;

	// Use this for initialization
	void Start () {
        originPosition = transform.position;

        SetSpeed(0);
        SetStation(0);

        lastPosition = transform.position;

        Transform platformTransform = transform.Find("Platform");

        if(platformTransform != null)
        {
            platformObject = platformTransform.gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 posDiff = transform.position - lastPosition;
        lastPosition = transform.position;
        originPosition += posDiff;

        if (!Application.isPlaying)
        {
            originPosition = transform.position;

            if (stations != null && stations.Length > 0)
            {
                for (int i = 0; i < stations.Length; i++)
                {
                    Vector3 pos = stations[i];

                    if (pos.Equals(new Vector3(0, 0, 0)) && i != 0)
                    {
                        stations[i] = stations[i - 1] + new Vector3(0,0,1);
                    }

                    pos = pos + originPosition;

                    Debug.DrawLine(pos - new Vector3(0.5f, 0, 0), pos + new Vector3(0.5f, 0, 0), Color.red);
                    Debug.DrawLine(pos - new Vector3(0, 0.5f, 0), pos + new Vector3(0, 0.5f, 0), Color.red);
                    Debug.DrawLine(pos - new Vector3(0, 0, 0.5f), pos + new Vector3(0, 0, 0.5f), Color.red);

                    if(i > 0)
                    {
                        Vector3 lastPos = stations[i - 1] + originPosition;

                        Debug.DrawLine(pos, lastPos, Color.yellow);
                    }
                }
            }
            

            return;
        }

        TrainLogic();
	}

    /*
     * TrainLogic function is used to control the trains movement logic based on its current status
     * It will move the train if its speed is not 0, and will also handle stopping the train when it reaches the end of a line
    */

    private void TrainLogic()
    {
        if (currentSpeed > 0)
        {
            if (AtEndOfLine())
            {
                StopMoving();
            }
            else
            {
                bool reachedStation = MoveTowards(currentStation + 1);

                if (reachedStation)
                {
                    SetStation(currentStation + 1);
                }
            }
        }
        else if (currentSpeed < 0)
        {
            if (AtStartOfLine())
            {
                StopMoving();
            }
            else
            {
                bool reachedStation = MoveTowards(currentStation - 1);
                if (reachedStation)
                {
                    SetStation(currentStation - 1);
                }
            }
        }
    }

    /*
     * MoveTowards function is used to move the train towards the next station
     * It returns whether the train reached the next station during the move or not
     * It will also iterate through all players, and move them along with the train if they are grounded to it to prevent sliding on the moving train
    */
   
    private bool MoveTowards(int index)
    {
        if(platformObject == null) { return false; }

        Vector3 target = stations[index] + originPosition;
        Vector3 pos = platformObject.transform.position;
        float distance = Vector3.Distance(target, pos);

        bool reachedStation = false;

        float distanceToMove = 0f;

        if (smoothApproach > 0)
        {
            distanceToMove = (distance / smoothApproach) * Time.deltaTime * speed;

            if(distanceToMove < 0.01f)
            {
                reachedStation = true;
            }
        }
        else
        {
            distanceToMove = speed * Time.deltaTime;
        }

        if (distanceToMove >= distance)
        {
            distanceToMove = distance;
            reachedStation = true;
        }

        platformObject.transform.position = Vector3.MoveTowards(pos, target, distanceToMove);

        Vector3 newPos = platformObject.transform.position;
        Vector3 difference = newPos - pos;

        foreach (GameObject player in GameManager._instance.players)
        {
            Player p = player.GetComponent<Player>();
            if(p != null)
            {
                if(p.GroundedTo() == platformObject)
                {
                    player.transform.position += difference;
                }
            }
        }

        return reachedStation;
    }

    /*
     * SetStation function is used to update the trains current station index
     * This function also snaps the train to the position of the station to eliminate movement error
    */

    private bool SetStation(int index)
    {
        if (stations != null && stations.Length > index)
        {
            // transform.position = stations[index] + originPosition;
            currentStation = index;

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetSpeed(int speed)
    {
        lastDirection = currentSpeed;
        currentSpeed = Mathf.Clamp(speed, -1, 1);
    }

    public void StartForward()
    {
        SetSpeed(1);
    }

    public void StartBackward()
    {
        SetSpeed(1);
    }

    public void StartMoving()
    {
        if (AtEndOfLine())
        {
            SetSpeed(-1);
        }
        else if (AtStartOfLine())
        {
            SetSpeed(1);
        }
        else if (lastDirection != 0)
        {
            SetSpeed(lastDirection);
        }
        else
        {
            SetSpeed(1);
        }
    }

    public void StopMoving()
    {
        lastDirection = currentSpeed;
        currentSpeed = 0;
    }

    public void ToggleMoving()
    {
        if (IsMoving())
        {
            StopMoving();
        }
        else
        {
            StartMoving();
        }
    }

    public bool IsMoving()
    {
        return currentSpeed != 0;
    }

    public bool AtEndOfLine()
    {
        return currentStation >= stations.Length - 1;
    }

    public bool AtStartOfLine()
    {
        return currentStation == 0;
    }
}
