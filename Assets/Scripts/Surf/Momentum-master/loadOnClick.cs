// Decompiled with JetBrains decompiler
// Type: loadOnClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A6E90D9-4890-408D-BB9D-C003E1B300F1
// Assembly location: C:\Users\alecb\code\Momentum\momentum_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class loadOnClick : MonoBehaviour
{
  public string[] levels = new string[5]
  {
    "level1",
    "level2",
    "level3",
    "level4",
    "bonus"
  };
  public int currentLevel;

  public void chooseLevel(string level)
  {
    this.currentLevel = !(level == "level1") ? (!(level == "level2") ? (!(level == "level3") ? (!(level == "level4") ? 4 : 3) : 2) : 1) : 0;
    Cursor.visible = false;
    this.LoadScene(this.levels[this.currentLevel]);
  }

  public void nextLevel()
  {
    ++this.currentLevel;
    Debug.Log((object) this.currentLevel);
    Debug.Log((object) this.levels[this.currentLevel]);
    this.LoadScene(this.levels[this.currentLevel]);
  }

  public void LoadScene(string newLevel) => SceneManager.LoadScene(newLevel);
}
