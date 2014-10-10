using UnityEngine;
using System.Collections;
using System;

public class Director : MonoBehaviour
{
  private Board board;
  private Token selectedToken;
  public Material selectMaterial;
  public int seed;

  public float nextGUI = 0;
  public float deltaTime = 0.01f;

  public int left = 10;
  public int top = 10;
  public int width = 50;
  public int height = 50;

  private BaseSkill selectedSkill;

  void Awake ()
  {
    UnityEngine.Random.seed = seed;
    board = GameObject.Find ("BoardNode").GetComponent<Board> ();
  }

  void Start ()
  {
    board += "Effect";
    board += "Solid";
    board += "Terrain";

    GameObject cell = Resources.Load<GameObject> ("Tokens/Terrain/Cell");

    Layer terrain = board ["Terrain"];
    terrain.Init (pos => (GameObject)Instantiate (cell));
    
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }

    Layer solid = board ["Solid"];

    EnvGenerator env = board.GetComponent<EnvGenerator> ();

    env.Generate (solid);

    board.OnClick += Selector;
    //board.OnClick += DebugClick;

  }

  void OnGUI()
  {
    if(selectedToken == null) return;
    BaseSkill[] skills = selectedToken.All<BaseSkill>();
    if(skills == null) return;
    
    for(int i = 0; i < skills.Length; i++) {
      if(GUI.Button(new Rect(left+((width+left)*i),top,width,height),skills[i].power)) {
        SelectSkill(skills[i]);
        return;
      }
    }
  }

  private void SelectSkill(BaseSkill skill)
  {
    if( Time.time < nextGUI) return;
    nextGUI = Time.time + deltaTime;

    if(selectedSkill == null) {
      selectedSkill = skill;
      SkillStart(skill);
    } else {
      if( selectedSkill == skill ) {
        SkillConfirm(selectedSkill);
        selectedSkill = null;
      } else {
        SkillStop(selectedSkill);
        selectedSkill = skill;
        SkillStart(selectedSkill);
      }
    }
  }


  private void SkillStart (BaseSkill skill)
  {
    //Debug.Log("Start skill "+skill.power+ " @ "+Time.time);
    skill.Activate (board);

    board.OnClick -= Selector;

    board.OnClick += skill.ClickHandler;
    skill.OnCancel += SkillStop;
  }

  private void SkillStop (BaseSkill skill)
  {
    //Debug.Log("Stop skill "+skill.power+ " @ "+Time.time);
    skill.Cancel();

    board.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    
    board.OnClick += Selector;

    UnSelectAll();

    Selector(selectedToken);

  }

  private void SkillConfirm (BaseSkill skill)
  {
    //Debug.Log("Confirm skill "+skill.power+ " @ "+Time.time);
    skill.Apply();

    board.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    
    board.OnClick += Selector;

    UnSelectAll();

    Selector(selectedToken);

  }

  private void UnSelectAll()
  {
    foreach(Token tkn in board["Terrain"]) {
      tkn.Get<Selectable>().Unselect();
    }
  }

  private void Selector (Token tkn)
  {
    if (selectedToken != null) {
      board["Terrain"][selectedToken.pos].Get<Selectable>().Unselect();
    }
    if (tkn != null) {
      selectedToken = board["Solid"][tkn.pos];
      board["Terrain"][tkn.pos].Get<Selectable>().Select (selectMaterial);
    }
  }

  private void DebugClick (Token tkn)
  {
    if (tkn == null) {
      Debug.Log ("Hit Nothing");
    } else {
      Token[] tkns = board [tkn.pos];
      string[] arr = new string[tkns.Length];
      for (int i = 0; i < tkns.Length; i++) {
        arr [i] = (tkns [i] == null) ? "ø" : tkns [i].gameObject.name;
      }
      Debug.Log ("Hit @ " + tkn.pos + ": [ " + string.Join (", ", arr) + " ]");
    }
  }

}
