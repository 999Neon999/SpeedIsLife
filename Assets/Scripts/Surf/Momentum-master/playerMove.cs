using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
public class PlayerMove : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x000020B8 File Offset: 0x000002B8
	private void Start()
	{
		this.spawnPos = base.transform.position;
		this.spawnRot = base.transform.rotation;
		this.rb = base.GetComponent<Rigidbody>();
		this.ml = GameObject.Find("Main Camera").GetComponent<mouseLook>();
		this.levelManager = GameObject.Find("levelManager").GetComponent<loadOnClick>();
		this.uiSpeed = GameObject.Find("Speed").GetComponent<Text>();
		this.rotationX = this.ml.getRotationX();
		this.rotationY = this.ml.getRotationY();
		this.lastRotationX = this.ml.getRotationX();
		this.lastRotationY = this.ml.getRotationY();
		if (SceneManager.GetActiveScene().name == "boost")
		{
			this.isEndless = true;
			this.onGround = false;
			this.endlessManager = GameObject.Find("endlessManager").GetComponent<endlessControl>();
		}
		this.reset();
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021BC File Offset: 0x000003BC
	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Ramp")
		{
			this.onRamp = true;
			this.rampBoostTimer = Time.time;
			this.rampSpeed = this.rb.velocity.magnitude;
		}
		else if (col.gameObject.tag == "platform")
		{
			this.onRamp = false;
			this.onGround = true;
		}
		else if (col.gameObject.tag == "trigger")
		{
			this.reset();
		}
		else if (col.gameObject.tag == "finish")
		{
			string name = SceneManager.GetActiveScene().name;
			string level = "mainmenu";
			if (name == "level1")
			{
				level = "level2";
			}
			else if (name == "level2")
			{
				level = "level3";
			}
			else if (name == "level3")
			{
				level = "level4";
			}
			else if (name == "level4")
			{
				level = "bonus";
			}
			this.levelManager.chooseLevel(level);
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002306 File Offset: 0x00000506
	private void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Ramp")
		{
			if (this.speed < 2f)
			{
				this.onRamp = false;
			}
			else
			{
				this.onRamp = true;
			}
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002348 File Offset: 0x00000548
	private void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Ramp")
		{
			this.onRamp = false;
		}
		else if (col.gameObject.tag == "platform")
		{
			this.onGround = false;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0000239C File Offset: 0x0000059C
	private void reset()
	{
		this.rb.position = this.spawnPos;
		this.rb.velocity = new Vector3(0f, 0f, 0f);
		base.transform.rotation = this.spawnRot;
		this.endlessMag = 0f;
		if (this.isEndless)
		{
			this.endlessManager.clearStage();
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000240C File Offset: 0x0000060C
	private void Update()
	{
		this.speed = this.rb.velocity.magnitude;
		this.uiSpeed.text = string.Empty + Mathf.RoundToInt(this.speed);
		if (Input.GetKeyDown("r"))
		{
			this.reset();
		}
		if (Input.GetKeyDown("space"))
		{
			this.jumpQueue = true;
		}
		if (Input.GetKeyUp("space"))
		{
			this.jumpQueue = false;
		}
		if (this.jumpQueue && this.onGround)
		{
			this.applyJump();
		}
		this.lastRotationX = this.rotationX;
		this.lastRotationY = this.rotationY;
		this.rotationX = this.ml.getRotationX();
		this.rotationY = this.ml.getRotationY();
		float num = 0f;
		float num2 = 0f;
		if (Input.GetKey("w"))
		{
			num2 += 1f;
		}
		if (Input.GetKey("s"))
		{
			num2 -= 1f;
		}
		if (Input.GetKey("a"))
		{
			num -= 1f;
		}
		if (Input.GetKey("d"))
		{
			num += 1f;
		}
		if (this.onGround)
		{
			this.handleGroundControl(num2, num);
		}
		else if (this.onRamp)
		{
			this.handleRampControl(num2, num);
		}
		else
		{
			this.handleAirControl(num2, num);
		}
		this.lastVelocity = this.rb.velocity;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000025A0 File Offset: 0x000007A0
	private void applyJump()
	{
		float num = this.floorFriction;
		this.floorFriction = 0f;
		Vector3 force = new Vector3(this.rb.velocity.x, this.jumpForce, this.rb.velocity.y);
		this.rb.AddForce(force);
		this.floorFriction = num;
		this.onGround = false;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000260C File Offset: 0x0000080C
	private void handleGroundControl(float forward, float side)
	{
		this.rb.useGravity = true;
		float num = this.rotationX;
		if (forward != 0f || side != 0f)
		{
			if (forward == -1f)
			{
				num -= 180f;
			}
			if (side == -1f)
			{
				if (forward == 1f)
				{
					num += 45f;
				}
				if (forward == -1f)
				{
					num += 45f;
				}
				else
				{
					num -= 90f;
				}
			}
			if (side == 1f)
			{
				if (forward == 1f)
				{
					num -= 45f;
				}
				if (forward == -1f)
				{
					num -= 45f;
				}
				else
				{
					num += 90f;
				}
			}
			this.ApplyRunForce(num);
		}
		if (side == 0f && forward == 0f)
		{
			this.rb.AddForce(-this.rb.velocity * this.floorFriction);
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002714 File Offset: 0x00000914
	private void handleRampControl(float forward, float side)
	{
		this.ApplyRampForce(this.rotationX);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002724 File Offset: 0x00000924
	private void handleAirControl(float forward, float side)
	{
		this.rb.useGravity = true;
		float num = this.rotationX - this.lastRotationX;
		if (side == 1f)
		{
			if (num > 0f)
			{
				this.ApplyAirForce(this.rotationX);
			}
		}
		else if (side == -1f)
		{
			if (num < 0f)
			{
				this.ApplyAirForce(this.rotationX);
			}
		}
		else if (forward == -1f)
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000027D4 File Offset: 0x000009D4
	private void ApplyRunForce(float angle)
	{
		angle = 0.017453292f * angle;
		float x = Mathf.Sin(angle);
		float z = Mathf.Cos(angle);
		Vector3 a = new Vector3(x, 0f, z);
		if (this.rb.velocity.magnitude < this.runSpeed)
		{
			this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.runSpeed);
		}
		this.rb.AddForce(a * this.accel);
		this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.runSpeed);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002880 File Offset: 0x00000A80
	private void ApplyRampForce(float angle)
	{
		float num = Mathf.Clamp(this.rb.velocity.y, -this.clamper, this.clamper);
		float d;
		if (this.rb.velocity.y < 0f)
		{
			d = num * -1f * this.rampFactor * Time.fixedDeltaTime + this.rampSpeed;
		}
		else
		{
			d = num * -0.8f * this.rampFactor * Time.fixedDeltaTime + this.rampSpeed;
		}
		this.rb.velocity = this.rb.velocity.normalized * d;
		this.rampSpeed = d;
		this.ApplyAirForce(angle);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002940 File Offset: 0x00000B40
	private void ApplyAirForce(float angle)
	{
		this.endlessMag += this.endlessBoost * Time.fixedDeltaTime;
		angle = 0.017453292f * angle;
		float x = Mathf.Sin(angle);
		float z = Mathf.Cos(angle);
		Vector3 vector = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		Vector3 vector2 = new Vector3(x, 0f, z);
		if (this.isEndless && this.endlessMag > vector.magnitude)
		{
			vector2 *= this.endlessMag;
		}
		else
		{
			vector2 *= vector.magnitude;
		}
		vector2.y = this.rb.velocity.y;
		this.rb.velocity = vector2;
	}

	// Token: 0x04000001 RID: 1
	private float runSpeed = 5f;

	// Token: 0x04000002 RID: 2
	private float accel = 1.6f;

	// Token: 0x04000003 RID: 3
	private float floorFriction = 0.1f;

	// Token: 0x04000004 RID: 4
	public float jumpForce = 20f;

	// Token: 0x04000005 RID: 5
	public float speed;

	// Token: 0x04000006 RID: 6
	private float rampSpeed;

	// Token: 0x04000007 RID: 7
	private float clamper = 4f;

	// Token: 0x04000008 RID: 8
	private float rampFactor = 0.8f;

	// Token: 0x04000009 RID: 9
	private bool onGround = true;

	// Token: 0x0400000A RID: 10
	private bool jumpQueue;

	// Token: 0x0400000B RID: 11
	private float jumpPressTimer;

	// Token: 0x0400000C RID: 12
	private bool onRamp;

	// Token: 0x0400000D RID: 13
	private float rampBoostTimer;

	// Token: 0x0400000E RID: 14
	private mouseLook ml;

	// Token: 0x0400000F RID: 15
	private float rotationX;

	// Token: 0x04000010 RID: 16
	public float rotationY;

	// Token: 0x04000011 RID: 17
	private float lastRotationX;

	// Token: 0x04000012 RID: 18
	private float lastRotationY;

	// Token: 0x04000013 RID: 19
	private Vector3 lastVelocity;

	// Token: 0x04000014 RID: 20
	private Vector3 spawnPos;

	// Token: 0x04000015 RID: 21
	private Quaternion spawnRot;

	// Token: 0x04000016 RID: 22
	private Rigidbody rb;

	// Token: 0x04000017 RID: 23
	private endlessControl endlessManager;

	// Token: 0x04000018 RID: 24
	private loadOnClick levelManager;

	// Token: 0x04000019 RID: 25
	private Text uiSpeed;

	// Token: 0x0400001A RID: 26
	private bool isEndless;

	// Token: 0x0400001B RID: 27
	public float endlessBoost = 2f;

	// Token: 0x0400001C RID: 28
	private float endlessMag;
}
