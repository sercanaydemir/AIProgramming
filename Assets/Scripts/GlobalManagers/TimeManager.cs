using UnityEngine;
using System;

namespace Ravenholm.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        
        private DateTime currentDate;
        private float timeElapsed;
        public int secondsPerDay = 1440; // Customize based on your desired progression speed

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

            currentDate = new DateTime(2184, 3, 24); // Example start date
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;
            
            if(timeElapsed/24 > 1)
            {
                timeElapsed = 0;
                currentDate = currentDate.AddHours(1);
                OnDayChanged?.Invoke(currentDate);
                OnHourChanged?.Invoke();
            }

            // if (timeElapsed >= secondsPerDay)
            // {
            //     timeElapsed = 0;
            //     currentDate = currentDate.AddDays(1);
            //     OnDayChanged?.Invoke(currentDate);
            // }
        }
        

        public DateTime GetCurrentDate()
        {
            return currentDate;
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
        public static event Action<DateTime> OnDayChanged;
        public static event Action OnHourChanged; 
    }
}
