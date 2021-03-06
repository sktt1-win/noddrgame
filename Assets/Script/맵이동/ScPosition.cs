﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScPosition : MonoBehaviour // 씬 이동시 플레이어가 시작할 위치가 가지는 script
{
    public string mapStartPoint; // 핸재 씬 이름

    public Playermanager playermanager;
    
    private void Start()
    {
        StartCoroutine(Position());
    }
    IEnumerator Position()
    {
        yield return new WaitForSeconds(0.05f);
        if (mapStartPoint == playermanager.currentScene) //씬이 바뀔때마다 mapchangepoint의 mapstartpoint와 currentMapname가 같을시 
            playermanager.transform.position = this.transform.position;
    }
}
