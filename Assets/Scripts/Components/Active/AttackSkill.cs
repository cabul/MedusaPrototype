using UnityEngine;
using System.Collections;

// Habilidad de ataque básico

public class AttackSkill : BaseSkill {

  public int dmg;

  public Material valid;
  public Material highlight;
  public Material mark;

  private Token target;

	public AttackSkill()
  {
    power = "Attack";    
  }

  public override void ClickHandler (Token clk)
  {
    if(clk == null) {
      ClearSelection();
      Exit();
      return;
    }
    if(clk.position.Distance(tkn.position) != 1) {
      ClearSelection();
      Exit();
      return;
    }
    Layer solid = board["Solid"];
    Token obj = solid[clk.position];
    
    Layer terrain = board["Terrain"];

    if (obj != target) {
      target = null;
      terrain[clk.position].Get<Selectable>().Unselect();
    }

    if(ValidTarget(obj)) {
      target = obj;
      terrain[clk.position].Get<Selectable>().Select(mark); 
    } else {
      terrain[clk.position].Get<Selectable>().Select((ValidTarget(solid[clk.position]))?valid:highlight); 
    }
  }

  public override void Activate (Board board)
  {
    base.Activate (board);

    Layer terrain = board["Terrain"];

    Layer solid = board["Solid"];

    foreach ( Direction dir in Direction.All ) {
      Position position = tkn.position + dir;
      if( position.Inside(terrain) ) {
        Selectable cell = terrain[position].Get<Selectable>();
        cell.Select(ValidTarget(solid[position])?valid:highlight);
      }
    }
  }

  public override bool Apply()
  {
    ClearSelection();
    if(target == null) return false;
    target.Get<LifeInfo>().Damage(dmg);
    return true;
  }

  public override void Cancel () 
  {
    ClearSelection();
  }

  public bool ValidTarget(Token obj) 
  {
    if(obj == null) {
      return false;
    }
    if(!obj.Has<LifeInfo>()) {
      return false;
    }
    if(obj.Get<LifeInfo>().isDead) {
      return false;
    }
    return true;
  }

  private void ClearSelection()
  {
    Layer terrain = board["Terrain"];
    foreach(Direction dir in Direction.All ) {
      Position position = tkn.position + dir;
      if(position.Inside(terrain)) {
        terrain[position].Get<Selectable>().Unselect();
      }
    }
  }


}
