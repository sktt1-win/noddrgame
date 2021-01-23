﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiecTalk : MonoBehaviour
{    
    public CutScenes cutScenes;
    public Gamemanager gamemanager;
    public Playermanager playermanager;

    public GameObject choiceObject;
    public GameObject[] choicePanel;
    public GameObject[] choiceCursor;


    public bool keyInput;
    private int count; // 배열의 크기
    private int result; // 선택한 선택창.
    int[] questionId;

    public void Question(int[] questionNumber) // 질문창 띄움
    {
        result = 0;
        count = choicePanel.Length - 1;
        choiceObject.SetActive(true);
        choiceCursor[0].SetActive(true);
        keyInput = true;
        questionId = questionNumber;
    }

    private void Update() 
    {
        if (keyInput) // 질문창이 뛰어지면 버튼 입력을 받음에 따라 다른 코드가 실행됨
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) // 엔터를 눌러서 질문이 끝날 때
            {
                keyInput = false;
                StartCoroutine(QuestionTalk(result, questionId));
                ExitChoice();
            }
        }
    }

    public void Selection() // 현재 선택되어있는 커서 빼고 다 지우는 함수
    {
        for(int i = 0; i < choicePanel.Length; i++)
        {
            choiceCursor[i].SetActive(false);
        }
        choiceCursor[result].SetActive(true);
    }

    void ExitChoice() // 질문 끝난 후 오브젝트 및 변수 정리
    {
        count = -1;
        choiceObject.SetActive(false);
        choiceCursor[0].SetActive(false);
        choiceCursor[1].SetActive(false);
        keyInput = false;
    }

    IEnumerator QuestionTalk(int result, int[] cutNumber) // 질문 선택에 따른 액션
    {
        gamemanager.talkindex = 0; // 새로운 대화를 시작해야 돼서
        int Action = 0;
        for (int i = 0; i < result + 1; i++)
        {
            if (result == i) // 어차피 제일높은때가 선택한 대답임
                gamemanager.CutSceneTalk(cutNumber[i]);
            Action = cutNumber[i];
        }
        yield return new WaitUntil(() => !playermanager.isaction);
        if (Action == 300)
        {
            StartCoroutine(cutScenes.Sleep(6));
        }
    }
}