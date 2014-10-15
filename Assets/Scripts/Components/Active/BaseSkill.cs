using UnityEngine;
using System.Collections;

// Habilidad basica (raiz)

// Cuando se cancela
public delegate void OnSkillCancel (BaseSkill skill);

public abstract class BaseSkill : MonoBehaviour
{

  public event OnSkillCancel OnCancel;

  protected Board board;
  protected CellMarker mark;
  public Token parentToken;
  public string power;

  public abstract void ClickHandler (Token clk);

  void Awake ()
  {
    parentToken = GetComponent<Token> ();
  }

  // Cuando se termina de ejecutar
  public virtual bool Apply()
  {
    return false;
  }

  // Cuando se selecciona
  public void Activate (Board board, CellMarker mark)
  {
    this.board = board;
    this.mark = mark;
    Setup();
  }

  public virtual void Setup()
  {

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
