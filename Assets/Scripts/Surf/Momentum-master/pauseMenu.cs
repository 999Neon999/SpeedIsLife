// Decompiled with JetBrains decompiler
// Type: pauseMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A6E90D9-4890-408D-BB9D-C003E1B300F1
// Assembly location: C:\Users\alecb\code\Momentum\momentum_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
  public Transform canvas;
  public Transform settings;
  public Transform controls;
  private loadOnClick levelManager;
  private mouseLook camera;
  public Slider sliderval;

  private void Start()
  {
    this.canvas.gameObject.SetActive(false);
    this.settings.gameObject.SetActive(false);
    this.controls.gameObject.SetActive(false);
    this.levelManager = GameObject.Find("levelManager").GetComponent<loadOnClick>();
    this.camera = GameObject.Find("Main Camera").GetComponent<mouseLook>();
  }

  private void Update()
  {
    if (!Input.GetKeyDown("escape"))
      return;
    this.pauseGame();
  }

  public void gotoMainMenu()
  {
    Time.timeScale = 1f;
    this.levelManager.LoadScene("mainmenu");
  }

  public void pauseGame()
  {
    if (!this.canvas.gameObject.activeInHierarchy)
    {
      this.camera.enabled = false;
      Cursor.visible = true;
      this.settings.gameObject.SetActive(false);
      this.controls.gameObject.SetActive(false);
      this.canvas.gameObject.SetActive(true);
      Time.timeScale = 0.0f;
    }
    else
    {
      this.camera.enabled = true;
      Cursor.visible = false;
      this.canvas.gameObject.SetActive(false);
      this.settings.gameObject.SetActive(false);
      this.controls.gameObject.SetActive(false);
      Time.timeScale = 1f;
    }
  }

  public void quitGame() => Application.Quit();

  public void setSens() => this.camera.setSens(this.sliderval.value);

  public void gotoSettings()
  {
    this.settings.gameObject.SetActive(true);
    this.canvas.gameObject.SetActive(false);
  }

  public void gotoPause()
  {
    this.settings.gameObject.SetActive(false);
    this.controls.gameObject.SetActive(false);
    this.canvas.gameObject.SetActive(true);
  }

  public void gotoControls()
  {
    this.controls.gameObject.SetActive(true);
    this.canvas.gameObject.SetActive(false);
  }
}
