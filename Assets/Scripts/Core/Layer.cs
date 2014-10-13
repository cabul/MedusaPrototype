using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


// Un layer ... tadaa
// Mapea de Posición a Token
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

  // Eso se deberia llamar siempre!!
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
  
  // Indexa de una posicion a un token
#pragma warning disable 219
  public Token this [Position pos] {
    get { 
      // Error si la posicion esta fuera del layer
      if(Outside(pos)) throw new ArgumentOutOfRangeException(pos+" not in layer");
      return map [pos.x, pos.y]; 
    }
    set {
      // Al poner un token en el layer se actualiza su posicion
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
  public bool Inside (Position pos)
  {
    return pos.x >= 0
        && pos.x < xs
        && pos.y >= 0
        && pos.y < ys;
  }

  // Outside of bounds
  public bool Outside (Position pos)
  {
    return pos.x < 0
      || pos.x >= xs
      || pos.y < 0
      || pos.y >= ys;
  }

  // @Dani: Estos dos operadores molan, porque es como en matematicas

  // Para cada x | x < 25

  // Para cada Token done se cumpla test
  public static IEnumerable<Token> operator | (Layer lay, Func<Token,bool> test)
  {
    foreach (Token tkn in lay) {
      if (test (tkn)) {
        yield return tkn;
      }
    }
  }

  // Para cada Posicion donde se cumpla test
  public static IEnumerable<Position> operator | (Layer lay, Func<Position,bool> test)
  {
    for (int x = 0, xs = lay.xs; x < xs; x++) {
      for (int y = 0, ys = lay.ys; y < ys; y++) {
        Position pos = new Position (x, y);
        if (test (pos)) {
          yield return pos;
        }
      }
    }
  }

  // Inicializar el layer completo con lo que devuelva init
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

  // Instanciar un go en el layer
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
  
  public Position Mirror(Position pos)
  {
    return new Position (xs - pos.x - 1, ys - pos.y - 1);
  }

  // Para poder recorrer todos los tokens que no sean null
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

  // Un rayo a partir de una posicion, en una direccion
  public Position[] Ray (Position pos, Direction dir)
  {
    List<Position> list = new List<Position> ();
    while (Inside(pos+=dir)) {
      list.Add (pos);
    }
    return list.ToArray ();
  }

  // Todos los X del indice
  public Position[] ForX (int x)
  {
    return Ray (new Position (x, -1), Direction.Up);
  }

  // Todos los Y del indice
  public Position[] ForY (int y)
  {
    return Ray (new Position (-1, y), Direction.Right);
  }

}
