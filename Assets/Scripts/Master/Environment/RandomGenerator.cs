using UnityEngine;
using System.Collections;

public class RandomGenerator : EnvGenerator
{

  private float tree_th = 0.1f;
  private float stone_th = 0.15f;
  private GameObject tree;
  private GameObject stone;
  private Layer solid;

  void Awake ()
  {
    
    tree = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    stone = Resources.Load<GameObject> ("Tokens/Environment/Stone");

  }

  public override void Generate (Layer lay)
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

}
