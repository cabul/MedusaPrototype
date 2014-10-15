using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{

  public int width;
  public int height;
  private Dictionary<string,Layer> layers;

  void Awake()
  {
    layers = new Dictionary<string,Layer > ();
  }

  // Se accede a las capas mediante su nombre
  public Layer this [string name] {
    get {
      if (!layers.ContainsKey (name)) {
        throw new ArgumentException (name + " not in board");
      } else {
        return layers [name];
      }
    }
  }

  // Se accede a todos los tokens en una posicion
  public Token[] this [Position position] {
    get {
      Token[] tokens = new Token[layers.Count];
      int i = 0;
      foreach (Layer layer in layers.Values) {
        tokens[i++] = layer[position];
      }
      return tokens;
    }
  }

  // Añadir una capa nueva :)
  public static Board operator + (Board board, string name)
  {
    GameObject go = new GameObject (name);
    Layer layer = go.AddComponent<Layer> ();
    go.transform.parent = board.transform;
    layer.SetSize (board.width, board.height);
    board.layers.Add (name, layer);
    return board;
  }

}
