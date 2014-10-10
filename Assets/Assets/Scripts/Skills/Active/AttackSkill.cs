﻿using UnityEngine;
using System.Collections;

public class AttackSkill : BaseSkill {

  public int dmg;

  public Material highlight;
  public Material mark;

  private Token target;

	public AttackSkill()
  {
    power = "Attack";    
  }

  public override void ClickHandler (Token clk)
  {
    if(clk.pos % tkn.pos != 1) {
      ClearSelection();
      Exit();
      return;
    }
    Layer solid = board["Solid"];
    Token obj = solid[clk.pos];
    
    Layer terrain = board["Terrain"];

    if (obj != target) {
      target = null;
      terrain[clk.pos].Get<Selectable>().Unselect();
    }

    if(ValidTarget(obj)) {
      target = obj;
      terrain[clk.pos].Get<Selectable>().Select(mark); 
    }
  }


  public override void Activate (Board board)
  {
    base.Activate (board);

    Layer terrain = board["Terrain"];

    foreach ( Direction dir in Direction.All ) {
      Position pos = tkn.pos + dir;
      if( pos < terrain ) {
        Selectable cell = terrain[pos].Get<Selectable>();
        cell.Select(highlight);
      }
    }
  }

  public override bool Apply()
  {
    ClearSelection();
    if(target == null) return false;
    target.Get<Life>().Damage(dmg);
    return true;
  }

  public override void Cancel () 
  {
    ClearSelection();
  }

  public bool ValidTarget(Token obj) 
  {
    return obj.Has<Life>() && !obj.Get<Life>().isDead;
  }

  private void ClearSelection()
  {
    Layer terrain = board["Terrain"];
    foreach(Direction dir in Direction.All ) {
      Position pos = tkn.pos + dir;
      if(pos<terrain) {
        terrain[pos].Get<Selectable>().Unselect();
      }
    }
  }


}
