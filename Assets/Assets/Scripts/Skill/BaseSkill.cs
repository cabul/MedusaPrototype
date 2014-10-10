using UnityEngine;
using System.Collections;

public delegate void OnSkillFinish (BaseSkill skill);

public abstract class BaseSkill : MonoBehaviour
{

  public event OnSkillFinish OnFinish;

  protected Board board;
  protected Token me;
  public string power;

  public abstract void ClickHandler (Token clk);

  void Awake ()
  {
    me = GetComponent<Token> ();
  }

  public virtual void Activate (Board board)
  {
    this.board = board;
  }

  protected void Finish ()
  {
    if(OnFinish != null)
      OnFinish(this);
  }

}
