using System;
using System.Collections;

// Representa una Posición en un Espacio 2D / Tablero
// Es inmutable
public sealed class Position
{

  public readonly int x;
  public readonly int y;
  public static int ASCII = 65;

  public Position(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  // Comparaciones
  public static bool operator ==(Position a, Position b)
  {
    if ((object)a == null) return (object)b == null;
    if ((object)b == null) return (object)a == null;
    return (a.x == b.x) && (a.y == b.y);
  }

  public static bool operator !=(Position a, Position b)
  {
    if ((object)a == null) return (object)b == null;
    if ((object)b == null) return (object)a == null;
    return (a.x != b.x) || (a.y != b.y);
  }

  public override bool Equals (object obj)
  {
    if( obj == null ) return false;
    return this == (obj as Position);
  }

  // Esperemos que no haya tableros más grandes que 100x100 :P
  public override int GetHashCode ()
  {
    return x * 100 + y;
  }

  // Mueve una Posición según una Dirección
  // Pos = Pos + Dir
  public static Position operator +(Position p, Direction d)
  {
    return new Position(p.x + d.x, p.y + d.y);
  }

  // Calcula la dirección que une a dos posiciones
  // Dir = Pos - Pos
  public static Direction operator -(Position a, Position b)
  {
    return new Direction(a.x - b.x, a.y - b.y);
  }

  // Comprueba si se encuentra fuera de un layer
  // Fuera = Pos > Lay
  public bool Outside(Layer lay)
  {
    return lay.Outside(this);
  }

  // Comprueba si se encuentra dentro de un layer
  // Dentro = Pos < Lay
  public bool Inside(Layer lay)
  {
    return lay.Inside(this);
  }

  // Calcula la distancia entre dos posiciones
  // Distancia = Pos % Pos
  public static int operator %(Position a, Position b)
  {
    return (a - b).len;
  }

  public override string ToString()
  {
    return char.ConvertFromUtf32(x + ASCII) + (y+1);
  }
  
  public static Position FromString(string str)
  {
    return new Position((int)str [0] - ASCII, Int32.Parse(str.Substring(1)) - 1);
  }

}
