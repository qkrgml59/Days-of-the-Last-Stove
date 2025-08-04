using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expendition : MonoBehaviour
{
    [Header("Ž�� ������")]
    public ExpeditionSO[] expeditions;     //Ž�� ������


    [Header("Ž�� UI")]
    public Button expeditionButton;                //Ÿ�� ���� ��ư
    public Button[] memberButtons;                 //��� ���� ��ư��
    public GameObject memberSelectPanel;               //��� ���� �г�    
    public Text expeditionInforText;                //���õ� Ž�� ����
    public Text resultText;                       //��� ǥ�� �ؽ�Ʈ

    private GameManager gameManager;
    private ExpeditionSO currentExpedition;

    [Header("��� �ý���")]
    public EquipmentSO[] availableEquiments;
    public Dropdown equipmentDropDown;

    public int selectedEquipmentIndex = 0;
    public int[] equipmentDurability;


    public void Start()
    {
        gameManager = GetComponent<GameManager>();

        memberSelectPanel.SetActive(false);
        resultText.text = "";
        expeditionInforText.text = "";

        expeditionButton.onClick.AddListener(OpenMemberSelect);

        for (int i = 0; i < memberButtons.Length; i++)
        {
            int memberIndex = i;
            memberButtons[i].onClick.AddListener(() => StartExpedition(memberIndex));  //��� ��ư Ŭ�� �� StartExpedition ȣ��
        }

        InitializoEquipmentDurability();

        SetupEquipmentDropDown();
        equipmentDropDown.onValueChanged.AddListener(OnEquipmentChanged);

    }

    void InitializoEquipmentDurability()
    {
        equipmentDurability = new int[availableEquiments.Length];

        for (int i = 0; i < availableEquiments.Length; i++)
        {
            equipmentDurability[i] = availableEquiments[i].maxDurability;
        }
    }

    void SetupEquipmentDropDown()
    {
        equipmentDropDown.options.Clear();

        for (int i = 0; i < availableEquiments.Length; i++)
        {
            string equipName = availableEquiments[i].equipmentName;

            if (i == 0)
            {
                equipmentDropDown.options.Add(new Dropdown.OptionData(equipName));
            }
            else if (equipmentDurability[i] <= 0)
            {
                equipmentDropDown.options.Add(new Dropdown.OptionData($"{equipName} (�η���)"));
            }
            else
            {
                equipmentDropDown.options.Add(new Dropdown.OptionData($"{equipName} ({equipmentDurability[i]} / {availableEquiments[i].maxDurability})"));
            }
        }

        equipmentDropDown.value = 0;
        equipmentDropDown.RefreshShownValue();
    }
            void OnEquipmentChanged(int equipmentIndex)
            {
                selectedEquipmentIndex = equipmentIndex;
                UpdateExpeditionInfo();
            }

            void UpdateExpeditionInfo()
            {
                if (currentExpedition != null)
                {
                    EquipmentSO selectedEquipment = availableEquiments[selectedEquipmentIndex];

                    int equipBouns = (selectedEquipmentIndex > 0 && equipmentDurability[selectedEquipmentIndex] <= 0) ? 0 : selectedEquipment.successBouns;
                    int totalSuccessRate = currentExpedition.baseSuccessRate + equipBouns;

                    string durabilityInfo = "";

                    if (selectedEquipmentIndex > 0)
                    {
                        if (equipmentDurability[selectedEquipmentIndex] <= 0) durabilityInfo = "(�η��� ���� - ȿ�� ����)";
                        else durabilityInfo = $"\n��� ������: {equipmentDurability[selectedEquipmentIndex]} / {selectedEquipment.maxDurability}";
                    }

                    expeditionInforText.text = $"Ž��: {currentExpedition.expeditionName}\n" +
                                               $"{currentExpedition.description}\n" +
                                               $"�⺻ ������ : {currentExpedition.baseSuccessRate}%\n" +
                                               $"��� ���ʽ� : +{equipBouns}%\n" +
                                               $"���� ������ :{totalSuccessRate}%\n";



                }
            }

            void UpdateMemberButtons()                                 //��� ��ư ������Ʈ ����
            {
                for (int i = 0; i < memberButtons.Length && i < gameManager.groupMembers.Length; i++)
                {
                    GroupMemberSO member = gameManager.groupMembers[i];
                    bool canGo = gameManager.memberHealth[i] > 20;                  //ü�� 20 �̻��� �� Ž�� ����

                    Text buttonText = memberButtons[i].GetComponentInChildren<Text>();
                    buttonText.text = $"{member.memberName}\n ü�� : {gameManager.memberHealth[i]}";
                    memberButtons[i].interactable = canGo;
                }
            }

        

    public void OpenMemberSelect()
    {
        //���ο� Ž�� ���� ����
        if (expeditions.Length > 0)
        {
            currentExpedition = expeditions[Random.Range(0, expeditions.Length)];
            UpdateExpeditionInfo();
        }

        memberSelectPanel.SetActive(true);
        UpdateMemberButtons();
    }

    public void StartExpedition(int memberIndex)
    {
        if (currentExpedition == null) return;

        memberSelectPanel.SetActive(false);

        GroupMemberSO member = gameManager.groupMembers[memberIndex];
        EquipmentSO selectedEquip = availableEquiments[selectedEquipmentIndex];

        //������ ��� (ExpeditionSO�� �⺻ ������ + ��� ���ʽ�)
        bool equipmentBroken = selectedEquipmentIndex > 0 && equipmentDurability[selectedEquipmentIndex] < 0;
        int rewardBouns = equipmentBroken ? 0 : selectedEquip.rewardBonus;
        int equipBouns = equipmentBroken ? 0 : selectedEquip.successBouns;


        int finalSuccessRate = currentExpedition.baseSuccessRate + equipBouns;
        finalSuccessRate = Mathf.Clamp(finalSuccessRate, 5, 95);

        bool success = Random.Range(1, 101) <= finalSuccessRate;

        if (selectedEquipmentIndex > 0 && !equipmentBroken)
        {
            equipmentDurability[selectedEquipmentIndex] -= 1;
            SetupEquipmentDropDown();
        }
        

        if (success)
        {
            //���� ExpeditionSO ���� ����
            gameManager.food += currentExpedition.sucessFoodReward;
            gameManager.fuel += currentExpedition.successFuelReward;
            gameManager.medicine += currentExpedition.successMedicineReward;

            //Ž�� �Ϸ� �� �ɹ� �ణ �Ƿ�
            gameManager.memberHunger[memberIndex] -= 5;

            resultText.text = $"{member.memberName} {currentExpedition.expeditionName} ����! (������ : {finalSuccessRate}%)\n" +
                         $"���� : {currentExpedition.sucessFoodReward + rewardBouns}, ���� + {currentExpedition.successFuelReward + rewardBouns}," +
                         $"��ǰ + {currentExpedition.successFuelReward + rewardBouns}";

            resultText.color = Color.green;
         }
        else
        {
            //���� : ExpeditionSO �г�Ƽ ����
            gameManager.memberHealth[memberIndex] += currentExpedition.failHealthPenalty;
            gameManager.memberHunger[memberIndex] += currentExpedition.failHungerPenalty;
            gameManager.memberBodyTemp[memberIndex] += currentExpedition.failTempPenalty;

            resultText.text = $"{member.memberName} {currentExpedition.expeditionName} ����! (������ : {finalSuccessRate}%)\n" +
                $"ü�� - {currentExpedition.failHealthPenalty}, ����� - {currentExpedition.failHungerPenalty}, " +
                $" �µ� - {currentExpedition.failTempPenalty}";

            resultText.color = Color.red;
        }

        //�ּҰ� ����

        GroupMemberSO memberSO = gameManager.groupMembers[memberIndex];
        gameManager.memberHunger[memberIndex] = Mathf.Max(0, gameManager.memberHunger[memberIndex]);
        gameManager.memberBodyTemp[memberIndex] = Mathf.Max(0, gameManager.memberBodyTemp[memberIndex]);
        gameManager.memberHealth[memberIndex] = Mathf.Max(0, gameManager.memberHealth[memberIndex]);

        gameManager.UpdateUI();

        Invoke("ClearResultText", 3f);

    }

    void ClearResultText()
    {
        resultText.text = "";
    }

}
