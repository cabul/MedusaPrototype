using UnityEngine;
using System.Collections;

// Un Label

public class SimpleInfo : BaseInfo {

  public string description;

  protected override string GetInfo()
  {
    return description;
  }
	
}
