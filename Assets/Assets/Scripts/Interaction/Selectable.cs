using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour
{

  private Material normal;

  void Start()
  {
    normal = transform.GetChild(0).renderer.material;
  }

  public void Select(Material select)
  {
    transform.GetChild(0).renderer.material = select;
  }

  public void Unselect()
  {
    transform.GetChild(0).renderer.material = normal;
  }

}
