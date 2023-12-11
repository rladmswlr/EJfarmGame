using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//클래스를 다중상속 받게 하기위한 인터페이스화
public interface TimeTracker
{
    void ClockUpdate(TimeManage timeManage);
}
