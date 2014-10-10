using UnityEngine;
using System.Collections;

public class SimpleInfo : BaseInfo {

  public string description;

  protected override string GetInfo()
  {
    return description;
  }
	
}
