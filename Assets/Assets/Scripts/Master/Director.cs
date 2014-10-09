using UnityEngine;
using System.Collections;
using System;

public class Director : MonoBehaviour
{
  private Board board;
  public Selectable lastSelected;
  public Material selectMaterial;
  public bool mirror;
  public int seed;

  private GameObject doll;

  void Awake ()
  {
    doll = Resources.Load<GameObject> ("Tokens/Character/Doll");
    UnityEngine.Random.seed = seed;
    board = GameObject.Find ("BoardNode").GetComponent<Board> ();
  }

  void Start ()
  {
    board += "Effect";
    board += "Solid";
    board += "Terrain";

    GameObject cell = Resources.Load<GameObject> ("Tokens/Terrain/Cell");

    Layer terrain = board ["Terrain"];
    terrain.Init(pos => (GameObject)Instantiate (cell));
    
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }

    Layer solid = board["Solid"];

    EnvGenerator env = board.GetComponent<EnvGenerator>();

    env.Generate( solid );

    board.OnClick += PlaceDoll;
    board.OnClick += DebugClick;

    Debug.Log ("Place doll");

  }

  private void Selector( Token tkn )
  {
    if ( lastSelected != null ) {
      lastSelected.Unselect();
    }
    if( tkn != null) {
      lastSelected = board["Terrain"][tkn.pos].Get<Selectable>();
      if (lastSelected != null) {
        lastSelected.Select(selectMaterial);
      }
    }
  }

  private void DebugClick( Token tkn )
  {
    if (tkn == null) {
      Debug.Log("Hit Nothing");
    } else {
      Token[] tkns = board[tkn.pos];
      string[] arr = new string[tkns.Length];
      for(int i = 0; i < tkns.Length; i++) {
        arr[i] = (tkns[i] == null)?"ø":tkns[i].gameObject.name;
      }
      Debug.Log ("Hit @ "+tkn.pos + ": [ "+string.Join(", ",arr) +" ]");
    }
  }

  private void PlaceDoll( Token tkn )
  {
    if (tkn == null ) return;
    Layer solid = board["Solid"];
    if (solid[tkn.pos] != null ) return;
    GameObject go = (GameObject)Instantiate(doll);
    go.name = "Doll";
    Token set = go.GetComponent<Token>();
    solid[ tkn.pos ] = set;
    go.transform.parent = solid.transform;
    board.OnClick -= PlaceDoll;
    board.OnClick += Selector;
  }

}
