using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour
{

  private Material normal;
  private Material current;
  private Renderer ren;

  void Start()
  {
    ren = transform.GetChild(0).renderer;
    normal = ren.material;
    current = normal;
  }

  public Material Select(Material select)
  {
    current = select;
    return normal;
  }

  public Material Unselect()
  {
    Material old = current;
    current = normal;
    return old;
  }

  void Update()
  {
    // Esto aqui es horrible!
    ren.material = current;
  }

}
