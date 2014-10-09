using UnityEngine;
using System.Collections;

public class EnvGenerator : MonoBehaviour
{

  private float tree_th = 0.1f;
  private float stone_th = 0.15f;
  private GameObject tree;
  private GameObject stone;
  private Layer solid;
  public bool mirror;

  void Awake ()
  {
    
    tree = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    stone = Resources.Load<GameObject> ("Tokens/Environment/Stone");

  }

  public void Generate (Layer lay)
  {
    int mx = (lay.xs + 1) / 2;

    if (mirror) {
      lay.Init(pos => {
        if (pos.x < mx)
          return RandomObject (pos);
        else
          return MirrorObject (pos, lay);
      });
    } else {
      lay.Init( RandomObject );
    }

  }

  private GameObject RandomObject (Position pos)
  {
    float pct = Random.value;
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
    return null;
  }

  private GameObject MirrorObject (Position pos, Layer lay)
  {
    Token tkn = lay [lay % pos];
    if (tkn == null)
      return (GameObject)null;
    GameObject go = (GameObject)Instantiate (tkn.gameObject);
    go.name = tkn.gameObject.name;
    return go;
  }
  
}
