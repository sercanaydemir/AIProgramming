using System;
using System.Collections.Generic;
using Ravenholm.Managers;
using UnityEngine;

namespace Ravenholm
{
    public class GUIPrinter : MonoBehaviour
    {
        private List<Notification> notifications = new List<Notification>();
        private DateTime currentDate;
        private bool hide;
        private void OnGUI()
        {
            if(hide) return;
            
            NotificationPrinter();
            ResourcesPrinter();
            TimePrinter();
        }

        private void NotificationPrinter()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            for (int i = 0; i < notifications.Count; i++)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 30;
                style.normal.textColor = notifications[i].GetColor();
                
                Texture2D grayTexture = new Texture2D(1, 1);
                grayTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.3f));
                
                style.normal.background = Texture2D.grayTexture;
                GUI.Label(new Rect((screenWidth/2)-screenWidth/4, screenHeight - (200+ i * 50),screenWidth/2 ,50), notifications[i].notification, style);
            }    
        }
        
        private void ResourcesPrinter()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            
            // string resources = "Energy: " + MiningManager.Instance.CurrentEnergy.ToString("F");
            //
            // Texture2D white = new Texture2D(1, 1);
            // white.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.25f));
            //     
            // style.normal.background = white;
            //
            // resources += " / Oxygen Level: " + MiningManager.Instance.OxygenLevel.ToString("F");
            //
            // for (int i = 0; i < MiningManager.Instance.GetCurrentResources().Count; i++)
            // {
            //     resources += " / " + MiningManager.Instance.GetCurrentResources()[i].gameResource + ": " + MiningManager.Instance.GetCurrentResources()[i].amount;
            // }
            //
            //GUI.Label(new Rect(20, 30,screenWidth ,50), resources, style);
            
        }

        private void TimePrinter()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;

            string resources = "Date: " + currentDate.ToShortDateString();
            
            GUI.Label(new Rect(20, 60,100 ,50), resources, style);
        }
        
        private void AddNotification(Notification notification)
        {
            notifications.Add(notification);
            
            if(notifications.Count>3)
                notifications.RemoveAt(0);
        }

        private void TimeManagerOnOnDayChanged(DateTime obj)
        {
            currentDate = obj;
        }
        private void HideGUI(bool obj)
        {
            hide = obj;
        }

        private void OnEnable()
        {
            OnNotificationAdded += AddNotification;
            TimeManager.OnDayChanged += TimeManagerOnOnDayChanged;
            OnHide += HideGUI;
        }
        private void OnDisable()
        {
            OnNotificationAdded -= AddNotification;
            TimeManager.OnDayChanged -= TimeManagerOnOnDayChanged; 
            OnHide -= HideGUI;
        }

        private static event Action<Notification> OnNotificationAdded;
        public static void AddNotificationStatic(Notification notification)
        {
            OnNotificationAdded?.Invoke(notification);
        }
        
        private static event Action<bool> OnHide;
        public static void Hide(bool show)
        {
            OnHide?.Invoke(show);
        }
    }
    
    public struct Notification
    {
        public string notification;
        public NotificationType type;
        
        public Notification(string notification, NotificationType type)
        {
            this.notification = notification;
            this.type = type;
        }
        public Color GetColor()
        {
            switch (type)
            {
                case NotificationType.Info:
                    return Color.white;
                case NotificationType.Warning:
                    return Color.yellow;
                case NotificationType.Error:
                    return Color.red;
            }
            
            return Color.white;
        }
    }
    
    public enum NotificationType
    {
        Info,
        Warning,
        Error
    }
}