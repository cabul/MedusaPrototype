using UnityEngine;
using System.Collections;

// Se ocupa de mantener la vida

public class LifeInfoWithDestroy : LifeInfo {


  protected override void OnDeath()
  {
    parentToken.Destroy();
  }

}
