using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
  private Camera cam;
  private float minFOV = 2;
  private float maxFOV = 8;
  private float distance = 50;
  private float zoomSens = 4;
  private float zoomInertia = 4;
  private Vector3 mousePosition;
  private bool isRotating = false;
  private float rotation = 45;
  private float rotSens = 0.1f;
  private float rotInertia = 2;
  private float rotDelta;

  private float keyDx;
  private float keySens = 2;

  public bool doRotate;
  public bool doZoom;

  void Start ()
  {
    Board board = GameObject.Find ("BoardNode").GetComponent<Board> ();
    transform.position = new Vector3 ((float)board.xs / 2
                                    , -0.75f
                                    , (float)board.ys / 2);
    cam = Camera.main;

    distance = cam.orthographicSize;
    rotation = transform.eulerAngles.y;

  }
  
  void Update ()
  {
    if(doZoom) ZoomCamera ();
    if(doRotate) RotateScreen ();
  }

  void ZoomCamera ()
  {
    distance -= Input.GetAxis ("Mouse ScrollWheel") * zoomSens;
    distance = Mathf.Clamp (distance, minFOV, maxFOV);
    cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, distance, Time.deltaTime * zoomInertia);
  }

  void RotateScreen ()
  {

    if( Input.GetMouseButtonDown(1)) {
      isRotating = true;
      mousePosition = Input.mousePosition;
    }
    if(Input.GetMouseButtonUp(1)) {
      isRotating = false;
    }

    float dx;

    if (isRotating) {
      dx = (Input.mousePosition.x - mousePosition.x) * rotSens;
      mousePosition = Input.mousePosition;
    } else {
      dx = - rotDelta;
    }

    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
      keyDx = -1;
    }
    if(Input.GetKeyDown(KeyCode.RightArrow)) {
      keyDx = 1;
    }

    if(Input.GetKeyUp(KeyCode.LeftArrow)) {
      keyDx = 0;
    }
    if(Input.GetKeyUp(KeyCode.RightArrow)) {
      keyDx = 0;
    }

    dx += keyDx * keySens;


    rotDelta = Mathf.Lerp(rotDelta,rotDelta+dx,Time.deltaTime *rotInertia);

    rotation += rotDelta;
    transform.eulerAngles = new Vector3 (30, rotation, 0);

  }

}


