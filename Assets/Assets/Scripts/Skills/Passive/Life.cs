using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

  public int maxLife = 20;

  public bool isDead {
    get { return maxLife <= 0; }
  }

  public bool Damage(int dmg) 
  {
    maxLife -= dmg;

    if(maxLife <= 0) {
      Debug.Log ( gameObject.name + " dies");
      return true;
    }

    return false;
  }

}
