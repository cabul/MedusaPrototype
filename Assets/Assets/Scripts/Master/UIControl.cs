using UnityEngine;
using System.Collections;

public delegate void OnSkillEvent(BaseSkill skill);

public class UIControl : MonoBehaviour {

  public event OnSkillEvent OnSelect;
  public event OnSkillEvent OnCancel;

  public int left = 10;
  public int top = 10;
  public int width = 50;
  public int height = 50;

  public GUIStyle defStyle;
  public GUIStyle actStyle;
  public GUIStyle invStyle;

  private BaseSkill[] skills;

  private GUIStyle[] styles;

  private int selected = -1;

  public void Render(BaseSkill[] skills)
  {
    this.skills = skills;
    styles = new GUIStyle[skills.Length];
    for(int i = 0; i < skills.Length; i++) {
      styles[i] = defStyle;
    }
  }

  void OnGUI()
  {
    if(skills != null)
    {
      for(int i = 0; i < skills.Length; i++) {
        if(GUI.Button(new Rect(left+((width+left)*i),top,width,height),skills[i].power)) {
          SelectSkill(i);
        }
      }
    }
  }

  private void SelectSkill(int s)
  {
    if(s == selected) {
      if( OnCancel != null ) {
        OnCancel( skills[s]);
        selected = -1;
      }
      for(int i = 0; i < skills.Length; i++) {
        styles[i] = defStyle;
      }
    } else {
      selected = s;
      if(OnSelect != null) {
        OnSelect(skills[s]);
      }
      for(int i = 0; i < skills.Length; i++) {
        styles[i] = invStyle;
      }
      styles[s] = actStyle;

    }
  }


}
