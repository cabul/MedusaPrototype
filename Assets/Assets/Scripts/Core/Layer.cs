using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour,IEnumerable
{

  private Token[,] map;

  public int xs {
    get;
    private set;
  }

  public int ys {
    get;
    private set;
  }

  public void Resize (int xs, int ys)
  {
    this.xs = xs;
    this.ys = ys;
    map = new Token[xs, ys];
  }

  public Layer Clear()
  {
    foreach (Token tkn in this) {
      this [tkn.pos] = null;
    }
    return this;
  }
  
#pragma warning disable 219
  public Token this [Position pos] {
    get { 
      if(this<pos) throw new ArgumentOutOfRangeException(pos+" not in layer");
      return map [pos.x, pos.y]; 
    }
    set {
      Token old = map [pos.x, pos.y];
      if (old != null) {
        old.pos = null;
      } 
      if (value != null) {
        value.pos = pos;
      }
      map [pos.x, pos.y] = value;
    }
  }

  // Inside of bounds
  public static bool operator > (Layer lay, Position pos)
  {
    return pos.x >= 0
        && pos.x < lay.xs
        && pos.y >= 0
        && pos.y < lay.ys;
  }

  // Outside of bounds
  public static bool operator < (Layer lay, Position pos)
  {
    return pos.x < 0
      || pos.x >= lay.xs
      || pos.y < 0
      || pos.y >= lay.ys;
  }

  public static Token[] operator | (Layer lay, Func<Token,bool> test)
  {
    List<Token> list = new List<Token> ();
    foreach (Token tkn in lay) {
      if (test (tkn)) {
        list.Add (tkn);
      }
    }
    return list.ToArray ();
  }

  public static Position[] operator | (Layer lay, Func<Position,bool> test)
  {
    List<Position> list = new List<Position> ();
    for (int x = 0, xs = lay.xs; x < xs; x++) {
      for (int y = 0, ys = lay.ys; y < ys; y++) {
        Position pos = new Position (x, y);
        if (test (pos)) {
          list.Add (pos);
        }
      }
    }
    return list.ToArray ();
  }

  public Layer Init (Func<Position,GameObject> init)
  {
    Position pos;
    for (int x = 0; x < xs; x++) {
      for (int y = 0; y < ys; y++) {
        pos = new Position (x, y);
        Put( init (pos), pos);
      }
    }
    return this;
  }

  public Layer Put(GameObject go, Position pos) {
    if (go == null) {
      this [pos] = null;
    } else {
      Token tkn = go.GetComponent<Token> ();
      if (tkn == null)
        throw new ArgumentException (go.name + " must have a Token Component");
      this [pos] = tkn;
      tkn.transform.parent = transform;
    }
    return this;
  }
  
  public static Position operator % (Layer lay, Position pos)
  {
    return new Position (lay.xs - pos.x - 1, lay.ys - pos.y - 1);
  }

  public IEnumerator GetEnumerator ()
  {
    List<Token> list = new List<Token> ();
    foreach (Token tkn in map) {
      if (tkn != null) {
        list.Add (tkn);
      }
    }
    return list.GetEnumerator ();
  }

  public Position[] Ray (Position pos, Direction dir)
  {
    List<Position> list = new List<Position> ();
    while (this>(pos+=dir)) {
      list.Add (pos);
    }
    return list.ToArray ();
  }

  public Position[] ForX (int x)
  {
    return Ray (new Position (x, -1), Direction.Up);
  }

  public Position[] ForY (int y)
  {
    return Ray (new Position (-1, y), Direction.Right);
  }

}
