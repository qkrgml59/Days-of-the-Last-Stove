using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Expedition", menuName = "Game/Expedition")]

public class ExpeditionSO : ScriptableObject
{
    [Header("탐방 기본 정보")]
    public string expeditionName = "탐방";
    [TextArea(2,3)]
    public string description = "근처 숲을 탐방 하여 자원을 찾습니다.";

    [Header("난이도")]
    [Range(1, 5)]
    public int difficulty = 2;       //1: 쉬움 5: 매우 어려움

    [Header("성공 시 보상")]
    public int sucessFoodReward = 3;
    public int successFuelReward = 2;
    public int successMedicineReward = 1;
    public int successVaccineReward = 1;

    [Header("실패 시 패널티")]
    public int failHealthPenalty = -20;
    public int failHungerPenalty = -10;
    public int failTempPenalty = -2;
    public int failInfectionPenalty = 5; //좀비 감염률 증가

    [Header("기본 성공률")]
    [Range(10, 90)]
    public int baseSuccessRate = 60;                   //기본 성공률 설정

}

