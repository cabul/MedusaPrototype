using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Habilidad de movimiento

public class MoveSkill : BaseSkill
{

  public int range;
  public Material highlight;
  public Material mark;
  private HashSet<Position> valid;
  private LinkedList<Position> steps;

  public MoveSkill ()
  {
    power = "Move";
    valid = new HashSet<Position> ();
    steps = new LinkedList<Position> ();
  }

  public override void ClickHandler (Token clk)
  {
    if (clk == null) {
      Debug.Log("Nothing clicked");
      Exit ();
      return;
    }
    if (!valid.Contains (clk.pos)) {
      Debug.Log("Not valid clicked");
      Exit ();
      return;
    }

    if(clk.pos == tkn.pos) {
      Debug.Log("Already clicked");
      Exit ();
      return;
    }

    if(steps.Count == 0) {
      if(clk.pos % tkn.pos == 1 && steps.Count < range) {
        steps.AddLast(clk.pos);
        board ["Terrain"] [clk.pos].Get<Selectable> ().Select (mark);
      }
      return;
    }

    if(clk.pos == steps.Last.Value) {
      board ["Terrain"] [clk.pos].Get<Selectable> ().Unselect();
      board ["Terrain"] [clk.pos].Get<Selectable> ().Select (highlight);
      steps.RemoveLast();
      return;
    }

    if (clk.pos % steps.Last.Value == 1 && !steps.Contains (clk.pos) && steps.Count < range) {
      steps.AddLast(clk.pos);
      board ["Terrain"] [clk.pos].Get<Selectable> ().Select (mark);
    }
  }

  protected override void Exit()
  {
    ClearSelection();
    base.Exit();
  }

  public override bool Apply()
  {
    if(steps.Count == 0) {
      ClearSelection();
      return false;
    }
    tkn.layer[tkn.pos] = null;
    tkn.layer[steps.Last.Value] = tkn;
    ClearSelection();
    return true;
  }

  public override void Activate (Board board)
  {
    base.Activate (board);

    valid.Clear ();
    steps.Clear ();

    List<Position> cells = new List<Position>();

    SearchWay (cells, board ["Solid"], tkn.pos, range);
    
    foreach(Position pos in cells)
    {
      valid.Add(pos);
    }

    valid.Remove(tkn.pos);

    Layer terrain = board ["Terrain"];

    foreach (Position pos in valid) {
      Selectable select = terrain [pos].Get<Selectable> ();
      if (select != null) {
        select.Select (highlight);
      }
    }

  }

  public override void Cancel ()
  {
    ClearSelection();
  }

  public void SearchWay (List<Position> cells, Layer lay, Position init, int max)
  {
    if (max-- == 0)
      return;
    Position pos;
    foreach (Direction dir in Direction.All) {
      pos = init + dir;
      if (pos > lay)
        continue;
      if (lay [pos] == null) {
        cells.Add (pos);
        SearchWay (cells, lay, pos, max);
      }
    }
  }

  private void ClearSelection()
  {
    Layer terrain = board ["Terrain"];
    foreach (Position pos in valid) {
      Selectable sable = terrain [pos].Get<Selectable> ();
      if (sable != null) {
        sable.Unselect ();
      }
    }
  }

}
