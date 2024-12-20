// Decompiled with JetBrains decompiler
// Type: endlessControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A6E90D9-4890-408D-BB9D-C003E1B300F1
// Assembly location: C:\Users\alecb\code\Momentum\momentum_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class endlessControl : MonoBehaviour
{
  public GameObject sl;
  public GameObject ss;
  public GameObject tfl;
  public GameObject tfs;
  private List<GameObject> ramps = new List<GameObject>(4);
  private List<GameObject> addedRamps = new List<GameObject>();
  private float tick;
  private float courseAngle = 180f;
  private float nextCourseAngle = 180f;
  private float lastx = -27.2f;
  private float lastz = 200f;
  private float courseHeight = -28.4f;
  private float distanceFromLast = 100f;
  private static float maxDeltaAngle = 30f;
  private static float spawnRate = 4f;

  private void Start()
  {
    this.ramps.Add(this.sl);
    this.ramps.Add(this.ss);
    this.ramps.Add(this.tfl);
    this.ramps.Add(this.tfs);
    this.tick = Time.fixedTime;
    UnityEngine.Object.Instantiate<GameObject>(this.sl, new Vector3(-27.2f, -28.4f, 200f), Quaternion.identity * Quaternion.AngleAxis(this.courseAngle, Vector3.up) * Quaternion.AngleAxis(45f, Vector3.forward));
  }

  private void Reset()
  {
  }

  private void PlaceRamp()
  {
    this.nextCourseAngle = this.courseAngle + UnityEngine.Random.value * 2f * endlessControl.maxDeltaAngle - endlessControl.maxDeltaAngle;
    Quaternion rotation = Quaternion.identity * Quaternion.AngleAxis(this.nextCourseAngle, Vector3.up) * Quaternion.AngleAxis(45f, Vector3.forward);
    float num = this.courseAngle - 90f;
    float x = this.lastx + Mathf.Cos((float) Math.PI / 180f * num) * this.distanceFromLast;
    float courseHeight = this.courseHeight;
    float z = this.lastz + Mathf.Sin((float) Math.PI / 180f * num) * this.distanceFromLast;
    this.lastx = x;
    this.lastz = z;
    this.addedRamps.Add(UnityEngine.Object.Instantiate<GameObject>(this.ramps[UnityEngine.Random.Range(0, 3)], new Vector3(x, courseHeight, z), rotation));
    this.distanceFromLast += (float) (50.0 + 0.800000011920929 * (double) Time.fixedTime);
    this.courseAngle = this.nextCourseAngle;
  }

  private void Update()
  {
    if ((double) Time.fixedTime - (double) this.tick <= (double) endlessControl.spawnRate)
      return;
    this.tick = Time.fixedTime;
    this.PlaceRamp();
  }

  public void clearStage()
  {
    this.Reset();
    for (int index = 0; index < this.addedRamps.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.addedRamps[index]);
  }
}
