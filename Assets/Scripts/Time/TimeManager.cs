using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//시간을 관리하는 매니저 클래스
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField]
    TimeManage Calctime;

    //시간이 흘러가는 속도 조절
    public float timeScale = 4.0f;

    //해의 포지션
    public Transform sunT;

    //시간을 관리해야할 오브젝트의 리스트
    List<TimeTracker> listTime = new List<TimeTracker>();

    //싱글톤 클래스로 설정
    private void Awake()
    {
        //이것 이외에 한개 더 있을경우 오브젝트 삭제
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Calctime = new TimeManage(0, TimeManage.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(TimeManage timestamp)
    {
        this.Calctime = new TimeManage(timestamp);
    }

    IEnumerator TimeUpdate()
	{
        while(true)
		{
            yield return new WaitForSeconds(1/ timeScale);
            SetTick();
        }
	}

    //인게임 시간변화
    public void SetTick()
	{
        //시간업데이트
        Calctime.UpdateClock();

        //시간에 따른 작물들을 업데이트하기위해 TimeTracker에서 ClockUpdate 호출
        foreach(TimeTracker listtime in listTime)
		{
            listtime.ClockUpdate(Calctime);
		}

        //시간변화에 따른 해의 이동을 주기위한 함수 호출
        UpdateSunMove();

    }

    public void SkipTime(TimeManage timeToSkipTo)
	{
        int timeToSkipInMinutes = TimeManage.TimestampInMinutes(timeToSkipTo);

        int timeNowInMinutes = TimeManage.TimestampInMinutes(Calctime);

        int diffenceInMInutes = timeToSkipInMinutes - timeNowInMinutes;

        if (diffenceInMInutes <= 0) return;

        for(int i = 0; i < diffenceInMInutes; i++)
		{
            SetTick();
		}
	}

    void UpdateSunMove()
	{
        // 해당 분에 맞게 앵글을 바꿔주기위해 계산
        int timeInMinutes = TimeManage.HourToMinute(Calctime.hour) + Calctime.minute;

        //시간에 맞춰서 해의 각도를 바꿔주면서 해와 밤을 조절
        float sunAngle = .25f * timeInMinutes - 90;

        //해의 위치를 바꾼다.
        sunT.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //시간을 관리하는 오브젝트를 추가
    public void RegisterTrack(TimeTracker listtime)
	{
        listTime.Add(listtime);
	}

    //시간을 관리하는 오브젝트를 제거
    public void UnRegisterTrack(TimeTracker listtime)
    {
        listTime.Remove(listtime);
    }

    //시간오브젝트를 반환함
    public TimeManage GetGameTimemanage()
    {
        //클론을 만들어 리턴함
        return new TimeManage(Calctime);
    }


}
