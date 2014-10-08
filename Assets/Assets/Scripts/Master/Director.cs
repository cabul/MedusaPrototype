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

  void Start()
  {

    rnd = new System.Random(seed);

    board = GameObject.Find("BoardNode").GetComponent<Board>();

    board += "Effect";
    board += "Solid";
    board += "Terrain";

    GameObject cell = Resources.Load<GameObject>("Tokens/Terrain/Cell");

    Layer terrain = board ["Terrain"];
    terrain &= (pos => (GameObject)Instantiate(cell));
    
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }

    GameObject tree = Resources.Load<GameObject>("Tokens/Environment/Tree");

    int pct = 10;

    Layer solid = board["Solid"];

    if (mirror) {
      Layer dummy = Layer.Dummy(board.xs/2,board.ys);
      solid &= ((Position pos) => {
        if ( pos<dummy ) {
          if(rnd.Next(100) >= pct) return null;
          GameObject go = (GameObject)Instantiate(tree);
          go.name = tree.name;
          return go;
        } else {
          Token tkn = solid[solid%pos];
          if ( tkn == null ) return null;
          GameObject go = (GameObject)Instantiate(tkn.gameObject);
          go.name = tree.name;
          return go;
        }
      });
      Layer.Undummy(dummy);
    } else {
      solid &= (pos => {
        if(rnd.Next(100) < pct) {
          GameObject go = (GameObject)Instantiate(tree);
          go.name = tree.name;
          return go;
        } else return null;
      });
    }

  }

  void Update()
  {

    TestObjectSelection();

  }

  void TestObjectSelection()
  {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      if (Physics.Raycast(ray, out hitInfo)) {
        if (lastSelected != null) {
          lastSelected.Unselect();
        }
        Token tkn = hitInfo.transform.GetComponent<Token>();
        if (tkn == null) {
          lastSelected = null;
          throw new InvalidOperationException("Selected Object is no Token");
        } else {
          lastSelected = board["Terrain"][tkn.pos].Get<Selectable>();
          if (lastSelected != null) {
            lastSelected.Select(selectMaterial);
          }
        }
      } else {
        if (lastSelected != null) {
          lastSelected.Unselect();
        }
      }
    }
  }

}
