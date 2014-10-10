using UnityEngine;
using System.Collections;

public delegate void OnSkillFinish (BaseSkill skill);

public abstract class BaseSkill : MonoBehaviour
{

  public event OnSkillFinish OnFinish;

  protected Board board;
  public Token tkn;
  public string power;

  public abstract void ClickHandler (Token clk);

  void Awake ()
  {
    tkn = GetComponent<Token> ();
  }

  public virtual void Activate (Board board)
  {
    this.board = board;
  }

  public abstract void Cancel();

  protected void Finish ()
  {
    if(OnFinish != null)
      OnFinish(this);
  }

}
