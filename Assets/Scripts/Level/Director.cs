using UnityEngine;
using System.Collections;
using System;

public delegate void CellMarker(Position position, Material material);

public class Director : MonoBehaviour
{
  private Board board;
  public Token selectedToken;
  public int seed;

  private GUIControl gui;

  private BaseSkill selectedSkill;

  public Material selectMaterial;

  void Awake ()
  {
    UnityEngine.Random.seed = seed;
    board = GameObject.Find ("BoardNode").GetComponent<Board> ();
    gui = GetComponent<GUIControl>();
  }

  void Start ()
  {
    CreateLayers();

    CreateTerrain();

    PopulateBoard();

    // Conectar los eventos

    gui.OnClick += SelectCell;
    //gui.OnClick += DebugClick;

    gui.OnSkill += SelectSkill;

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
    skill.Activate (board,MarkCell);
    PropagateEvents(skill);
  }

  private void SkillStop (BaseSkill skill)
  {
    skill.Cancel();

    HandleEvents(skill);

    UnselectAll();
    MarkCell(selectedToken.position,selectMaterial);

  }

  private void SkillConfirm (BaseSkill skill)
  {
    skill.Apply();

    HandleEvents(skill);

    UnselectAll();
    MarkCell(selectedToken.position,selectMaterial);
  }

  private void PropagateEvents(BaseSkill skill)
  {
    gui.OnClick -= SelectCell;
    gui.OnClick += skill.ClickHandler;
    skill.OnCancel += SkillStop;
    selectedSkill = skill;
  }

  private void HandleEvents(BaseSkill skill)
  {
    gui.OnClick -= skill.ClickHandler;
    skill.OnCancel -= SkillStop;
    gui.OnClick += SelectCell;
    selectedSkill = null;
  }

  private void UnselectAll()
  {
    foreach(Token token in board["Terrain"]) {
      token.Get<Selectable>().Unselect();
    }
  }

  // Seleccionar una celda
  private void SelectCell (Token token)
  {
    UnselectAll();
    if (token != null) {
      selectedToken = board["Solid"][token.position];
      gui.Render(selectedToken);
      MarkCell(token.position,selectMaterial);
    } else {
      gui.Render(null);
    }
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

  public void MarkCell(Position position, Material material)
  {
    if (material == null) {
      board["Terrain"][position].Get<Selectable>().Unselect();
    } else {
      board["Terrain"][position].Get<Selectable>().Select(material);
    }
  }

}
