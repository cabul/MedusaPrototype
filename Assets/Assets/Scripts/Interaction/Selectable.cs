using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour
{

  private Material normal;

  void Start()
  {
    normal = transform.GetChild(0).renderer.material;
  }

  public Material Select(Material select)
  {
    transform.GetChild(0).renderer.material = select;
    return normal;
  }

  public Material Unselect()
  {
    Material old = transform.GetChild(0).renderer.material;
    transform.GetChild(0).renderer.material = normal;
    return old;
  }

}
