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

  void Start ()
  {

    UnityEngine.Random.seed = seed;

    board = GameObject.Find ("BoardNode").GetComponent<Board> ();

    board += "Effect";
    board += "Solid";
    board += "Terrain";

    GameObject cell = Resources.Load<GameObject> ("Tokens/Terrain/Cell");

    Layer terrain = board ["Terrain"];
    terrain &= (pos => (GameObject)Instantiate (cell));
    
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }

    Layer solid = board["Solid"];

    EnvGenerator env = board.GetComponent<EnvGenerator>();

    env.Generate( solid );

    board.OnClick += Selector;

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
      Debug.Log ("Hit @ "+tkn.pos);
    }
  }

}
