using UnityEngine;
using System.Collections;

public abstract class EnvGenerator : MonoBehaviour {

  public abstract void Generate(Layer layer);

  public bool mirror;

  public GameObject MirrorObject (Position position, Layer layer)
  {
    Token token = layer [layer.Mirror(position)];
    if (token == null)
      return (GameObject)null;
    GameObject go = (GameObject)Instantiate (token.gameObject);
    go.name = token.gameObject.name;
    return go;
  }

}
