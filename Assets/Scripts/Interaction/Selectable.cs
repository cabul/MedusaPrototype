using UnityEngine;
using System.Collections;

// Este componente se le añade al terreno, para seleccionarlo
public class Selectable : MonoBehaviour
{

  private Material normal;
  private Renderer ren;

  void Start()
  {
    ren = transform.GetChild(0).renderer;
    normal = ren.material;
  }

  public Material Select(Material select)
  {
    ren.material = select;
    return normal;
  }

  public Material Unselect()
  {
    Material old = ren.material;
    ren.material = normal;
    return old;
  }


}
