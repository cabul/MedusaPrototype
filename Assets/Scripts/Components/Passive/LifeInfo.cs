using UnityEngine;
using System.Collections;

// Se ocupa de mantener la vida

public class LifeInfo : BaseInfo {

  public int maxLife = 20;
  public int currentLife = 20;

  protected override string GetInfo()
  {
    if(isDead) return "K.O.";
    return "HP: "+currentLife+"/"+maxLife;
  }

  public bool isDead {
    get { return currentLife <= 0; }
  }

  public bool Damage(int dmg) 
  {
    currentLife -= dmg;

    if(currentLife <= 0) {
      Debug.Log ( gameObject.name + " dies");
      currentLife = 0;
      return true;
    }

    return false;
  }

}
