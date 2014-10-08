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
  private System.Random rnd;

  void Start ()
  {

    rnd = new System.Random (seed);

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

    Layer solid = board ["Solid"];

    if (mirror) {
      solid &= MirrorObjectInstantiation;
    } else {
      solid &= NormalObjectInstantiation;
    }

  }

  private GameObject MirrorObjectInstantiation (Position pos)
  {
    int mx = (board.xs+1) / 2;
    int tree_th = 10;
    int stone_th = 15;

    GameObject tree = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    GameObject stone = Resources.Load<GameObject> ("Tokens/Environment/Stone");

    if (pos.x < mx) {

      int pct = rnd.Next (100);

      if (pct <= tree_th) {
        GameObject go = (GameObject)Instantiate (tree);
        go.name = tree.name;
        return go;
      }
      if (pct <= stone_th) {
        GameObject go = (GameObject)Instantiate (stone);
        go.name = stone.name;
        return go;
      }
      return (GameObject)null;

    } else { 
      Layer solid = board ["Solid"];
      Token tkn = solid [solid % pos];
      if (tkn == null)
        return (GameObject)null;
      GameObject go = (GameObject)Instantiate (tkn.gameObject);
      go.name = tkn.gameObject.name;
      return go;
    }

  }

  private GameObject NormalObjectInstantiation (Position pos)
  {
    int tree_th = 10;
    int stone_th = 15;
    
    GameObject tree = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    GameObject stone = Resources.Load<GameObject> ("Tokens/Environment/Stone");

    int pct = rnd.Next (100);
      
    if (pct <= tree_th) {
      GameObject go = (GameObject)Instantiate (tree);
      go.name = tree.name;
      return go;
    }
    if (pct <= stone_th) {
      GameObject go = (GameObject)Instantiate (stone);
      go.name = stone.name;
      return go;
    }
    return (GameObject)null;
      
  }



}
