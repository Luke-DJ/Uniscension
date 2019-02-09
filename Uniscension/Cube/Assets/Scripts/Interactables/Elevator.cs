using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Elevator : MonoBehaviour {

    public enum TrackRenderType { NONE, GEARS }

    public float height = 10;
    public float speed = 1;
    public bool startUp = false;
    public TrackRenderType trackType = TrackRenderType.NONE;
    public Material trackMaterial;
    [Header("Track Settings")]
    public float trackRenderScale = 0.01f;
    public Vector3 trackOffset;
    public Vector3 trackRotation;
    [Header("Track Teeth Settings")]
    public Material trackToothMaterial;
    public float toothGap = 0.2f;
    public Vector3 trackToothOffset;

    public bool isUp { get; private set; }
    public GameObject elevatorObject { get; private set; }
    public GameObject baseObject { get; private set; }
    public int direction { get; private set; }

    private float startYOffset;
    private Mesh trackMesh;
    private Mesh trackToothMesh;

    private void Awake()
    {
        elevatorObject = transform.Find("ElevatorObject").gameObject;
        baseObject = transform.Find("ElevatorBase").gameObject;

        startYOffset = elevatorObject.transform.localPosition.y;
    }
    // Use this for initialization
    void Start() {
        direction = 0;

        isUp = startUp;

        if (startUp)
        {
            Vector3 pos = elevatorObject.transform.position;
            float maxY = transform.position.y + startYOffset + height;

            elevatorObject.transform.position = new Vector3(pos.x, maxY, pos.z);
        }
    }

    // Update is called once per frame
    void Update() {
        switch (trackType)
        {
            case TrackRenderType.GEARS:
                LoadTrackMesh();

                if (trackMesh != null)
                {
                    Vector3 scale = transform.localScale * trackRenderScale;
                    scale.y *= height / 5;

                    Vector3 worldPos = transform.TransformPoint(trackOffset);
                    Vector3 pos = new Vector3(worldPos.x, worldPos.y + startYOffset, worldPos.z);

                    Quaternion rotation = transform.rotation;

                    if (trackRotation != Vector3.zero)
                    {
                        Quaternion rotateTrack = Quaternion.Euler(trackRotation.x, trackRotation.y, trackRotation.z);
                        rotation *= rotateTrack;
                    }

                    Matrix4x4 matrix = Matrix4x4.TRS(pos, rotation, scale);
                    Graphics.DrawMesh(trackMesh, matrix, trackMaterial, 0);

                    float yToothLimit = transform.position.y + height;

                    if (trackToothMesh != null)
                    {
                        scale = transform.localScale * trackRenderScale;

                        for (int i = 0; worldPos.y + startYOffset + (i * toothGap) < yToothLimit; i++)
                        {
                            pos = new Vector3(worldPos.x, worldPos.y + startYOffset + (i * toothGap), worldPos.z) + trackToothOffset;

                            matrix = Matrix4x4.TRS(pos, rotation, scale);
                            Graphics.DrawMesh(trackToothMesh, matrix, trackToothMaterial, 0);
                        }
                    }
                }
                break;
        }

        if (direction < 0)
        {
            Vector3 pos = elevatorObject.transform.position;

            elevatorObject.transform.position = new Vector3(transform.position.x, pos.y - (speed * Time.deltaTime), transform.position.z);

            float minY = transform.position.y + startYOffset;
            if (elevatorObject.transform.position.y < minY) {
                pos = elevatorObject.transform.position;
                elevatorObject.transform.position = new Vector3(pos.x, minY, pos.z);
                direction = 0;

                isUp = false;
            }
        }
        else if (direction > 0)
        {
            Vector3 pos = elevatorObject.transform.position;

            elevatorObject.transform.position = new Vector3(transform.position.x, pos.y + (speed * Time.deltaTime), transform.position.z);

            float maxY = transform.position.y + startYOffset + height;
            if (elevatorObject.transform.position.y > maxY) {
                pos = elevatorObject.transform.position;
                elevatorObject.transform.position = new Vector3(pos.x, maxY, pos.z);
                direction = 0;

                isUp = true;
            }
        }

        Debug.DrawLine(transform.position + new Vector3(0, startYOffset, 0), transform.position + new Vector3(0, startYOffset + height, 0), Color.red);
    }

    public void StartForward()
    {
        direction = 1;
    }

    public void StartBackward()
    {
        direction = -1;
    }

    public void ToggleElevator()
    {
        if (IsMoving()) { return; }

        if (isUp)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }
    public bool IsMoving()
    {
        return direction != 0;
    }

    private void LoadTrackMesh()
    {
        if(trackMesh != null) { return; }

        switch (trackType)
        {
            case TrackRenderType.GEARS:
                trackMesh = Resources.Load<Mesh>("models/Elavator_Shaft");
                trackToothMesh = Resources.Load<Mesh>("models/Elavator_Tooth");
                break;
        }
    }
}
