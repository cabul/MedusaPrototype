using UnityEngine;
using System.Collections;

public delegate void OnSkillCancel (BaseSkill skill);

public abstract class BaseSkill : MonoBehaviour
{

  public event OnSkillCancel OnCancel;

  protected Board board;
  public Token tkn;
  public string power;

  public abstract void ClickHandler (Token clk);

  void Awake ()
  {
    tkn = GetComponent<Token> ();
  }

  public virtual bool Apply()
  {
    return false;
  }

  public virtual void Activate (Board board)
  {
    this.board = board;
  }

  public virtual void Cancel()
  {

  }

  protected virtual void Exit ()
  {
    if(OnCancel != null)
      OnCancel(this);
  }

}
