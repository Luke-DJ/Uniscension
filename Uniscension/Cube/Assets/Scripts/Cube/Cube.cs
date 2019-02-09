using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cube : MonoBehaviour {

    public enum CubeSide { /*TOP, BOTTOM,*/ LEFT, RIGHT, FRONT, BACK, NONE }
    public float scale = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public Vector3 GetCubeSideGravity(Cube.CubeSide cubeSide)
    {
        switch (cubeSide)
        {
            case Cube.CubeSide.FRONT:
                return -transform.forward;
            case Cube.CubeSide.BACK:
                return transform.forward;
            case Cube.CubeSide.RIGHT:
                return -transform.right;
            case Cube.CubeSide.LEFT:
                return transform.right;
            /*case Cube.CubeSide.TOP:
                return -transform.up;
            case Cube.CubeSide.BOTTOM:
                return transform.up;*/
            default:
                return new Vector3(0, 0, 0);
        }
    }

    public Cube.CubeSide GetNextCubeSide(Cube.CubeSide cubeSize, bool right = true)
    {
        if (right)
        {
            switch (cubeSize)
            {
                case Cube.CubeSide.RIGHT:
                    return Cube.CubeSide.FRONT;
                case Cube.CubeSide.FRONT:
                    return Cube.CubeSide.LEFT;
                case Cube.CubeSide.LEFT:
                    return Cube.CubeSide.BACK;
                case Cube.CubeSide.BACK:
                    return Cube.CubeSide.RIGHT;
            }
        }
        else
        {
            switch (cubeSize)
            {
                case Cube.CubeSide.RIGHT:
                    return Cube.CubeSide.BACK;
                case Cube.CubeSide.BACK:
                    return Cube.CubeSide.LEFT;
                case Cube.CubeSide.LEFT:
                    return Cube.CubeSide.FRONT;
                case Cube.CubeSide.FRONT:
                    return Cube.CubeSide.RIGHT;
            }
        }

        return Cube.CubeSide.NONE;
    }

    public Vector3 GetCubeSideOrigin(Cube.CubeSide cubeSide, float addDistance = 0)
    {
        Vector3 cubePosition = transform.position;

        float distance = (scale * 0.5f) + addDistance;

        switch (cubeSide)
        {
            case Cube.CubeSide.RIGHT:
                return cubePosition + (transform.right * distance);
            case Cube.CubeSide.LEFT:
                return cubePosition + (-transform.right * distance);
            case Cube.CubeSide.FRONT:
                return cubePosition + (transform.forward * distance);
            case Cube.CubeSide.BACK:
                return cubePosition + (-transform.forward * distance);
            default:
                return transform.position;
        }
    }

    public Cube.CubeSide GetCubeSide(Vector3 position)
    {
        Vector3 cubePosition = transform.position;

        // Calculate positions in the center of each cube face
        Vector3 right = GetCubeSideOrigin(Cube.CubeSide.RIGHT);
        Vector3 left = GetCubeSideOrigin(Cube.CubeSide.LEFT);
        Vector3 front = GetCubeSideOrigin(Cube.CubeSide.FRONT);
        Vector3 back = GetCubeSideOrigin(Cube.CubeSide.BACK);

        // Find and return the minimum distance between the object and each cube face

        /*Vector3 topRelative = top - position;
        float sqrDistanceTop = topRelative.sqrMagnitude;

        Vector3 bottomRelative = bottom - position;
        float sqrDistanceBottom = bottomRelative.sqrMagnitude;*/

        Vector3 frontRelative = front - position;
        float sqrDistanceFront = frontRelative.sqrMagnitude;

        Vector3 backRelative = back - position;
        float sqrDistanceBack = backRelative.sqrMagnitude;

        Vector3 rightRelative = right - position;
        float sqrDistanceRight = rightRelative.sqrMagnitude;

        Vector3 leftRelative = left - position;
        float sqrDistanceLeft = leftRelative.sqrMagnitude;

        float minSqrDistance = Mathf.Min(/*sqrDistanceTop, sqrDistanceBottom,*/ sqrDistanceFront, sqrDistanceBack, sqrDistanceRight, sqrDistanceLeft);


        if (minSqrDistance == sqrDistanceRight)
        {
            return Cube.CubeSide.RIGHT;
        }
        else if (minSqrDistance == sqrDistanceLeft)
        {
            return Cube.CubeSide.LEFT;
        }
        else if (minSqrDistance == sqrDistanceFront)
        {
            return Cube.CubeSide.FRONT;
        }
        else if (minSqrDistance == sqrDistanceBack)
        {
            return Cube.CubeSide.BACK;
        }
        /*else if (minSqrDistance == sqrDistanceTop)
        {
            return Cube.CubeSide.TOP;
        }
        else if (minSqrDistance == sqrDistanceBottom)
        {
            return Cube.CubeSide.BOTTOM;
        }*/
        else
        {
            return Cube.CubeSide.NONE;
        }
    }
}
