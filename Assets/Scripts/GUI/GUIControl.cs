using UnityEngine;
using System.Collections;
using System;

// El OnClickListener
public delegate void OnClickHandler (Token clicked);
public delegate void OnSkillHandler (BaseSkill clicked);

public class GUIControl : MonoBehaviour {

  public int leftOffset = 10;
  public int topOffset = 10;
  public int buttonWidth = 50;
  public int buttonHeight = 50;
  public int labelWidth = 200;
  public int labelHeight = 20;

  // El evento que se lanza al hacer click
  public event OnClickHandler OnClick;

  public event OnSkillHandler OnSkill;

  private bool launchClick = true;

  private Token renderToken;

  public void Render(Token token)
  {
    renderToken = token;
  }

  // La Interfaz
  void OnGUI()
  {
    if(renderToken == null) return;
    launchClick = false;

    ShowLabels(renderToken);

    if( ShowSkills(renderToken) ) return;

    launchClick = true;
  }

  private void ShowLabels(Token token)
  {
    BaseInfo[] infos = token.All<BaseInfo>();
    for(int i = 0; i < infos.Length; i++) {
      GUI.Label(new Rect(leftOffset,topOffset+(labelHeight+topOffset)*i,labelWidth,labelHeight), infos[i].Content);
    }
  }

  // returns true if skill was selected
  private bool ShowSkills(Token token)
  {
    BaseSkill[] skills = token.All<BaseSkill>();
    if(skills != null) {
      for(int i = 0; i < skills.Length; i++) {
        if(GUI.Button(new Rect(leftOffset+((buttonWidth+leftOffset)*i),Screen.height - topOffset - buttonHeight,buttonWidth,buttonHeight),skills[i].power)) {
          SkillEvent(skills[i]);
          Event.current.Use();
          return true;
        }
      }
    }
    return false;
  }

  // Click detection
  void Update ()
  {
    if(launchClick && GUIUtility.hotControl == 0) {
      if (Input.GetMouseButtonDown (0)) {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast (ray, out hitInfo)) {
          Token token = hitInfo.transform.GetComponent<Token> ();
          if (token == null) {
            throw new InvalidOperationException ("Selected Object is no Token");
          } else {
            ClickEvent(token);
          }
        } else {
          ClickEvent(null);
        } 
          
      }
    }
  }

  // Lanzar el evento de forma segura
  private void ClickEvent(Token token)
  {
    if(OnClick != null) {
      OnClick(token);
    }
  }

  private void SkillEvent(BaseSkill skill)
  {
    if(OnSkill != null) {
      OnSkill(skill);
    }
  }

}
