using UnityEngine;
using System.Collections;

public delegate void OnSkillFinish(BasicSkill skill);

public abstract class BasicSkill : MonoBehaviour {

  public event OnSkillFinish OnFinish;

	
}
