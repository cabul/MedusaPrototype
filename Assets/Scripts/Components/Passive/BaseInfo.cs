using UnityEngine;
using System.Collections;

// Informacion basica

public abstract class BaseInfo : MonoBehaviour{

	public string Content{
    get{ return GetInfo(); }
  }

  protected Token parentToken;

  void Awake()
  {
    parentToken = GetComponent<Token>();
  }

  protected abstract string GetInfo();


}
