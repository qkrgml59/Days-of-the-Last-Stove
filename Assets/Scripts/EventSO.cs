using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Event", menuName = "Game/Event")]
public class EventSO : ScriptableObject
{
    [Header("이벤트 정보")]
    public string eventTitel = "이벤트 발생!";
    [TextArea(3, 5)]
    public string eventDescription = "무슨 일이 일어났습니다.";

    [Header("자원 변화")]
    public int foodChange = 0;
    public int fuelChange = 0;
    public int medicineChange = 0;
    public int vaccineChange = 0; // 억제제 변화

    [Header("멤버 상태 변화")]    
    public int healthChange = 0;          //체력 변화
    public int hungerChange = 0;
    public int tempChange = 0;
    public int infectionChange = 0;       //감염률 변화

    [Header("발생 확률")]
    [Range(1, 100)]
    public int probability = 30;
}
