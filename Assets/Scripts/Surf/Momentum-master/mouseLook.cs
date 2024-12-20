// Decompiled with JetBrains decompiler
// Type: mouseLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A6E90D9-4890-408D-BB9D-C003E1B300F1
// Assembly location: C:\Users\alecb\code\Momentum\momentum_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class mouseLook : MonoBehaviour
{
  private float sensitivityX = 2f;
  private float sensitivityY = 2f;
  private float minimumY = -90f;
  private float maximumY = 90f;
  private float rotationX;
  private float rotationY;
  private Quaternion originalRotation;

  public float getRotationX() => this.rotationX;

  public float getRotationY() => this.rotationY;

  public void setSens(float newSens)
  {
    this.sensitivityX = newSens;
    this.sensitivityY = newSens;
  }

  private void Update()
  {
    this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
    this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
    this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
    this.transform.localRotation = this.originalRotation * Quaternion.AngleAxis(this.rotationX, Vector3.up) * Quaternion.AngleAxis(this.rotationY, -Vector3.right);
  }

  private void Start()
  {
    Cursor.visible = false;
    this.originalRotation = this.transform.localRotation;
  }
}
