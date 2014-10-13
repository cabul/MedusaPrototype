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
    terrain.Init (position => (GameObject)Instantiate (cell));
    foreach (Token token in terrain) {
      token.transform.name = cell.name + " @ " + token.position;
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

  private void ShowLabels(Token token)
  {
    BaseInfo[] infos = token.All<BaseInfo>();
    for(int i = 0; i < infos.Length; i++) {
      GUI.Label(new Rect(left,top+(labelHeight+top)*i,labelWidth,labelHeight), infos[i].Content);
    }
  }

  // returns true if skill was selected
  private bool ShowSkills(Token token)
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
    SelectAt(selectedToken.position);

  }

  private void SkillConfirm (BaseSkill skill)
  {
    Debug.Log("Confirm skill "+skill.power+ " @ "+Time.time);
    skill.Apply();

    HandleEvents(skill);

    UnselectAll();
    SelectAt(selectedToken.position);

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
    foreach(Token token in board["Terrain"]) {
      token.Get<Selectable>().Unselect();
    }
  }

  // Seleccionar una celda
  private void Selector (Token token)
  {
    UnselectAll();
    if (token != null) {
      selectedToken = board["Solid"][token.position];
      SelectAt(token.position);
    }
  }

  private void UnselectAt(Position position)
  {
    board["Terrain"][position].Get<Selectable>().Unselect();
  }

  private void SelectAt(Position position)
  {
    board["Terrain"][position].Get<Selectable>().Select (selectMaterial);
  }

  private void DebugClick (Token token)
  {
    if (token == null) {
      Debug.Log ("Hit Nothing");
    } else {
      Token[] tokens = board [token.position];
      string[] arr = new string[tokens.Length];
      for (int i = 0; i < tokens.Length; i++) {
        arr [i] = (tokens [i] == null) ? "ø" : tokens [i].gameObject.name;
      }
      Debug.Log ("Hit @ " + token.position + ": [ " + string.Join (", ", arr) + " ]");
    }
  }

}
