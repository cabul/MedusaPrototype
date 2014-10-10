using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnClickHandler (Token clicked);

public class Board : MonoBehaviour
{

  public int xs;
  public int ys;
  private Dictionary<string,Layer> layer_map;

  public event OnClickHandler OnClick;

  public Board ()
  {
    layer_map = new Dictionary<string,Layer > ();
  }

  public Layer this [string name] {
    get {
      if (!layer_map.ContainsKey (name)) {
        throw new ArgumentException (name + " not in board");
      } else {
        return layer_map [name];
      }
    }
  }

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

  public static Board operator + (Board brd, string str)
  {
    GameObject go = new GameObject (str);
    Layer lay = go.AddComponent<Layer> ();
    go.transform.parent = brd.transform;
    lay.Resize (brd.xs, brd.ys);
    brd.layer_map.Add (str, lay);
    return brd;
  }

  public static Board operator - (Board brd, string str)
  {
    Transform ch = brd.transform.FindChild (str);
    if (ch == null)
      throw new ArgumentException (str + " not in board");
    Destroy (ch);
    return brd;
  }

  void Update ()
  {
    if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0) {
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

  private void ClickEvent(Token tkn) {
    if(OnClick != null) {
      OnClick(tkn);
    }
  }

}
