using UnityEngine;
using System;
using System.Collections;

// Representa una Dirección entre dos Posiciones

// No me gustan los Warnings
#pragma warning disable 659,661
public sealed class Direction
{

  public readonly int x;
  public readonly int y;
  public static readonly Direction Left = new Direction(-1, 0);
  public static readonly Direction Right = Left * -1;
  public static readonly Direction Up = new Direction(0, 1);
  public static readonly Direction Down = Up * -1;

  public static readonly Direction[] All = {Up,Right,Down,Left};

  public Direction(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public int length {
    get { return Math.Abs(x) + Math.Abs(y); }
  }

  public static bool operator ==(Direction a, Direction b)
  {
    if ((object)a == null) return (object)b == null;
    if ((object)b == null) return (object)a == null;
    return a.x == b.x && a.x == b.x;
  }

  public static bool operator !=(Direction a, Direction b)
  {
    if ((object)a == null) return (object)b == null;
    if ((object)b == null) return (object)a == null;
    return a.x != b.x || a.y != b.y;
  }

  public override bool Equals (object obj)
  {
    if( obj == null ) return false;
    return this == (obj as Direction);
  }

  // Permite operaciones básicas con Direcciones
  public static Direction operator +(Direction a, Direction b)
  {
    return new Direction(a.x + b.x, a.y + b.y);
  }

  public static Direction operator -(Direction a, Direction b)
  {
    return new Direction(a.x - b.x, a.y - b.y);
  }

  // Multiplicación por un scalar
  public static Direction operator *(Direction d, int s)
  {
    return new Direction(d.x * s, d.y * s);
  }

}
