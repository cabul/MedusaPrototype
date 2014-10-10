using UnityEngine;
using System.Collections;

// Habilidad basica (raiz)

// Cuando se cancela
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

  // Cuando se termina de ejecutar
  public virtual bool Apply()
  {
    return false;
  }

  // Cuando se selecciona
  public virtual void Activate (Board board)
  {
    this.board = board;
  }

  // Cuand se cancela
  public virtual void Cancel()
  {

  }

  // Salida de emergencia
  protected virtual void Exit ()
  {
    if(OnCancel != null)
      OnCancel(this);
  }

}
