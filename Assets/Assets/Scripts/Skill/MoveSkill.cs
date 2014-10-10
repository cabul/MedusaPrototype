using UnityEngine;
using System.Collections;

public class MoveSkill : BaseSkill {

  public MoveSkill()
  {
    power = "Move";
  }

  public override void ClickHandler(Token clk)
  {

  }

  public override void Activate(Board board)
  {
    base.Activate(board);
    Debug.Log ("Hello move");
  }

}
