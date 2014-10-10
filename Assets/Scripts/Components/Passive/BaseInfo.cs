using UnityEngine;
using System.Collections;

public abstract class BaseInfo : MonoBehaviour{

	public string info{
    get{ return GetInfo(); }
  }

  protected abstract string GetInfo();


}
