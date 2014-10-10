using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSkill : BaseSkill
{

  public int range;
  public Material highlight;
  public Material mark;
  private HashSet<Position> valid;
  private List<Position> steps;
  private Position current;

  public MoveSkill ()
  {
    power = "Move";
    valid = new HashSet<Position> ();
    steps = new List<Position> ();
  }

  public override void ClickHandler (Token clk)
  {
    if (clk == null) {
      FinishMove ();
      return;
    }
    if (!valid.Contains (clk.pos)) {
      FinishMove ();
      return;
    }

    if (clk.pos == current) {
      Material select = board ["Terrain"] [me.pos].Get<Selectable>().Unselect();
      me.layer [current] = me;
      valid.Remove(current);
      board ["Terrain"] [me.pos].Get<Selectable>().Select(select);
      FinishMove ();
    }

    if (clk.pos % current == 1 && !steps.Contains (clk.pos)) {
      steps.Add (clk.pos);
      board ["Terrain"] [clk.pos].Get<Selectable> ().Select (mark);
      current = clk.pos;
    }
  }

  public void FinishMove ()
  {
    Layer terrain = board ["Terrain"];
    foreach (Position pos in valid) {
      Selectable select = terrain [pos].Get<Selectable> ();
      if (select != null) {
        select.Unselect ();
      }
    }
    Finish ();
  }

  public override void Activate (Board board)
  {
    base.Activate (board);

    valid.Clear ();
    steps.Clear ();

    current = me.pos;

    SearchWay (valid, board ["Solid"], me.pos, range);

    Layer terrain = board ["Terrain"];

    foreach (Position pos in valid) {
      Selectable select = terrain [pos].Get<Selectable> ();
      if (select != null) {
        select.Select (highlight);
      }
    }

  }

  public void SearchWay (HashSet<Position> cells, Layer lay, Position init, int max)
  {
    if (max-- == 0)
      return;
    Position pos;
    foreach (Direction dir in Direction.All) {
      pos = init + dir;
      if (pos > lay)
        continue;
      if (lay [pos] == null) {
        if (pos != me.pos && cells.Add (pos)) {
          SearchWay (cells, lay, pos, max);
        }
      }
    }
  }

}
