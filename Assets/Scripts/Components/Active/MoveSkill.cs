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
    if (!valid.Contains (clk.position)) {
      Debug.Log("Not valid clicked");
      Exit ();
      return;
    }

    if(clk.position == tkn.position) {
      Debug.Log("Already clicked");
      Exit ();
      return;
    }

    if(steps.Count == 0) {
      if(clk.position.Distance(tkn.position) == 1 && steps.Count < range) {
        steps.AddLast(clk.position);
        board ["Terrain"] [clk.position].Get<Selectable> ().Select (mark);
      }
      return;
    }

    if(clk.position == steps.Last.Value) {
      board ["Terrain"] [clk.position].Get<Selectable> ().Unselect();
      board ["Terrain"] [clk.position].Get<Selectable> ().Select (highlight);
      steps.RemoveLast();
      return;
    }

    if (clk.position.Distance(steps.Last.Value) == 1 && !steps.Contains (clk.position) && steps.Count < range) {
      steps.AddLast(clk.position);
      board ["Terrain"] [clk.position].Get<Selectable> ().Select (mark);
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
    tkn.layer[tkn.position] = null;
    tkn.layer[steps.Last.Value] = tkn;
    ClearSelection();
    return true;
  }

  public override void Activate (Board board)
  {
    base.Activate (board);

    valid.Clear ();
    steps.Clear ();

    SearchWay (valid, board ["Solid"], tkn.position, range);
    
    valid.Remove(tkn.position);

    Layer terrain = board ["Terrain"];

    foreach (Position position in valid) {
      Selectable select = terrain [position].Get<Selectable> ();
      if (select != null) {
        select.Select (highlight);
      }
    }

  }

  public override void Cancel ()
  {
    ClearSelection();
  }

  public void SearchWay (HashSet<Position> cells, Layer lay, Position init, int max)
  {
    if (max-- == 0)
      return;
    Position position;
    foreach (Direction dir in Direction.All) {
      position = init + dir;
      if (position.Outside(lay))
        continue;
      if (lay [position] == null) {
        cells.Add (position);
        SearchWay (cells, lay, position, max);
      }
    }
  }

  private void ClearSelection()
  {
    Layer terrain = board ["Terrain"];
    foreach (Position position in valid) {
      Selectable sable = terrain [position].Get<Selectable> ();
      if (sable != null) {
        sable.Unselect ();
      }
    }
  }

}
