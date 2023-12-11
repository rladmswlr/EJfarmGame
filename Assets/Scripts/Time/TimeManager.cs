using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ð��� �����ϴ� �Ŵ��� Ŭ����
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField]
    TimeManage Calctime;

    //�ð��� �귯���� �ӵ� ����
    public float timeScale = 4.0f;

    //���� ������
    public Transform sunT;

    //�ð��� �����ؾ��� ������Ʈ�� ����Ʈ
    List<TimeTracker> listTime = new List<TimeTracker>();

    //�̱��� Ŭ������ ����
    private void Awake()
    {
        //�̰� �̿ܿ� �Ѱ� �� ������� ������Ʈ ����
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

    //�ΰ��� �ð���ȭ
    public void SetTick()
	{
        //�ð�������Ʈ
        Calctime.UpdateClock();

        //�ð��� ���� �۹����� ������Ʈ�ϱ����� TimeTracker���� ClockUpdate ȣ��
        foreach(TimeTracker listtime in listTime)
		{
            listtime.ClockUpdate(Calctime);
		}

        //�ð���ȭ�� ���� ���� �̵��� �ֱ����� �Լ� ȣ��
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
        // �ش� �п� �°� �ޱ��� �ٲ��ֱ����� ���
        int timeInMinutes = TimeManage.HourToMinute(Calctime.hour) + Calctime.minute;

        //�ð��� ���缭 ���� ������ �ٲ��ָ鼭 �ؿ� ���� ����
        float sunAngle = .25f * timeInMinutes - 90;

        //���� ��ġ�� �ٲ۴�.
        sunT.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //�ð��� �����ϴ� ������Ʈ�� �߰�
    public void RegisterTrack(TimeTracker listtime)
	{
        listTime.Add(listtime);
	}

    //�ð��� �����ϴ� ������Ʈ�� ����
    public void UnRegisterTrack(TimeTracker listtime)
    {
        listTime.Remove(listtime);
    }

    //�ð�������Ʈ�� ��ȯ��
    public TimeManage GetGameTimemanage()
    {
        //Ŭ���� ����� ������
        return new TimeManage(Calctime);
    }


}
