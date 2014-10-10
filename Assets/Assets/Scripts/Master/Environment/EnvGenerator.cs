using UnityEngine;
using System.Collections;

public abstract class EnvGenerator : MonoBehaviour {

  public abstract void Generate(Layer lay);

  public bool mirror;

  public GameObject MirrorObject (Position pos, Layer lay)
  {
    Token tkn = lay [lay % pos];
    if (tkn == null)
      return (GameObject)null;
    GameObject go = (GameObject)Instantiate (tkn.gameObject);
    go.name = tkn.gameObject.name;
    return go;
  }

}
