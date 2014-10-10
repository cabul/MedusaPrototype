using UnityEngine;
using System.Collections;
using System;

public class Director : MonoBehaviour
{
  private Board board;
  private Selectable lastSelected;
  public Material selectMaterial;
  public int seed;
  private UIControl ui;

  void Awake ()
  {
    UnityEngine.Random.seed = seed;
    board = GameObject.Find ("BoardNode").GetComponent<Board> ();
    ui = GetComponent<UIControl> ();
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

    ui.OnSkill += SkillStart;

  }

  private void SkillStart (BaseSkill skill)
  {
    board.OnClick -= Selector;
    board.OnClick += skill.ClickHandler;
    skill.OnFinish += SkillStop;
    ui.OnSkill -= SkillStart;
    skill.Activate (board);
  }

  private void SkillStop (BaseSkill skill)
  {
    board.OnClick += Selector;
    board.OnClick -= skill.ClickHandler;
    skill.OnFinish -= SkillStop;
    ui.OnSkill += SkillStart;
  }

  private void Selector (Token tkn)
  {
    if (lastSelected != null) {
      lastSelected.Unselect ();
    }
    if (tkn != null) {
      Token sol = board ["Solid"] [tkn.pos];
      if (sol != null) {
        ui.Render (sol.All<BaseSkill> ());
      } else ui.Render(null);
      lastSelected = board ["Terrain"] [tkn.pos].Get<Selectable> ();
      if (lastSelected != null) {
        lastSelected.Select (selectMaterial);
      }
    } else
      ui.Render (null);
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
