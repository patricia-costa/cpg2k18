﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foe : MonoBehaviour {
	public Character playerStatus;

	public int maxLife;
	public int life;
	public int MoveSpeed;

	public bool isAttacking;
	public bool isReceivingDamage;

	public int speed;
	public int groundIndex = 0;
	public int enemyType;

	private Rigidbody2D RB2d;
	private BoxCollider2D BC2d;
	private SpriteRenderer ownRenderer;
	public float verticalUpdateDistance = 0.5f;

	private float attackDistance = 1.5f;
	public float time;
	public Text timer;

	// Use this for initialization
	void Start() {
		this.RB2d = this.GetComponent<Rigidbody2D>();
		this.BC2d = this.GetComponent<BoxCollider2D>();
		this.ownRenderer = this.GetComponent<SpriteRenderer>();
		this.playerStatus = GameObject.FindWithTag ("Player").GetComponent<Character>();
		this.life = maxLife;
	}

	// Update is called once per frame
	void Update() {
		if (this.life > 0) {
			ChasePlayer ();
		} else {
			this.Die ();
		}
	}

	IEnumerator Delay ()
	{
		isAttacking = true;
		yield return new WaitForSeconds(.5f);
		isAttacking = false;
	}

	void ChasePlayer () {
		double newDistance = Mathf.Abs (this.transform.position.x - playerStatus.transform.position.x);
		double verticalDist = Mathf.Abs (this.transform.position.y - playerStatus.transform.position.y);

		if (!isAttacking) {
			// Check if it's in distance to the player
			if (newDistance <= attackDistance && verticalDist <= 0.2) {
				Attack ();
				StartCoroutine ("Delay");
			} else if (newDistance <= attackDistance && verticalDist >= 0.2) {
				if (verticalDist >= 0.8) {
					if (this.transform.position.y > playerStatus.transform.position.y) {
						this.MoveDown();
					} else {
						this.MoveUp();
					}
				}
			}
			else {
				int op = (int) Random.Range (0, 100);
				op %= 2;
				if (op == 0) {
					if (verticalDist >= 0.2) {
						if (this.transform.position.y > playerStatus.transform.position.y) {
							this.MoveDown();
						} else {
							this.MoveUp();
						}
					}
				} else { 
					if (this.transform.position.x > playerStatus.transform.position.x) {
						this.Mirror(6);
						this.MoveLeft();
					} else { 
						this.Mirror(4);
						this.MoveRight();
					}
				}
			}
		}

	}

	public void Mirror(int dir) {
		if (dir == 6) this.transform.rotation = new Quaternion(0, 0, 0, 0);
		if (dir == 4) this.transform.rotation = new Quaternion(0, 180, 0, 0);
	}

	public void MoveLeft() {
		this.Mirror(4);
		this.Move();
	}

	public void MoveRight() {
		this.Mirror(6);
		this.Move();
	}

	public void MoveDown() {
		this.transform.Translate (new Vector2 (0, -0.6f) * Time.deltaTime * this.speed);
	}

	public void MoveUp() {
		this.transform.Translate (new Vector2 (0, 0.6f) * Time.deltaTime * this.speed);
	}
		

	public void Move() {
		// this.animator.SetBool("walking", true);
		this.transform.Translate(new Vector2(1, 0) * Time.deltaTime * this.speed);
	}

	public void StopWalking() {
		// this.animator.SetBool("walking", false);
	}
		
	public void Attack() {
		Debug.Log ("kapow!");
		playerStatus.takeDamage (1);
	}

	public void Damage(int damage) {
		Debug.Log ("Argh!");
		if (!isReceivingDamage && this.life > 0) {
			this.life -= damage;
			isReceivingDamage = true;
			StartCoroutine("Flash");
		}
		if (this.life <= 0)
			this.Die ();
	}

	private void Die () {
		Destroy(this.gameObject);
	}

	IEnumerator Flash ()
	{
		bool toggle = true;
		this.BC2d.enabled = false;
		for (int i = 0; i < 10; ++i) {
			yield return new WaitForSeconds(.1f);
			if (toggle) {
				this.ownRenderer.enabled = false;
				toggle = false;
			} else {
				this.ownRenderer.enabled = true;
				toggle = true;
			}
		}
		this.BC2d.enabled = true;
		isReceivingDamage = false;
	}
}

