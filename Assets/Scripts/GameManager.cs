using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("그룹 구성원 템플릿")]
    public GroupMemberSO[] groupMembers;

    [Header("참조 UI")]
    public Text dayText;
    public Text[] memberStatusText;  // 멤버 상태 표시
    public Button nextDayButton; //다음 날 버튼



    private int[] memberHealth;
    private int[] memberHunger;
    private int[] memberBodyTemp;
    private int[] memberzombieinfection; //좀비 감염도

    int currentDay;                   //현재 날짜

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

    void InitializeGroup()                     //그룹 멤버의 길이 만큼 인원 수 할당
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
        int baseHungerLoss = 15;                     //배고픔 15 감소
        int baseTempLoss = 1;
       // int basezombieLoss = 2;                     //감염도 2상승

        for (int i = 0; i < groupMembers.Length; i++)
        {


            if (groupMembers[i] == null) continue;

            GroupMemberSO member = groupMembers[i];

            float hungerMltiplier = member.ageGroup == GroupMemberSO.AgeGroup.Child ? 0.8f : 1.0f;

            memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMltiplier);          //멤버별 배고픔 저항
            memberBodyTemp[i] -= Mathf.RoundToInt(baseTempLoss * member.coldResistance);      //멤버별 추위 저항력 
           // memberzombieinfection[i] += Mathf.RoundToInt(basezombieLoss % member.infection);

            if (memberHunger[i] <= 0) memberHunger[i] -= 15;     //굶주림
            if (memberHealth[i] <= 30) memberHealth[i] -= 20;        //심각한 저체온증
            if (memberBodyTemp[i] <= 32) memberBodyTemp[i] -= 10;       //저체온증



            memberHunger[i] = Mathf.Max(0, memberHunger[i]);
            memberBodyTemp[i] = Mathf.Max(25, memberBodyTemp[i]);
            memberHealth[i] = Mathf.Max(0, memberHealth[i]);
          //  memberzombieinfection[i] = Mathf.Clamp(memberzombieinfection[i], 0, 100);  // 감염 수치도 100 넘지 않도록

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
                                           $"체력   : {memberHealth[i]} \n" +
                                           $"배고픔 : {memberHunger[i]} \n" +
                                           $"체온   : {memberBodyTemp[i]}도\n";
                                        //   $"감염도 : {memberzombieinfection[i]}";

            }
            
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
