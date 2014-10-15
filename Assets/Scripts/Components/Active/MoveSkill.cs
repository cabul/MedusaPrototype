using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Habilidad de movimiento

public class MoveSkill : BaseSkill
{

  public int range;
  private HashSet<Position> positionsInRange;
  private LinkedList<Position> stepList;

  public Material inRangeMaterial;
  public Material selectedStepMaterial;
  public Material lastStepMaterial;

  public MoveSkill ()
  {
    power = "Move";
    positionsInRange = new HashSet<Position> ();
    stepList = new LinkedList<Position> ();
  }

  public override void ClickHandler (Token clicked)
  {
    if (clicked == null) {
      Exit ();
      return;
    }

    if (!positionsInRange.Contains (clicked.position)) {
      Exit ();
      return;
    }

    if(clicked.position == parentToken.position) {
      Exit ();
      return;
    }

    if(stepList.Count == 0) {
      if( clicked.position.Distance(parentToken.position) == 1 
      &&  stepList.Count < range) {
        stepList.AddLast(clicked.position);
        mark(clicked.position,lastStepMaterial);
      }
      return;
    }

    if(clicked.position == stepList.Last.Value) {
      mark(clicked.position,inRangeMaterial);
      stepList.RemoveLast();
      return;
    }

    if (clicked.position.Distance(stepList.Last.Value) == 1 && !stepList.Contains (clicked.position) && stepList.Count < range) {
      mark(stepList.Last.Value,selectedStepMaterial);
      stepList.AddLast(clicked.position);
      mark(clicked.position,lastStepMaterial);
    }
  }

  protected override void Exit()
  {
    ClearSelection();
    base.Exit();
  }

  public override bool Apply()
  {
    if(stepList.Count == 0) {
      ClearSelection();
      return false;
    }
    parentToken.layer[parentToken.position] = null;
    parentToken.layer[stepList.Last.Value] = parentToken;
    ClearSelection();
    return true;
  }

  public override void Setup ()
  {
    positionsInRange.Clear ();
    stepList.Clear ();

    SearchWay (positionsInRange, board ["Solid"], parentToken.position, range);
    
    positionsInRange.Remove(parentToken.position);

    foreach (Position position in positionsInRange) {
      mark(position,inRangeMaterial);
    }

  }

  public override void Cancel ()
  {
    ClearSelection();
  }

  public void SearchWay (HashSet<Position> inRange, Layer layer, Position startingPosition, int stepCount)
  {
    if (stepCount-- == 0)
      return;
    Position position;
    foreach (Direction dir in Direction.All) {
      position = startingPosition + dir;
      if (position.Outside(layer))
        continue;
      if (layer [position] == null) {
        inRange.Add (position);
        SearchWay (inRange, layer, position, stepCount);
      }
    }
  }

  private void ClearSelection()
  {
    foreach (Position position in positionsInRange) {
      mark(position,null);
    }
  }

}
