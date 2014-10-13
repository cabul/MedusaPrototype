using UnityEngine;
using System.Collections;


// Generacion Compacta
// aka con menos random
public class CompactGenerator : EnvGenerator
{  
  public int treeCount;
  public int stoneCount;
  public int dollCount;
  private GameObject tree;
  private GameObject stone;
  private GameObject doll;

  void Awake ()
  {
    tree = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    stone = Resources.Load<GameObject> ("Tokens/Environment/Stone");
    doll = Resources.Load<GameObject> ("Tokens/Character/Doll");
  }

  public override void Generate(Layer lay)
  {
    SetRandom(tree,lay,treeCount);
    SetRandom(stone,lay,stoneCount);
    SetRandom(doll,lay,dollCount);
  }

  private void SetRandom(GameObject org,Layer lay,int cnt)
  {
    int xMax = (mirror)?(lay.xs+1)/2:lay.xs;
    int yMax = lay.ys;

    Position pos;

    while(cnt > 0) {
      pos = RandomPosition(xMax,yMax);
      if(lay[pos] == null) {
        lay.Put(CloneGO(org),pos);
        cnt-=1;
        if(mirror) {
          lay.Put(CloneGO(org),lay.Mirror(pos));
          cnt-=1;
        }
      }
    }

  }

  private GameObject CloneGO(GameObject org)
  {
    GameObject go = (GameObject)Instantiate(org);
    go.name = org.name;
    return go;
  }

  private Position RandomPosition (int xMax, int yMax)
  {
    return new Position ((int)(Random.value * xMax), (int)(Random.value * yMax));
  }
  
}
