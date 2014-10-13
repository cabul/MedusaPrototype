using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// El OnClickListener
public delegate void OnClickHandler (Token clicked);

public class Board : MonoBehaviour
{

  public int width;
  public int height;
  private Dictionary<string,Layer> layer_map;

  // El evento que se lanza al hacer click
  public event OnClickHandler OnClick;

  // Eso esta falso mientras se interactua con el GUI
  public bool launchClick;

  public Board ()
  {
    layer_map = new Dictionary<string,Layer > ();
    launchClick = true;
  }

  // Se accede a las capas mediante su nombre
  public Layer this [string name] {
    get {
      if (!layer_map.ContainsKey (name)) {
        throw new ArgumentException (name + " not in board");
      } else {
        return layer_map [name];
      }
    }
  }

  // Se accede a todos los tokens en una posicion
  public Token[] this [Position position] {
    get {
      Token[] tokens = new Token[layer_map.Count];
      int i = 0;
      foreach (Layer layer in layer_map.Values) {
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
    board.layer_map.Add (name, layer);
    return board;
  }

  // Click detection
  void Update ()
  {
    if(launchClick && GUIUtility.hotControl == 0) {
      if (Input.GetMouseButtonDown (0)) {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast (ray, out hitInfo)) {
          Token token = hitInfo.transform.GetComponent<Token> ();
          if (token == null) {
            throw new InvalidOperationException ("Selected Object is no Token");
          } else {
            ClickEvent(token);
          }
        } else {
          ClickEvent(null);
        } 
          
      }
    }
  }

  // Lanzar el evento de forma segura
  private void ClickEvent(Token token) {
    if(OnClick != null) {
      OnClick(token);
    }
  }

}
