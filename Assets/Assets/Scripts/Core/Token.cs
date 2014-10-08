using UnityEngine;
using System.Collections;
using System;

public class Token : MonoBehaviour
{

  public Position pos {
    get;
    private set;
  }

  public T[] All<T>() where T : Component
  {
    return transform.GetComponents<T>();
  }

  public T Get<T>() where T : Component
  {
    return transform.GetComponent<T>();
  }

  public static Token operator &(Token tkn, Position pos)
  {
    tkn.pos = pos;
    return tkn;
  }

  public static Position operator ~(Token tkn)
  {
    Position pos = tkn.pos;
    tkn.pos = null;
    return pos;
  }

  public static bool operator !(Token tkn)
  {
    if ((object)tkn.pos == null) {
      return false;
    } else {
      tkn.transform.position = new Vector3(tkn.pos.x, 0, tkn.pos.y);
      return true;
    }
  }

}
