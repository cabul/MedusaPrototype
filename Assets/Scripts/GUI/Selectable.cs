using UnityEngine;
using System.Collections;

// Este componente se le añade al terreno, para seleccionarlo
public class Selectable : MonoBehaviour
{

  private Material normalMaterial;
  private Renderer ren;

  void Start()
  {
    ren = transform.GetChild(0).renderer;
    normalMaterial = ren.material;
  }

  public Material Select(Material selectMaterial)
  {
    ren.material = selectMaterial;
    return normalMaterial;
  }

  public Material Unselect()
  {
    Material old = ren.material;
    ren.material = normalMaterial;
    return old;
  }


}
