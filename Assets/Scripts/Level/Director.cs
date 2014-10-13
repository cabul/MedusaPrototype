using UnityEngine;
using System.Collections;
using System;

public class Director : MonoBehaviour
{
  private Board board;
  public Token selectedToken;
  public Material selectMaterial;
  public int seed;


  // GUI Stuff
  public int left = 10;
  public int top = 10;
  public int buttonWidth = 50;
  public int buttonHeight = 50;
  public int labelWidth = 200;
  public int labelHeight = 20;

  private BaseSkill selectedSkill;

  void Awake ()
  {
    UnityEngine.Random.seed = seed;
    board = GameObject.Find ("BoardNode").GetComponent<Board> ();
  }

  void Start ()
  {
    CreateLayers();

    CreateTerrain();

    PopulateBoard();

    // Conectar los eventos

    board.OnClick += Selector;
    board.OnClick += DebugClick;

  }

  private void CreateLayers() 
  {
    board += "Effect";
    board += "Solid";
    board += "Terrain";
  }

  private void CreateTerrain()
  {
    GameObject cell = Resources.Load<GameObject> ("Tokens/Terrain/Cell");
    Layer terrain = board ["Terrain"];
    terrain.Init (pos => (GameObject)Instantiate (cell));
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }
  }

  private void PopulateBoard()
  {
    Layer solid = board ["Solid"];
    EnvGenerator env = board.GetComponent<EnvGenerator> ();
    env.Generate (solid);
  }

  // La Interfaz
  void OnGUI()
  {
    if(selectedToken == null) return;
    board.launchClick = false;

    ShowLabels(selectedToken);

    if( ShowSkills(selectedToken) ) return;

    board.launchClick = true;
  }

  private void ShowLabels(Token tkn)
  {
    BaseInfo[] infos = tkn.All<BaseInfo>();
    for(int i = 0; i < infos.Length; i++) {
      GUI.Label(new Rect(left,top+(labelHeight+top)*i,labelWidth,labelHeight), infos[i].info);
    }
  }

  private bool ShowSkills(Token tkn)
  {
    BaseSkill[] skills = selectedToken.All<BaseSkill>();
    if(skills != null) {
      for(int i = 0; i < skills.Length; i++) {
        if(GUI.Button(new Rect(left+((buttonWidth+left)*i),Screen.height - top - buttonHeight,buttonWidth,buttonHeight),skills[i].power)) {
          SelectSkill(skills[i]);
          Event.current.Use();
          return true;
        }
      }
    }
    return false;
  }

  // Seleccionar una habilidad
  private void SelectSkill(BaseSkill skill)
  {
    if(selectedSkill == null) {
      SkillStart(skill);
      return;
    } 
    if( selectedSkill == skill ) {
      SkillConfirm(skill);
      return;
    }
    SkillStop(skill);
    SkillStart(skill);
  }

  // Esto hay que retocarlo un poco
  // A lo mejor crear una clase que se ocupa de ello
  private void SkillStart (BaseSkill skill)
  {
    Debug.Log("Start skill "+skill.power+ " @ "+Time.time);
    skill.Activate (board);
    PropagateEvents(skill);
  }

  private void SkillStop (BaseSkill skill)
  {
    Debug.Log("Stop skill "+skill.power+ " @ "+Time.time);
    skill.Cancel();

    HandleEvents(skill);

    UnselectAll();
    SelectAt(selectedToken.pos);

  }

  private void SkillConfirm (BaseSkill skill)
  {
    Debug.Log("Confirm skill "+skill.power+ " @ "+Time.time);
    skill.Apply();

    HandleEvents(skill);

    UnselectAll();
    SelectAt(selectedToken.pos);

  }

  private void PropagateEvents(BaseSkill skill)
  {
    board.OnClick -= Selector;
    board.OnClick += skill.ClickHandler;
    skill.OnCancel += SkillStop;
    selectedSkill = skill;
  }

  private void HandleEvents(BaseSkill skill)
  {
    board.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    board.OnClick += Selector;
    selectedSkill = null;
  }

  private void UnselectAll()
  {
    foreach(Token tkn in board["Terrain"]) {
      tkn.Get<Selectable>().Unselect();
    }
  }

  // Seleccionar una celda
  private void Selector (Token tkn)
  {
    UnselectAll();
    if (tkn != null) {
      selectedToken = board["Solid"][tkn.pos];
      SelectAt(tkn.pos);
    }
  }

  private void UnselectAt(Position pos)
  {
    board["Terrain"][pos].Get<Selectable>().Unselect();
  }

  private void SelectAt(Position pos)
  {
    board["Terrain"][pos].Get<Selectable>().Select (selectMaterial);
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
