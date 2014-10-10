using UnityEngine;
using System.Collections;
using System;

public class Token : MonoBehaviour
{

  private Position _pos;
  public Position pos {
    get { return _pos; }
    set {
      _pos = value;
      transform.position = new Vector3(value.x,0,value.y);
    }
  }

  public Layer layer {
    get { return transform.parent.GetComponent<Layer>(); }
  }

  public T[] All<T>() where T : Component
  {
    return transform.GetComponents<T>();
  }

  public T Get<T>() where T : Component
  {
    return transform.GetComponent<T>();
  }

  public override string ToString ()
  {
    return transform.name;
  }

}
