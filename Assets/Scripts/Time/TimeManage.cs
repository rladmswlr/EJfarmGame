using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ð��� �߰��ϰ� �����ϴ� �������� ����
[System.Serializable]
public class TimeManage
{
    public int year;
    public enum Season
	{
		Spring,
		Summer,
		Fall,
		Winter
	}
	public Season season;

	public enum DayWeek
	{
		Sun,
		Mon,
		Tue,
		Wed,
		Thu,
		Fri,
		Sat
	}

	public int day;
	public int hour;
	public int minute;

	//Ŭ���� ������
	public TimeManage(int year, Season season, int day, int hour, int minute)
	{
		this.year = year;
		this.season = season;
		this.day = day;
		this.hour = hour;
		this.minute = minute;
	}


	//�ν��Ͻ��� ���� �ڱ��ڽ��� �����Ҽ��ִ�.
	public TimeManage(TimeManage timemanage)
	{
		this.year = timemanage.year;
		this.season = timemanage.season;
		this.day = timemanage.day;
		this.hour = timemanage.hour;
		this.minute = timemanage.minute;
	}

	public void UpdateClock()
	{
		minute++;

		//60���� ������ �ð��� ������ �ٲ�
		if(minute >= 60)
		{
			//���� �ʱ�ȭ�ϰ� �ð� �ø�
			minute = 0;
			hour++;
		}

		//24�ð��� ������ �ʱ�ȭ
		if(hour >= 24)
		{
			//�ð� �ʱ�ȭ�ϰ� ���� �ø�
			hour = 0;
			day++;
		}

		//30���� �Ѿ�� �� �ʱ�ȭ
		if(day > 30)
		{
			//�� �ʱ�ȭ
			day = 1;

			//�ܿ��� �����ϸ� ��� ������ �ڵ����� �ٲ�
			if(season == Season.Winter)
			{
				season = Season.Spring;
				year++;
			}
			else
			{
				season++;
			}
		}
	}

	public DayWeek GetDayWeek()
	{
		int daysWeek = YearsToDays(year) + SeasonsToDays(season) + day;

		//������ 7�γ����� �ش� ������ ���
		int dayIndex = daysWeek % 7;
		
		//������ ���� ��ȯ
		return (DayWeek)dayIndex;
	}

	public static int HourToMinute(int hour)
	{
		//60���� �ѽð����� �ٲ�
		return hour * 60;
	}

	public static int DaysToHours(int days)
	{
		//24�ð��� �Ϸ�� �ٲ�
		return days * 24;
	}

	public static int SeasonsToDays(Season season)
	{
		int seasonIndex = (int)season;
		return seasonIndex * 30 + 1;
	}

	public static int YearsToDays(int year)
	{
		return year * 4 * 30;
	}

	public static int TimestampInMinutes(TimeManage timestamp)
	{
		return HourToMinute(DaysToHours(YearsToDays(timestamp.year)) + DaysToHours(SeasonsToDays(timestamp.season)) + DaysToHours(timestamp.day) + timestamp.hour) + timestamp.minute;
	}

	public static int CompareTimeManage(TimeManage timemanage1, TimeManage timemanage2)
	{
		//��� ���� ���� ��ģ�ð� - ��糯�� ���� ��ģ�ð�
		int timemange1Hour = DaysToHours(YearsToDays(timemanage1.year)) + DaysToHours(SeasonsToDays(timemanage1.season)) + DaysToHours(timemanage1.day) + timemanage1.hour;
		int timemange2Hour = DaysToHours(YearsToDays(timemanage2.year)) + DaysToHours(SeasonsToDays(timemanage2.season)) + DaysToHours(timemanage2.day) + timemanage2.hour;
		int diff = timemange2Hour - timemange1Hour;
		
		//�������� ������� Ȯ�� �����
		return Mathf.Abs(diff);
	}
}
