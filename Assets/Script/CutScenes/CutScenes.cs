﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenes : MonoBehaviour
{
    public PlayerStat playerStat;
    public Gamemanager gamemanager;
    public Fademanager fademanager;
    public Playermanager playermanager;
    public MoveOther moveOther;
    public Mapchange mapchange;
    private Animator animator;

    public GameObject Player;
    public GameObject colliderObject;
    private Collider2D objectCollider;

    private void Awake()
    {
        animator = playerStat.GetComponent<Animator>();
        objectCollider = colliderObject.GetComponent<Collider2D>();
    }

    public void StartCut(float speed)
    {
        StartCoroutine(GameStartCut(speed));
    }

    IEnumerator GameStartCut(float Speed)
    {
        fademanager.UIFadeIn(Speed);
        animator.SetBool("testDDR", true);
        yield return new WaitUntil(() => fademanager.color.a < 0.6f);
        // 대사 시작
        gamemanager.CutSceneTalk(700);
        yield return new WaitUntil(() => !playermanager.isaction);
        StartCoroutine(playerStat.AddHp(4, 0.15f));
        yield return new WaitUntil(() => playerStat.PlayerDie);
        animator.SetBool("testDDR", false);
        gamemanager.CutSceneTalk(800);
    }


    void OffCollider()
    {
        objectCollider.enabled = false;
    }

    public IEnumerator Sleep(int walkcount) // 잠자는 컷씬 코루틴
    {
        moveOther = ReturnMoveOther();
        int moveCount = walkcount; // walkcount가 while문에서 --되면서 나누는 값이 작아져서 다른 변수 생성
        OffCollider();
        Vector3 BedVec = Return_Move_Position(Player, colliderObject);
        BedVec.y += 0.15f;
        yield return new WaitForSeconds(0.5f);
        if (BedVec.y > 0.25f) Y_Move_Animation(BedVec);
        while (moveCount > 0)
        {
            Player.transform.Translate(0, BedVec.y / walkcount, 0);
            yield return new WaitForSeconds(0.03f);
            moveCount--;
        }
        moveOther.PlayerMove();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(AwakeHome());
    }

    void Y_Move_Animation(Vector3 DirY) // 이동할 위치에 따른 애니메이션 
    {
        animator.SetBool("Walking", true);
        animator.SetFloat("DirX", 0);
        if (DirY.y > 0)
            animator.SetFloat("DirY", 1);
        else
            animator.SetFloat("DirY", -1);
    }

    Vector3 Return_Move_Position(GameObject moveObject, GameObject ReachObject)
    {
        Vector3 MoveVec = new Vector3(moveObject.transform.position.x, ReachObject.transform.position.y - moveObject.transform.position.y, moveObject.transform.position.z);
        return MoveVec;
    }


    IEnumerator AwakeHome() // 침대에서 나오는 이동 및 대사 나오는 기능 추가해야 됨
    {
        StartCoroutine(mapchange.FadeIn_and_Out());
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => mapchange.fademanager.color.a < 0.05);
        yield return new WaitForSeconds(1f);
        moveOther.Move("LEFT");
    }

    MoveOther ReturnMoveOther()
    {
        MoveOther moveOther = null;
        if (playermanager.eventObject != null)
            moveOther = playermanager.eventObject;
        return moveOther;
    }

    // 대사 넘어갈 때마다 카메라 이동시키는 코드
    //for (int i = 0; i < 2; i++)
    //{
    //    int index = gamemanager.talkindex;
    //    cameramanager.CameraMove(new Vector3(0, 4.5f, 0), 0.1f, 15);
    //    yield return new WaitUntil(() => index != gamemanager.talkindex);
    //}
}
