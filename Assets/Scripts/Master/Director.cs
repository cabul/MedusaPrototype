using UnityEngine;
using System.Collections;
using System;

// Master of Disaster

public class Director : MonoBehaviour
{
  private Board board;
  public Token selectedToken;
  public Material selectMaterial;
  public int seed;

  public float nextGUI = 0;
  public float deltaTime = 0.01f;

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
    // Añadir capas
    board += "Effect";
    board += "Solid";
    board += "Terrain";

    GameObject cell = Resources.Load<GameObject> ("Tokens/Terrain/Cell");

    // Instanciar el terreno
    Layer terrain = board ["Terrain"];
    terrain.Init (pos => (GameObject)Instantiate (cell));
    
    foreach (Token tkn in terrain) {
      tkn.transform.name = cell.name + " @ " + tkn.pos;
    }

    // Generar los objetos

    Layer solid = board ["Solid"];

    EnvGenerator env = board.GetComponent<EnvGenerator> ();

    env.Generate (solid);

    // Conectar los eventos

    board.OnClick += Selector;
    board.OnClick += DebugClick;

    // Esto creo que ya no hace falta
    nextGUI = Time.time + deltaTime;

  }

  // La Interfaz
  void OnGUI()
  {
    if(selectedToken == null) return;
    board.launchClick = false;
    BaseInfo[] infos = selectedToken.All<BaseInfo>();
    for(int i = 0; i < infos.Length; i++) {
      GUI.Label(new Rect(left,top+(labelHeight+top)*i,labelWidth,labelHeight), infos[i].info);
    }
    BaseSkill[] skills = selectedToken.All<BaseSkill>();
    if(skills != null) {
      for(int i = 0; i < skills.Length; i++) {
        if(GUI.Button(new Rect(left+((buttonWidth+left)*i),Screen.height - top - buttonHeight,buttonWidth,buttonHeight),skills[i].power)) {
          SelectSkill(skills[i]);
          Event.current.Use();
          return;
        }
      }
    }
    board.launchClick = true;
  }

  // Seleccionar una habilidad
  private void SelectSkill(BaseSkill skill)
  {
    // Bug fix?
    if( Time.time < nextGUI) return;
    nextGUI = Time.time + deltaTime;

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

    board.OnClick -= Selector;

    board.OnClick += skill.ClickHandler;
    skill.OnCancel += SkillStop;

    selectedSkill = skill;
  }

  private void SkillStop (BaseSkill skill)
  {
    Debug.Log("Stop skill "+skill.power+ " @ "+Time.time);
    skill.Cancel();

    board.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    
    board.OnClick += Selector;

    selectedSkill = null;

    UnselectAll();
    SelectAt(selectedToken.pos);

  }

  private void SkillConfirm (BaseSkill skill)
  {
    Debug.Log("Confirm skill "+skill.power+ " @ "+Time.time);
    skill.Apply();

    board.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    
    board.OnClick += Selector;

    selectedSkill = null;

    UnselectAll();
    SelectAt(selectedToken.pos);

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
