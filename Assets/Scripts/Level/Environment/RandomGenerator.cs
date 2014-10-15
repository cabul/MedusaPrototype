using UnityEngine;
using System.Collections;

public class RandomGenerator : EnvGenerator
{  
  public int treeCount;
  public int stoneCount;
  public int dollCount;
  private GameObject treePrefab;
  private GameObject stonePrefab;
  private GameObject dollPrefab;

  void Awake ()
  {
    treePrefab = Resources.Load<GameObject> ("Tokens/Environment/Tree");
    stonePrefab = Resources.Load<GameObject> ("Tokens/Environment/Stone");
    dollPrefab = Resources.Load<GameObject> ("Tokens/Character/Doll");
  }

  public override void Generate(Layer layer)
  {
    SetRandom(treePrefab,layer,treeCount);
    SetRandom(stonePrefab,layer,stoneCount);
    SetRandom(dollPrefab,layer,dollCount);
  }

  private void SetRandom(GameObject prefab,Layer layer,int count)
  {
    int xMax = (mirror)?(layer.width+1)/2:layer.width;
    int yMax = layer.height;

    Position position;

    while(count > 0) {
      position = RandomPosition(xMax,yMax);
      if(layer[position] == null) {
        layer.Put(CloneGO(prefab),position);
        count-=1;
        if(mirror) {
          layer.Put(CloneGO(prefab),layer.Mirror(position));
          count-=1;
        }
      }
    }

  }

  private GameObject CloneGO(GameObject prefab)
  {
    GameObject go = (GameObject)Instantiate(prefab);
    go.name = prefab.name;
    return go;
  }

  private Position RandomPosition (int xMax, int yMax)
  {
    return new Position ((int)(Random.value * xMax), (int)(Random.value * yMax));
  }
  
}
