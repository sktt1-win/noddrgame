﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviour 대신에 상속받고 싶은 script의 이름을 쓰면 상속이 됨
public class Playermanager : 변수저장소 //변수저장소 script를 상속받고 있음
{
    public Button button;

    Vector2 RayVector;
    GameObject TalkObject;
    private Animator animator;

    private int Xani;
    private int Yani;
    private float RayX;
    private float RayY;

    public bool isaction;
    bool Run;

    public string currentmapname; //Scenechange script에 있는 mapname변수를 저장

    private void Awake() // Awake() : Start()함수와 다르게 script가 비활성화 상태여도 실해됨 즉 Start()함수는 비활성화 상태일시 script가 활성화되어야 실행됨
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        Animation();
        RayDirection();
        대화();
    }

    void GetInput()
    {
        Run = Input.GetButton("Run");
    }

    void Animation()
    {
        if (isaction)
        {
            MainVector.x = 0;
            MainVector.y = 0;
            animator.SetBool("Walking", false);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // 애니메이션
            MainVector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Vector에 따라 각각 -1,1을리턴
            if (Input.GetAxisRaw("Vertical") != 0)
                Yani++;
            if (Input.GetAxisRaw("Horizontal") != 0)
                Xani++;

            // Xani를 오랫동안 눌러서 값을 올라가면 수직이동중에 방향전환이 되지않는 버그때문에 XAni, YAni값이 축적되지 않게 하기위한 코드
            if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0)
            {
                // X축과 Y축을 동시에 이동 시 전에 이동하던 방향의 ani변수가 더 높도록 조정하여 방향전환이 일어나게 함
                if (Xani > Yani)
                {
                    Xani = 5;
                    Yani = 0;
                }
                else if (Yani > Xani)
                {
                    Yani = 5;
                    Xani = 0;
                }

                if (Xani > Yani && Input.GetAxisRaw("Vertical") != 0) // X축으로 움직이고 있다가 Y축 버튼을 누르면 방향.x값은 0 즉 수평이동중에 수직으로 방향전환 
                    MainVector.x = 0;
                else if (Xani < Yani && Input.GetAxisRaw("Horizontal") != 0)
                    MainVector.y = 0;
            }

            RayX = MainVector.x; // Ray가 안움직일때는 0이되서 여기서 Ray값을 받음
            RayY = MainVector.y;
            animator.SetFloat("DirX", MainVector.x); //DirX에 MainVector.x의 값을 받겠다.
            animator.SetFloat("DirY", MainVector.y);
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
            Xani = 0;
            Yani = 0;
            MainVector.x = 0;
            MainVector.y = 0;
        }
    }

    void RayDirection() // Ray방향
    {
        if (RayY == 1f)
            RayVector = Vector2.up;
        if (RayY == -1f)
            RayVector = Vector2.down;
        if (RayX == 1f)
            RayVector = Vector2.right;
        if (RayX == -1f)
            RayVector = Vector2.left;
    }

    void 대화()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (TalkObject != null)
                gamemanager.오브젝트정보확인(TalkObject);
            if (isaction && button.cuthome)
                gamemanager.컷씬대화(button.cutnumber, false);
        }
    }

    private void FixedUpdate()
    {
        // 이동
        MainVector = new Vector2(MainVector.x, MainVector.y); // 애니메이션 작업 때 x, y갑이 같이 나올 수 없도록 조정해서 대각선 이동이 차단됨 
        if(Run)
            Rigidbody.velocity = MainVector * (speed + 5); // velocity(속도) : 리지드바디의 속도 벡터로 Rigidbody 위치의 변화율을 나타냄.
        else
            Rigidbody.velocity = MainVector * speed;

        // ray 생성
        Debug.DrawRay(Rigidbody.position, RayVector * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(Rigidbody.position, RayVector, 0.7f, LayerMask.GetMask("Object"));

        // GameObject 변수는 null이 되면 인스펙터에서 None표시 안뜨고 그냥 전에 가져온 오브젝트가 빈 껍데기처럼 남아있는듯 함.
        if (rayhit.collider != null && !isaction)// 대화중이 아닐때만 rayhit에 걸린 오브젝트 가져오기(NPC와 대화중에 다른 오브젝트를 가져오는 것을 방지하기 위함) 
            TalkObject = rayhit.collider.gameObject;
        else if(!isaction)
            TalkObject = null;
    }

    private void OnTriggerEnter2D(Collider2D collision) // 트리거와 충돌시 충돌 오브젝트 정보 가져오기
    {
        if (collision.gameObject.name != null && collision.gameObject.tag == "NpcTalk")
        {
            TalkObject = collision.gameObject;
            gamemanager.오브젝트정보확인(TalkObject);
        }
        else
            TalkObject = null;
    }
}


