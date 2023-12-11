using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//시간을 추가하고 관리하는 열거형이 있음
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

	//클래스 재지정
	public TimeManage(int year, Season season, int day, int hour, int minute)
	{
		this.year = year;
		this.season = season;
		this.day = day;
		this.hour = hour;
		this.minute = minute;
	}


	//인스턴스를 통해 자기자신을 복제할수있다.
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

		//60분이 지나면 시간이 지나게 바뀜
		if(minute >= 60)
		{
			//분을 초기화하고 시간 늘림
			minute = 0;
			hour++;
		}

		//24시간이 지나면 초기화
		if(hour >= 24)
		{
			//시간 초기화하고 날을 늘림
			hour = 0;
			day++;
		}

		//30일이 넘어갈시 날 초기화
		if(day > 30)
		{
			//날 초기화
			day = 1;

			//겨울을 제외하면 모든 계절을 자동으로 바꿈
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

		//요일을 7로나눠서 해당 요일을 계산
		int dayIndex = daysWeek % 7;
		
		//열거형 요일 반환
		return (DayWeek)dayIndex;
	}

	public static int HourToMinute(int hour)
	{
		//60분을 한시간으로 바꿈
		return hour * 60;
	}

	public static int DaysToHours(int days)
	{
		//24시간을 하루로 바꿈
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
		//모든 날에 분을 합친시간 - 모든날에 분을 합친시간
		int timemange1Hour = DaysToHours(YearsToDays(timemanage1.year)) + DaysToHours(SeasonsToDays(timemanage1.season)) + DaysToHours(timemanage1.day) + timemanage1.hour;
		int timemange2Hour = DaysToHours(YearsToDays(timemanage2.year)) + DaysToHours(SeasonsToDays(timemanage2.season)) + DaysToHours(timemanage2.day) + timemanage2.hour;
		int diff = timemange2Hour - timemange1Hour;
		
		//음수인지 양수인지 확인 결과가
		return Mathf.Abs(diff);
	}
}
