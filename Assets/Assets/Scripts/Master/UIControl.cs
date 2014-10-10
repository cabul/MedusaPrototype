using UnityEngine;
using System.Collections;

public delegate void OnSkillSelect(BaseSkill skill);

public class UIControl : MonoBehaviour {

  public event OnSkillSelect OnSkill;

  public int left = 10;
  public int top = 10;
  public int width = 50;
  public int height = 50;

  private BaseSkill[] skills;

  public void Render(BaseSkill[] skills)
  {
    this.skills = skills;
  }

  void OnGUI()
  {
    if(skills != null)
    {
      for(int i = 0; i < skills.Length; i++) {
        BaseSkill skill = skills[i];
        if(GUI.Button(new Rect(left+((width+left)*i),top,width,height),skill.power)) {
          OnSkill(skill);
        }
      }
    }
  }

}
