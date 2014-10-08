using System;
using System.Collections;

#pragma warning disable 660,661
public class Position
{

  public readonly int x;
  public readonly int y;
  public static int ASCII = 65;

  public Position(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public static bool operator ==(Position a, Position b)
  {
    if ((object)a == null) return false;
    if ((object)b == null) return false;
    return (a.x == b.x) && (a.y == b.y);
  }

  public static bool operator !=(Position a, Position b)
  {
    if ((object)a == null) return true;
    if ((object)b == null) return true;
    return (a.x != b.x) || (a.y != b.y);
  }

  public static Position operator +(Position p, Direction d)
  {
    return new Position(p.x + d.x, p.y + d.y);
  }

  public static Direction operator -(Position a, Position b)
  {
    return new Direction(a.x - b.x, a.y - b.y);
  }

  public static bool operator >(Position pos, Layer lay)
  {
    return lay<pos;
  }

  public static bool operator <(Position pos, Layer lay)
  {
    return lay>pos;
  }

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
