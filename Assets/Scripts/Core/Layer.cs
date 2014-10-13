using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


// Un layer ... tadaa
// Mapea de Posición a Token
public class Layer : MonoBehaviour,IEnumerable
{

  private Token[,] map;

  public int width {
    get;
    private set;
  }

  public int height {
    get;
    private set;
  }

  // Eso se deberia llamar siempre!!
  public void SetSize (int width, int height)
  {
    this.width = width;
    this.height = height;
    map = new Token[width, height];
  }

  public Layer Clear()
  {
    foreach (Token token in this) {
      this [token.position] = null;
    }
    return this;
  }
  
  // Indexa de una posicion a un token
  public Token this [Position position] {
    get { 
      // Error si la posicion esta fuera del layer
      if(Outside(position)) throw new ArgumentOutOfRangeException(position+" not in layer");
      return map [position.x, position.y]; 
    }
    set {
      // Al poner un token en el layer se actualiza su posicion
      Token old = map [position.x, position.y];
      if (old != null) {
        old.position = null;
        old.Place();
      } 
      if (value != null) {
        value.position = position;
        value.Place();
      }
      map [position.x, position.y] = value;
    }
  }

  // Inside of bounds
  public bool Inside (Position position)
  {
    return position.x >= 0
        && position.x < width
        && position.y >= 0
        && position.y < height;
  }

  // Outside of bounds
  public bool Outside (Position position)
  {
    return position.x < 0
        || position.x >= width
        || position.y < 0
        || position.y >= height;
  }

  // @Dani: Estos dos operadores molan, porque es como en matematicas

  // Para cada x | x < 25

  // Para cada Token done se cumpla test
  public static IEnumerable<Token> operator | (Layer lay, Func<Token,bool> test)
  {
    foreach (Token token in lay) {
      if (test (token)) {
        yield return token;
      }
    }
  }

  // Para cada Posicion donde se cumpla test
  public static IEnumerable<Position> operator | (Layer lay, Func<Position,bool> test)
  {
    for (int x = 0, width = lay.width; x < width; x++) {
      for (int y = 0, height = lay.height; y < height; y++) {
        Position position = new Position (x, y);
        if (test (position)) {
          yield return position;
        }
      }
    }
  }

  // Inicializar el layer completo con lo que devuelva init
  public Layer Init (Func<Position,GameObject> init)
  {
    Position position;
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        position = new Position (x, y);
        Put( init (position), position);
      }
    }
    return this;
  }

  // Instanciar un go en el layer
  public Layer Put(GameObject go, Position position) {
    if (go == null) {
      this [position] = null;
    } else {
      Token token = go.GetComponent<Token> ();
      if (token == null)
        throw new ArgumentException (go.name + " must have a Token Component");
      this [position] = token;
      token.transform.parent = transform;
    }
    return this;
  }
  
  public Position Mirror(Position position)
  {
    return new Position (width - position.x - 1, height - position.y - 1);
  }

  // Para poder recorrer todos los tokens que no sean null
  public IEnumerator GetEnumerator ()
  {
    List<Token> list = new List<Token> ();
    foreach (Token token in map) {
      if (token != null) {
        list.Add (token);
      }
    }
    return list.GetEnumerator ();
  }

  // Un rayo a partir de una posicion, en una direccion
  public Position[] Ray (Position position, Direction direction)
  {
    List<Position> list = new List<Position> ();
    while (Inside(position+=direction)) {
      list.Add (position);
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
