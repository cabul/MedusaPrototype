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

    if (isRotating) {
      float dx = (Input.mousePosition.x - mousePosition.x) * rotSens;
      rotDelta = Mathf.Lerp(rotDelta,rotDelta+dx,Time.deltaTime *rotInertia);
      mousePosition = Input.mousePosition;
    } else {
      rotDelta = Mathf.Lerp(rotDelta,0,Time.deltaTime*rotInertia);
    }

    rotation += rotDelta;
    transform.eulerAngles = new Vector3 (30, rotation, 0);

  }

}


