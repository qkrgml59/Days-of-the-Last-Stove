using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "GroupMember", menuName = "Game/GroupMember")]
public class GroupMemberSO : ScriptableObject
{

    [Header("기본 정보")]
    public string memberName = "구성원";
    public Sprite protrait;
    public Gender gender = Gender.Male;
    public AgeGroup ageGroup = AgeGroup.Audlt;

    [Header("기본 스텟")]
    public int maxHealth = 100;
    public int maxHunger = 100;
    public int normalBodyTemp = 37;
    public int infection = 0;       //좀비 감염률

    [Header("특성")]
    [Range(0.5f, 2.0f)]
    public float foodEfficiency = 1.0f;   //음식 효율
    [Range(0.5f, 2.0f)]
    public float coldResistance = 1.0f;     //추위저항력
    [Range(0.5f, 1.5f)]
    public float recoveryRate = 1.0f;          //회복력
    [Range(0.3f, 1.5f)]
    public float zombieResistance = 0.5f; //좀비 저항력

    [Header("설명")]
    [TextArea(2, 3)]
    public string description = "그룹 구성원 입니다.";


    public enum Gender
    {
        Male,
        Female
    }

    public enum AgeGroup
    {
        Audlt,
        Child,
        Elder
    }

}
