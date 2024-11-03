using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class CameraCtrl : MonoBehaviour
{

    public GameObject player;
    private Camera camera;
    //0-free; 1-player focus
    private int cameraMode;
    public enum Status
    {
        None = 0,
        Idle, Move
    }
    private Status status;

    private void Awake()
    {
        cameraMode = 0;
        camera = GetComponent<Camera>();
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 3.7775f,
            transform.position.z);
        status = Status.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMode == 1)
        {
            transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 3.7775f,
                transform.position.z);
        }
    }

    public void setPlayer(GameObject p)
    {
        player = p;
    }

    public void quickMoveToHero()
    {
        transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + 3.7775f,
                transform.position.z);
    }

    public void moveCamera(Vector3 pos)
    {
        transform.position = pos;
    }

    public void scaleCamera(float size)
    {
        camera.orthographicSize = size;
    }

    public void setCameraMode(int mode) { 
        cameraMode = mode;
    }
}
