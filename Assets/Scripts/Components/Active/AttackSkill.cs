using UnityEngine;
using System.Collections;

// Habilidad de ataque básico

public class AttackSkill : BaseSkill {

  public int dmg;

  private Token target;

  public Material inRangeMaterial;
  public Material validTargetMaterial;
  public Material selectedTargetMaterial;

  public int range;

	public AttackSkill()
  {
    power = "Attack";    
  }

  public override void ClickHandler (Token clicked)
  {
    if(clicked == null) {
      ClearSelection();
      Exit();
      return;
    }
    if(clicked.position.Distance(parentToken.position) != range) {
      ClearSelection();
      Exit();
      return;
    }
    Layer solid = board["Solid"];
    Token token = solid[clicked.position];

    if(ValidTarget(target)) {
      if(target != null) mark(target.position,validTargetMaterial);
    } else {
      if(target != null) mark(target.position,inRangeMaterial);
    }

    if(ValidTarget(token)) {
      target = token;
      mark(token.position,selectedTargetMaterial);
    } 
  }

  public override void Setup ()
  {

    Layer terrain = board["Terrain"];

    foreach ( Direction direction in Direction.All ) {
      Position position = parentToken.position + direction;
      if( position.Inside(terrain) ) {
        SelectCell( position );
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

  public bool ValidTarget(Token token) 
  {
    if(token == null) {
      return false;
    }
    if(!token.Has<LifeInfo>()) {
      return false;
    }
    if(token.Get<LifeInfo>().isDead) {
      return false;
    }
    return true;
  }

  private void ClearSelection()
  {
    Layer terrain = board["Terrain"];
    foreach(Direction direction in Direction.All ) {
      Position position = parentToken.position + direction;
      if(position.Inside(terrain)) {
        mark(position,null);
      }
    }
  }

  private void SelectCell(Position position)
  {
    Token token = board["Solid"][position];
    if ( ValidTarget(token)) {
      mark(position, validTargetMaterial );
    } else {
      mark(position, inRangeMaterial );
    }

    
  }

}
