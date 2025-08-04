using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "GroupMember", menuName = "Game/GroupMember")]
public class GroupMemberSO : ScriptableObject
{

    [Header("�⺻ ����")]
    public string memberName = "������";
    public Sprite protrait;
    public Gender gender = Gender.Male;
    public AgeGroup ageGroup = AgeGroup.Audlt;

    [Header("�⺻ ����")]
    public int maxHealth = 100;
    public int maxHunger = 100;
    public int normalBodyTemp = 37;
    public int infection = 0;       //���� ������

    [Header("Ư��")]
    [Range(0.5f, 2.0f)]
    public float foodEfficiency = 1.0f;   //���� ȿ��
    [Range(0.5f, 2.0f)]
    public float coldResistance = 1.0f;     //�������׷�
    [Range(0.5f, 1.5f)]
    public float recoveryRate = 1.0f;          //ȸ����
    [Range(0.3f, 1.5f)]
    public float zombieResistance = 0.5f; //���� ���׷�

    [Header("����")]
    [TextArea(2, 3)]
    public string description = "�׷� ������ �Դϴ�.";


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
