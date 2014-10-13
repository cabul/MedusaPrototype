using UnityEngine;
using System.Collections;
using System;

// Un objeto en el tablero
// Cada obj que se encuentra en un layer debe tener un Token
// Hace la función de Wrapper para una posición;
public class Token : MonoBehaviour
{

  public Position position;
  

  public void Place()
  {
    transform.position = (position==null)?new Vector3(1000,1000,1000):new Vector3(position.x,0,position.y);
  }

  // Wrappers para acceder al padre

  public Layer layer {
    get { return transform.parent.GetComponent<Layer>(); }
  }

  // Todos los componentes de un tipo
  public T[] All<T>() where T : Component
  {
    return transform.GetComponents<T>();
  }

  // Un componente de tipo T
  public T Get<T>() where T : Component
  {
    return transform.GetComponent<T>();
  }

  // Comprubea si tiene el Componente
  public bool Has<T>() where T : Component {
    return transform.GetComponent<T>() != null;
  }

  public override string ToString ()
  {
    return transform.name;
  }

}
