using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("�׷� ������ ���ø�")]
    public GroupMemberSO[] groupMembers;

    [Header("���� UI")]
    public Text dayText;
    public Text[] memberStatusText;  // ��� ���� ǥ��
    public Button nextDayButton; //���� �� ��ư



    private int[] memberHealth;
    private int[] memberHunger;
    private int[] memberBodyTemp;
    private int[] memberzombieinfection; //���� ������

    int currentDay;                   //���� ��¥

    // Start is called before the first frame update
    void Start()
    {
        currentDay = 1;

        InitializeGroup();
        UpdateUI();

        nextDayButton.onClick.AddListener(NextDay);
    }

    public void NextDay()
    {
        currentDay += 1;
        processDailyChange();
        UpdateUI();
    }

    void InitializeGroup()                     //�׷� ����� ���� ��ŭ �ο� �� �Ҵ�
    {
        int memberCount = groupMembers.Length;
        memberHealth = new int[memberCount];
        memberHunger = new int[memberCount];
        memberBodyTemp = new int[memberCount];
        memberzombieinfection = new int[memberCount];

        for (int i = 0; i < memberCount; i++)
        {
            memberHealth[i] = groupMembers[i].maxHealth;
            memberHunger[i] = groupMembers[i].maxHunger;
            memberBodyTemp[i] = groupMembers[i].normalBodyTemp;
            memberzombieinfection[i] = groupMembers[i].infection;
        }
    }

    void processDailyChange()
    {
        int baseHungerLoss = 15;                     //����� 15 ����
        int baseTempLoss = 1;
       // int basezombieLoss = 2;                     //������ 2���

        for (int i = 0; i < groupMembers.Length; i++)
        {


            if (groupMembers[i] == null) continue;

            GroupMemberSO member = groupMembers[i];

            float hungerMltiplier = member.ageGroup == GroupMemberSO.AgeGroup.Child ? 0.8f : 1.0f;

            memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMltiplier);          //����� ����� ����
            memberBodyTemp[i] -= Mathf.RoundToInt(baseTempLoss * member.coldResistance);      //����� ���� ���׷� 
           // memberzombieinfection[i] += Mathf.RoundToInt(basezombieLoss % member.infection);

            if (memberHunger[i] <= 0) memberHunger[i] -= 15;     //���ָ�
            if (memberHealth[i] <= 30) memberHealth[i] -= 20;        //�ɰ��� ��ü����
            if (memberBodyTemp[i] <= 32) memberBodyTemp[i] -= 10;       //��ü����



            memberHunger[i] = Mathf.Max(0, memberHunger[i]);
            memberBodyTemp[i] = Mathf.Max(25, memberBodyTemp[i]);
            memberHealth[i] = Mathf.Max(0, memberHealth[i]);
          //  memberzombieinfection[i] = Mathf.Clamp(memberzombieinfection[i], 0, 100);  // ���� ��ġ�� 100 ���� �ʵ���

        }


    }





    
           

    void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] !=null && memberStatusText[i] != null)
            {
                GroupMemberSO member = groupMembers[i];


                memberStatusText[i].text =
                                           $"{member.memberName} \n" +
                                           $"ü��   : {memberHealth[i]} \n" +
                                           $"����� : {memberHunger[i]} \n" +
                                           $"ü��   : {memberBodyTemp[i]}��\n";
                                        //   $"������ : {memberzombieinfection[i]}";

            }
            
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
