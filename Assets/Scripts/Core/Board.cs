using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// El OnClickListener
public delegate void OnClickHandler (Token clicked);

public class Board : MonoBehaviour
{

  public int xs;
  public int ys;
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
  public Token[] this [Position pos] {
    get {
      Token[] tkns = new Token[layer_map.Count];
      int i = 0;
      foreach (Layer lay in layer_map.Values) {
        tkns[i++] = lay[pos];
      }
      return tkns;
    }
  }

  // Añadir una capa nueva :)
  public static Board operator + (Board brd, string str)
  {
    GameObject go = new GameObject (str);
    Layer lay = go.AddComponent<Layer> ();
    go.transform.parent = brd.transform;
    lay.Resize (brd.xs, brd.ys);
    brd.layer_map.Add (str, lay);
    return brd;
  }

  // Quitar una capa, nunca lo he usado
  public static Board operator - (Board brd, string str)
  {
    Transform ch = brd.transform.FindChild (str);
    if (ch == null)
      throw new ArgumentException (str + " not in board");
    Destroy (ch);
    return brd;
  }

  // Click detection
  void Update ()
  {
    if(launchClick && GUIUtility.hotControl == 0) {
      if (Input.GetMouseButtonDown (0)) {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast (ray, out hitInfo)) {
          Token tkn = hitInfo.transform.GetComponent<Token> ();
          if (tkn == null) {
            throw new InvalidOperationException ("Selected Object is no Token");
          } else {
            ClickEvent(tkn);
          }
        } else {
          ClickEvent(null);
        } 
          
      }
    }
  }

  // Lanzar el evento de forma segura
  private void ClickEvent(Token tkn) {
    if(OnClick != null) {
      OnClick(tkn);
    }
  }

}
