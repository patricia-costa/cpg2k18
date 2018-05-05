﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public int speed;
    public GameLoop gameLoop;
    public int groundIndex = 0;

    private Rigidbody2D RB2d;
    private Transform characterTransform;
    private BoxCollider2D BC2d;
    private Animator animator;


    public float verticalUpdateDistance = 0.5f;


    public float attackDistance;
    public float maxAttackDistance;


    public GameObject bombPrefab;
    public GameObject superBombPrefab;

    // Use this for initialization
    void Start() {

        this.RB2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.BC2d = this.GetComponent<BoxCollider2D>();

        this.transform.position = new Vector2(this.transform.position.x, gameLoop.groundLayers[this.groundIndex].position.y);

    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {

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
        if (groundIndex > 0) {
            groundIndex--;
            StopCoroutine("VerticalMove");
            StartCoroutine("VerticalMove",2);
        } 
    }

    public void MoveUp() {
        if (groundIndex < (gameLoop.groundLayers.Length - 1)) {
            groundIndex++;
            StopCoroutine("VerticalMove");
            StartCoroutine("VerticalMove",8);
        }
    }

    IEnumerator VerticalMove(int dir) {
        this.animator.SetBool("walking", true);

        float distance = Mathf.Abs(this.transform.position.y - gameLoop.groundLayers[this.groundIndex].position.y);

        for(float i = 0; i < distance; i+= verticalUpdateDistance) {
            if (dir == 2) this.transform.Translate(new Vector2(0, -verticalUpdateDistance)); 
            if (dir == 8) this.transform.Translate(new Vector2(0, verticalUpdateDistance));
            yield return new WaitForSeconds(.001f);
        }
    }

    public void Move() {
        this.animator.SetBool("walking", true);
        this.transform.Translate(new Vector2(1, 0) * Time.deltaTime * this.speed);
    }

    public void StopWalking() {
        this.animator.SetBool("walking", false);
    }

    public void BeginChargeAttack() {
        StartCoroutine("ChargingAttack");
    }

    IEnumerator ChargingAttack() {
        attackDistance = 0;
        while (true) {
            if (attackDistance > maxAttackDistance) {
                attackDistance = 0;
            }
            else {
                attackDistance += 1;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    public void Attack() {
        StopCoroutine("ChargingAttack");
        this.animator.SetTrigger("attack");
        Instantiate(bombPrefab, this.transform.position, Quaternion.identity);
    }

    public void Super() {
        this.animator.SetTrigger("super");
    }
}
