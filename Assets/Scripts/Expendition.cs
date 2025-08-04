using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expendition : MonoBehaviour
{
    [Header("탐방 데이터")]
    public ExpeditionSO[] expeditions;     //탐방 종류들


    [Header("탐방 UI")]
    public Button expeditionButton;                //타방 시작 버튼
    public Button[] memberButtons;                 //멤버 선택 버튼들
    public GameObject memberSelectPanel;               //멤버 선택 패널    
    public Text expeditionInforText;                //선택된 탐방 정보
    public Text resultText;                       //결과 표시 텍스트

    private GameManager gameManager;
    private ExpeditionSO currentExpedition;

    [Header("장비 시스템")]
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
            memberButtons[i].onClick.AddListener(() => StartExpedition(memberIndex));  //멤버 버튼 클릭 시 StartExpedition 호출
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
                equipmentDropDown.options.Add(new Dropdown.OptionData($"{equipName} (부러짐)"));
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
                        if (equipmentDurability[selectedEquipmentIndex] <= 0) durabilityInfo = "(부러진 상태 - 효과 없음)";
                        else durabilityInfo = $"\n장비 내구도: {equipmentDurability[selectedEquipmentIndex]} / {selectedEquipment.maxDurability}";
                    }

                    expeditionInforText.text = $"탐방: {currentExpedition.expeditionName}\n" +
                                               $"{currentExpedition.description}\n" +
                                               $"기본 성공률 : {currentExpedition.baseSuccessRate}%\n" +
                                               $"장비 보너스 : +{equipBouns}%\n" +
                                               $"최종 성공률 :{totalSuccessRate}%\n";



                }
            }

            void UpdateMemberButtons()                                 //멤버 버튼 업데이트 정보
            {
                for (int i = 0; i < memberButtons.Length && i < gameManager.groupMembers.Length; i++)
                {
                    GroupMemberSO member = gameManager.groupMembers[i];
                    bool canGo = gameManager.memberHealth[i] > 20;                  //체력 20 이상일 때 탐방 가능

                    Text buttonText = memberButtons[i].GetComponentInChildren<Text>();
                    buttonText.text = $"{member.memberName}\n 체력 : {gameManager.memberHealth[i]}";
                    memberButtons[i].interactable = canGo;
                }
            }

        

    public void OpenMemberSelect()
    {
        //새로운 탐방 랜덤 선택
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

        //성공률 계산 (ExpeditionSO의 기본 성공률 + 멤버 보너스)
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
            //성공 ExpeditionSO 보상 적용
            gameManager.food += currentExpedition.sucessFoodReward;
            gameManager.fuel += currentExpedition.successFuelReward;
            gameManager.medicine += currentExpedition.successMedicineReward;

            //탐방 완료 한 맴버 약간 피로
            gameManager.memberHunger[memberIndex] -= 5;

            resultText.text = $"{member.memberName} {currentExpedition.expeditionName} 성공! (성공률 : {finalSuccessRate}%)\n" +
                         $"음식 : {currentExpedition.sucessFoodReward + rewardBouns}, 연료 + {currentExpedition.successFuelReward + rewardBouns}," +
                         $"약품 + {currentExpedition.successFuelReward + rewardBouns}";

            resultText.color = Color.green;
         }
        else
        {
            //실패 : ExpeditionSO 패널티 적용
            gameManager.memberHealth[memberIndex] += currentExpedition.failHealthPenalty;
            gameManager.memberHunger[memberIndex] += currentExpedition.failHungerPenalty;
            gameManager.memberBodyTemp[memberIndex] += currentExpedition.failTempPenalty;

            resultText.text = $"{member.memberName} {currentExpedition.expeditionName} 실패! (성공률 : {finalSuccessRate}%)\n" +
                $"체력 - {currentExpedition.failHealthPenalty}, 배고픔 - {currentExpedition.failHungerPenalty}, " +
                $" 온도 - {currentExpedition.failTempPenalty}";

            resultText.color = Color.red;
        }

        //최소값 보정

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
