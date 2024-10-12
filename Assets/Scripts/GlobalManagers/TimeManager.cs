using UnityEngine;
using System;

namespace Ravenholm.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        
        private DateTime _currentDate;
        private int _minute;
        private float _tempMinute;
        private int _hour;
        private int _tempHour;
        private TimeOfDay _timeOfDay;
        public int secondsPerHour = 10; // Customize based on your desired progression speed
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _currentDate = new DateTime(2184, 3, 24,6,00,00); // Example start date
            
        }

        private void Start()
        {
            _hour = _currentDate.Hour;
            _timeOfDay = GetTimeOfDay();
            OnMinuteChanged?.Invoke();
            OnHourChanged?.Invoke();
            OnDayChanged?.Invoke(_currentDate);
            OnTimeOfDayChanged?.Invoke(_timeOfDay);
        }

        void Update()
        {
            _tempMinute += Time.deltaTime;
            
            CalculateTime();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4;
            }
        }
        
        void CalculateTime()
        {
            if (_tempMinute >= 1)
            {
                _minute++;
                _tempMinute = 0;
                OnMinuteChanged?.Invoke();
            }

            if(_minute >= secondsPerHour)
            {
                _tempHour++;
                _minute = 0;
            }
            
            if(_hour != _tempHour)
            {
                OnHourChanged?.Invoke();
                _hour = _tempHour;
                _currentDate = _currentDate.AddHours(1);
                OnDayChanged?.Invoke(_currentDate);
                
                if(_timeOfDay != GetTimeOfDay())
                {
                    _timeOfDay = GetTimeOfDay();
                    OnTimeOfDayChanged?.Invoke(_timeOfDay);
                }
                
                if(_hour >= 24)
                {
                    _hour = 0;
                    _tempHour = 0;
                    OnDayChanged?.Invoke(_currentDate);
                }
            }
        }
        
        private TimeOfDay GetTimeOfDay()
        {
            return _currentDate.Hour switch
            {
                >= 6 and < 9 => TimeOfDay.BeforeMorning,
                >= 9 and < 12 => TimeOfDay.Morning,
                >= 12 and < 13 => TimeOfDay.Noon,
                >= 13 and < 18 => TimeOfDay.Afternoon,
                >= 18 and < 23 => TimeOfDay.Evening,
                _ => TimeOfDay.Night
            };
        }

        public DateTime GetCurrentDate()
        {
            return _currentDate;
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
        public static event Action<DateTime> OnDayChanged;
        public static event Action OnHourChanged; 
        public static event Action OnMinuteChanged; 
        public static event Action<TimeOfDay> OnTimeOfDayChanged; 
    }
    
    public enum TimeOfDay
    {
        BeforeMorning = 0,
        Morning = 1,
        Noon = 2,
        Afternoon = 3,
        Evening = 4,
        Night = 5
    }
}
