﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutScenes : MoveOther
{
    Gamemanager gamemanager;
    Playermanager playermanager;
    Fademanager fademanager;
    public Image backgroundImage;

    public GameObject inputField;
    public Text inputAnswer;
    string answer = "보자기";

    private void Awake()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
        playermanager = FindObjectOfType<Playermanager>();
        animator = playermanager.GetComponent<Animator>();
        fademanager = FindObjectOfType<Fademanager>(); // mapchage는 여러개라서 나중에 오류의 원인일수도
        DirSave = new Queue<string>();
    }

    public IEnumerator GameStartCut(float Speed)
    {
        fademanager.UIFadeIn(Speed);
        yield return new WaitUntil(() => fademanager.color.a > 0.99f);
        backgroundImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        gamemanager.CutSceneTalk(100);
        yield return new WaitUntil(() => !playermanager.isaction);
        GameObject blackImage = GameObject.Find("Black Image");
        blackImage.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(gamemanager.QuestionCoroutine(200, new int[] { 400, 300 }));
        yield return new WaitUntil(() => !playermanager.isaction);
        if (gamemanager.choiecTalk.result == 0)
            StartCoroutine(FirstMeet());
        else
            StartCoroutine(GetAnswer());
    }

    IEnumerator FirstMeet()
    {
        //ParticleSystem particle = FindObjectOfType<ParticleSystem>();
        //particle.Play();
        fademanager.color.a = 0;
        fademanager.UIFadeIn(0.002f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => fademanager.color.a > 0.99f);
        SceneManager.LoadScene(1);
    }

    IEnumerator GetAnswer()
    {
        inputField.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        inputField.SetActive(false);
        if (answer == inputAnswer.text)
        {
            Debug.Log("정답");
        }
        else
        {
            gamemanager.CutSceneTalk(500);
            yield return new WaitUntil(() => !playermanager.isaction);
            playermanager.isaction = true;
            SceneManager.LoadScene(1);
        }
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

    CutScenes ReturnMoveOther()
    {
        CutScenes moveOther = null;
        if (playermanager.eventObject != null)
            moveOther = playermanager.eventObject;
        return moveOther;
    }
}
