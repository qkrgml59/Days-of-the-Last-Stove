using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

//�ؾ��ϴ� �� ��ο��� ������ ��� �� �� ��� �� ��ŭ ����

public class GameManager : MonoBehaviour
{
    [Header("�׷� ������ ���ø�")]
    public GroupMemberSO[] groupMembers;

    [Header("������ ���ø�")]
    public ItemSO foodItem;              //���� ������SO
    public ItemSO fuelItem;              //���� ������SO
    public ItemSO medicineItem;         //�Ǿ�ǰ ������SO
    public ItemSO vaccineItem;         //��� ������SO

    [Header("���� UI")]
    public Text dayText;                                //��¥ ǥ�� UI
    public Text[] memberStatusTexts;                    //�ɹ� ���� ǥ�� UI
    public Button nextDayButton;                        //���� �� ��ư
   // public Text inventoryText;                          //�κ��丮 ǥ��


    [Header("������ ��ư")]
    public Button feedButton;         //���� �ֱ�
    public Button heatButton;         //���� �ϱ�
    public Button healButton;          //ġ�� �ϱ�
    public Button vaccineButton;      //��� �ֱ�



    [Header("���� ����")]
    int currentDay;                          //���� ��¥
    public int food = 5;                     //���� ����
    public int fuel = 3;                     //���� ����
    public int medicine = 4;                // �Ǿ�ǰ ����
    public int vaccine = 0;                // ��� ����


    [Header("Ư�� ��� ������ ��� �˾�")]
    public GameObject ItemPopup;     //������ ��� �˾�
    public Text itemPopupTitleText;   //������ ��� �˾� ����
    public Button closePopupButton;   //������ ��� �˾� �ݱ� ��ư
    public Button openPopupButton;       // �˾� ���� ��ư

    [Header("Ư�� �ɹ������� �Ҹ� ��ư")]
    public Button[] individualFoodButtons;
    public Button[] individualHealButtons;
    public Button[] individualVaccinButtons;

    [Header("�̺�Ʈ �ý���")]
    public EventSO[] events;                 //�̺�Ʈ ���
    public GameObject eventPopup;            //�̺�Ʈ �˾� �г�
    public Text eventTitleText;              //�̺�Ʈ ����
    public Text eventDescriptionText;        //�̺�Ʈ ����
    public Button eventConfirmbutton;        //�̺�Ʈ �ݱ�(Ȯ��) ��ư


    [Header("�κ��丮UI")]
    public Button inventoryButton;            //�κ��丮 ��ư
    public GameObject inventoryPanel;         //�κ��丮 �г�
    public Text inventoryText;                //�κ��丮 ǥ�� �ؽ�Ʈ
    public Button closeInventoryButton;       //�κ��丮 �ݱ� ��ư

    [Header("������ ��� ����")]
    //public bool hasUsedFoodToday;
    //public bool hasUsedVaccine;
    //public bool hasUsedFuel;
    //public bool hasUsedMedicne;
    //���� �ڵ��
    public int usedItemCountToday = 0;
    public int dailyItemUseLimit = 2;

    [Header("���� �ý���")]
    public AudioSource audioSource;           //���� �ҽ�




    //��Ÿ�� ������
    public int[] memberHealth;              //ü��
    public int[] memberHunger;             //�����
    public int[] memberBodyTemp;            //ü��
    public int[] memberInfection;         //����
    

    //�Ϸ翡 �� ���� �������� ��� �� �� �ְ� �ϴ� ����
    public bool hasUsedFoodToday = false;         //���� ������ ��� ����
    public bool hasUsedMedicineToday = false;     //�Ǿ�ǰ ������ ��� ����
    public bool hasUsedFuelToday = false;         //���� ������ ��� ����
    public bool hasUsedVaccineToday = false;      //��� ������ ��� ����


    // Start is called before the first frame update
    void Start()
    {

        currentDay = 1;


        InitializeGroup();
        UpdateUI();

        nextDayButton.onClick.AddListener(NextDay);         //���� �� ��ư Ŭ�� �� NextDay �Լ� ȣ��
        feedButton.onClick.AddListener(UseFoodItem);
        healButton.onClick.AddListener(UseMedicItem);
        heatButton.onClick.AddListener(UseFuelItem);
        vaccineButton.onClick.AddListener(Usevaccinetem);


        for (int i = 0; i < individualFoodButtons.Length && i < groupMembers.LongLength; i++)
        {
            int memberindex = i;
            individualFoodButtons[i].onClick.AddListener(() => GiveFoodToMember(memberindex));

        }

        for (int i = 0; i < individualHealButtons.Length && i < groupMembers.LongLength; i++)
        {
            int memberindex = i;
            individualHealButtons[i].onClick.AddListener(() => HealMember(memberindex));
        }

        eventPopup.SetActive(false);
        eventConfirmbutton.onClick.AddListener(CloseEventPopup);

        inventoryPanel.SetActive(false);
        inventoryButton.onClick.AddListener(OpenInvetoryPopup);
        closeInventoryButton.onClick.AddListener(CloseInventoryPopup);


        ItemPopup.SetActive(false);            //������ �˾� ��Ȱ��ȭ
        openPopupButton.onClick.AddListener(OpenItemPopup); 
        closePopupButton.onClick.AddListener(CloseItemPopup); 

    }



    void InitializeGroup()
    {
        int memberCount = groupMembers.Length;                //�׷� �ɹ��� ���� ��ŭ �ο� �� �Ҵ�
        memberHealth = new int[memberCount];                  //�׷� �ɹ� ���� ��ŭ �迭 �Ҵ�
        memberHunger = new int[memberCount];
        memberBodyTemp = new int[memberCount];
        memberInfection = new int[memberCount];

        for (int i = 0; i < memberCount; i++)
        {
            if (groupMembers[i] != null)
            {
                memberHealth[i] = groupMembers[i].maxHealth;
                memberHunger[i] = groupMembers[i].maxHunger;
                memberBodyTemp[i] = groupMembers[i].normalBodyTemp;
                memberInfection[i] = groupMembers[i].infection;

            }
        }
    }

    public void UpdateUI()
    {
        dayText.text = $"Day{currentDay}";

        inventoryText.text = $"����   : {food}��\n" +
                             $"����   : {fuel}��\n" +
                             $"�Ǿ�ǰ : {medicine}��\n" +
                             $"���   : {vaccine}��\n";

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberStatusTexts[i] != null)
            {
                GroupMemberSO member = groupMembers[i];

                string status = GetMemberStatus(i);

                memberStatusTexts[i].text =
                    $"{member.memberName} {status} \n" +
                    $"ü��   : {memberHealth[i]} \n" +
                    $"����� : {memberHunger[i]} \n" +
                    $"ü��   : {memberBodyTemp[i]} ��\n" +
                    $"������ : {memberInfection[i]} %";
            }


            UpdateTextColor(memberStatusTexts[i], memberHealth[i]);
        }

        
        int aliveCount = GetAlivememberCount();

        //��ư Ȱ��/��Ȱ��
        feedButton.interactable = (food >= aliveCount && aliveCount > 0);
        heatButton.interactable = (fuel >= 1 && aliveCount > 0);                           //����� �Ѱ��� �־ ��ư Ȱ��ȭ ��.
        healButton.interactable = (medicine >= aliveCount && aliveCount > 0);
        vaccineButton.interactable = (vaccine >= aliveCount && aliveCount > 0);


        for (int i = 0; i < individualFoodButtons.Length; i++)
        {
            bool canGive = (food > 0 && memberHealth[i] > 0);
            individualFoodButtons[i].interactable = canGive;
        }


        for (int i = 0; i < individualHealButtons.Length; i++)
        {
            bool canGive = (medicine > 0 && memberHealth[i] > 0);
            individualHealButtons[i].interactable = canGive;
        }

        for (int i = 0; i < individualVaccinButtons.Length; i++)
        {
            bool canGive = (vaccine > 0 && memberHealth[i] > 0);
            individualVaccinButtons[i].interactable = canGive;
        }


    }

    void ProcessDilyChange()
    {
        int baseHungerLoss = 15;
        int baseTempLoss = 1;
        int baseInfectionLoss = 5;

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] == null) continue;


            GroupMemberSO member = groupMembers[i];

            //���̿� ���� ����� ����
            float hungerMltiplier = member.ageGroup == GroupMemberSO.AgeGroup.Child ? 0.8f : 1.0f;

            //���� ����
            memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMltiplier);              //�ɹ��� ����� ���� ����
            memberBodyTemp[i] -= Mathf.RoundToInt(baseTempLoss * member.coldResistance);       //����� ���� ���׷�
            memberInfection[i] += Mathf.RoundToInt(baseInfectionLoss * member.zombieResistance); //�ɹ��� ���� ���׷� (���� �ؾ��ؼ� +)


            //�ǰ� üũ
            if (memberHunger[i] <= 0) memberHunger[i] -= 15;
            if (memberBodyTemp[i] <= 32) memberHealth[i] -= 10;
            if (memberBodyTemp[i] <= 30) memberHealth[i] -= 20;
            if (memberInfection[i] >= 100) memberHealth[i] = 0; //������ 100% �̻��̸� ��� ó��

            //�ּҰ� ����
            memberHunger[i] = Mathf.Max(0, memberHunger[i]);
            memberBodyTemp[i] = Mathf.Max(25, memberBodyTemp[i]);
            memberHealth[i] = Mathf.Max(0, memberHealth[i]);
            memberInfection[i] = Mathf.Min(100, memberInfection[i]); //�������� 100%�� ���� ����



        }
    }

    public void NextDay()
    {
        currentDay += 1;

        usedItemCountToday =0;
        ProcessDilyChange();
        CheckRandomEvent();
        UpdateUI();
        CheckGameOver();

        //���� �� �ʱ�ȭ
       // hasUsedFoodToday = false;         
       // hasUsedMedicineToday = false;    
       // hasUsedFuelToday = false;         
       // hasUsedVaccineToday = false;      ���� �ڵ�

    }

    string GetMemberStatus(int memberIndex)
    {
        //��� üũ
        if (memberHealth[memberIndex] <= 0)
            return "(���)";

        //���� ������ ���º��� üũ
        if (memberBodyTemp[memberIndex] <= 30) return "(�ɰ��� ��ü����)";
        else if (memberHealth[memberIndex] <= 20) return "(����)";
        else if (memberHunger[memberIndex] <= 10) return "(���ָ�)";
        else if (memberBodyTemp[memberIndex] <= 32) return "(��ü����)";
        else if (memberHealth[memberIndex] <= 50) return "(����)";
        else if (memberHunger[memberIndex] <= 30) return "(�����)";
        else if (memberBodyTemp[memberIndex] <= 35) return "(����)";
        else if (memberInfection[memberIndex] <= 45) return "(���� ����)";
        
            
        
        else
            return "(�ǰ�)";


    }

    void CheckGameOver()
    {
        int aliveCount = 0;

        for (int i = 0; i < memberHealth.Length; i++)
        {
            if (memberHealth[i] > 0) aliveCount++;
        }

        if (aliveCount == 0)
        {
            nextDayButton.interactable = false;
            Debug.Log("���� ����! ��� �������� ���̷������� �̰ܳ��� ���߽��ϴ�.");
        }
    }

    void UpdateTextColor(Text text, int health)
    {
        if (health <= 0)
            text.color = Color.gray;
        else if (health <= 20)
            text.color = Color.red;
        else if (health < 50)
            text.color = Color.yellow;
        else
            text.color = Color.white;
    }

    public void UseFoodItem()                                         //���� ������ ���
    {

        if (usedItemCountToday >= dailyItemUseLimit) return;
        //if(hasUsedFoodToday) return;                    �����ڵ�      //���� ���� ��� ���� Ȯ��
        if (food <= 0 || foodItem == null) return;                   //���� ���� ó��

        int aliveCount = GetAlivememberCount();      //����ִ� ��� ��
        if (food < aliveCount) return;

        food -= aliveCount;                    //�ο� �� ��ŭ �Ҹ�
        UseItemOnAllMembers(foodItem);
        usedItemCountToday++;                                   //���� ���� ��� ����
        UpdateUI();
    }

    public void UseFuelItem()                                         //���� ������ ���
    {
        // if (hasUsedFuelToday) return;

        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (fuel <= 0 || fuelItem == null) return;                   //���� ���� ó��

        int aliveCount = GetAlivememberCount();      //����ִ� ��� ��

        fuel--;
        UseItemOnAllMembers(fuelItem);
        usedItemCountToday++;                                 //���� ���� ��� ����
        UpdateUI();
    }

    public void UseMedicItem()                                         //���� ������ ���
    {
        //if(hasUsedMedicineToday) return;
        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (medicine <= 0 || medicineItem == null) return;                   //���� ���� ó��

        int aliveCount = GetAlivememberCount();

        medicine -= aliveCount;
        UseItemOnAllMembers(medicineItem);
        usedItemCountToday++;                                   //���� �Ǿ�ǰ ��� ����
        UpdateUI();
    }

    public void Usevaccinetem()                                         //���� ������ ���
    {
        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (hasUsedVaccineToday) return;

        if (vaccine <= 0 || vaccineItem == null) return;                   //���� ���� ó��

        int aliveCount = GetAlivememberCount();

        vaccine -= aliveCount;
        UseItemOnAllMembers(vaccineItem);
        usedItemCountToday++;                                     //���� ��� ��� ����
        UpdateUI();
    }

    void UseItemOnAllMembers(ItemSO item)
    {
        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberHealth[i] > 0)                    //����ִ� ������
            {
                ApplyItemEffect(i, item);
            }
        }
    }


    public void GiveFoodToMember(int memberIndex)
    {
        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (food <= 0 || foodItem == null) return;
        if (memberHealth[memberIndex] <= 0) return;

        int aliveCount = GetAlivememberCount();

        food--;
        ApplyItemEffect(memberIndex, foodItem);
        usedItemCountToday++;
        UpdateUI();
    }

    public void HealMember(int memberIndex)
    {
        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (medicine <= 0 || medicineItem == null) return;
        if (memberHealth[memberIndex] <= 0) return;

        int aliveCount = GetAlivememberCount();

        medicine--;
        ApplyItemEffect(memberIndex, medicineItem);
        usedItemCountToday++;
        UpdateUI();
    }

    public void VaccineMember(int memberIndex)
    {
        if (usedItemCountToday >= dailyItemUseLimit) return;
        if (vaccine <= 0 || vaccineItem == null) return;
        if (memberInfection[memberIndex] <= 0) return;

        int aliveCount = GetAlivememberCount();

        vaccine--;
        ApplyItemEffect(memberIndex, vaccineItem);
        usedItemCountToday++;
        UpdateUI();
    }
    private int GetAlivememberCount()
    {
        int count = 0;
        for (int i = 0; i < memberHealth.Length; i++)
        {
            if (memberHealth[i] > 0) count++;
        }
        return count;
    }
    void ApplyItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        //���� Ư�� �����ؼ� ������ ȿ�� ���
        int actualHealth = Mathf.RoundToInt(item.healthEffect * member.recoveryRate);
        int actualHunger = Mathf.RoundToInt(item.hungerEffect * member.foodEfficiency);
        int actualTemp = item.tempEffect;
        int actualInfection = Mathf.RoundToInt(item.infectionEffect * member.zombieResistance);

        //ȿ�� ����
        memberHealth[memberIndex] += actualHealth;
        memberHunger[memberIndex] += actualHunger;
        memberBodyTemp[memberIndex] += actualTemp;
        memberInfection[memberIndex] -= actualInfection;

        //�ִ�ġ ����
        memberHealth[memberIndex] = Mathf.Min(memberHealth[memberIndex], member.maxHealth);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
        memberBodyTemp[memberIndex] = Mathf.Min(memberBodyTemp[memberIndex], member.normalBodyTemp);
        memberInfection[memberIndex] = Mathf.Max(0, memberInfection[memberIndex]); //�������� 0% �̻�

    }

    void ApplyEventEffects(EventSO eventSO)
    {
        //�ڿ���ȭ
        food += eventSO.foodChange;
        fuel += eventSO.fuelChange;
        medicine += eventSO.medicineChange;
        vaccine += eventSO.vaccineChange; 

        //�ڿ� �ּҰ� ����
        food = Mathf.Max(0, food);
        fuel = Mathf.Max(0, fuel);
        medicine = Mathf.Max(0, medicine);
        vaccine = Mathf.Max(0, vaccine);

        //��� ����ִ� ������� ���� ��ȭ����
        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberHealth[i] > 0)
            {
                memberHealth[i] += eventSO.healthChange;
                memberHunger[i] += eventSO.hungerChange;
                memberBodyTemp[i] += eventSO.tempChange;
                memberInfection[i] += eventSO.infectionChange;

                //���Ѱ� ����
                GroupMemberSO member = groupMembers[i];
                memberHealth[i] = Mathf.Clamp(memberHealth[i], 0, member.maxHealth);
                memberHunger[i] = Mathf.Clamp(memberHunger[i], 0, member.maxHunger);
                memberBodyTemp[i] = Mathf.Clamp(memberBodyTemp[i], 0, member.normalBodyTemp);
                memberInfection[i] = Mathf.Clamp(memberInfection[i], 0, 100); 

            }
        }
    }

   

    void ShowEventPopup(EventSO eventSO)
    {
        //�˾� Ȱ��ȭ
        eventPopup.SetActive(true);

        //�ؽ�Ʈ ����
        eventTitleText.text = eventSO.eventTitel;
        eventDescriptionText.text = eventSO.eventDescription;

        //�̺�Ʈ ȿ�� ����
        ApplyEventEffects(eventSO);

        if(eventSO.eventSound != null)
        {
            audioSource.PlayOneShot(eventSO.eventSound); //�̺�Ʈ ���� ���
        }

        //���� ���� �Ͻ�����
        nextDayButton.interactable = false;


    }

    public void CloseEventPopup()
    {
        eventPopup.SetActive(false);
        nextDayButton.interactable = true;
        UpdateUI();
    }

    public void OpenInvetoryPopup()
    {
        inventoryPanel.SetActive(true);
        UpdateUI();
    }

    public void CloseInventoryPopup()
    {
        inventoryPanel.SetActive(false);
        UpdateUI();
    }    

    public void OpenItemPopup()
    {
        ItemPopup.SetActive(true);
        UpdateUI();
    }

    public void CloseItemPopup()
    {
        ItemPopup.SetActive(false);
        UpdateUI();
    }

    void CheckRandomEvent()
    {
        int totalProbability = 0;

        //��ü Ȯ�� �� ���ϱ�
        for (int i = 0; i < events.Length; i++)
        {
            totalProbability += events[i].probability;
        }

        if (totalProbability == 0)
            return;

        int roll = Random.Range(1, totalProbability + 1 + 50);            //��üȮ�� ���ϱ⿡ �ƹ��͵� ���� Ȯ�� 50
        int cumualtive = 0;

        for (int i = 0; i < events.Length; i ++)
        {
            cumualtive += events[i].probability;
            if (roll <= cumualtive)
            {
                ShowEventPopup(events[i]);
                return;
            }
        }
    }

}
