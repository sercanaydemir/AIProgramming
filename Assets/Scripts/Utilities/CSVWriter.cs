using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter
{
    // Örnek veri sınıfı
    [System.Serializable]
    public class PlayerData
    {
        public int day;
        public string dayTime;
        public float hungry;
        public float fatigue;
        public float social;
        public float morale;
    }

    // Bir liste oluşturuyoruz
    public List<PlayerData> playerDataList = new List<PlayerData>();

    // CSV dosyasını kaydedecek fonksiyon
    public void SaveCSV()
    {
        string filePath = Application.dataPath + "/PlayerData.csv"; // Dosya yolunu belirleme
        StreamWriter writer = new StreamWriter(filePath); // Yazıcı başlatma
        
        Debug.LogError("path: " + filePath);
        
        // İlk satıra başlıkları yazıyoruz
        writer.WriteLine("Day;Day Time;Hungry;Fatigue,Social;Morale");

        // Her bir veriyi CSV formatında yazıyoruz
        foreach (PlayerData data in playerDataList)
        {
            string line = $"{data.day};{data.dayTime};{data.hungry};{data.fatigue};{data.social};{data.morale}";
            writer.WriteLine(line);
        }

        writer.Flush(); // Verileri dosyaya aktarma
        writer.Close(); // Dosyayı kapatma

        Debug.Log($"Veriler {filePath} adresine kaydedildi.");
    }
    
    public void AddData(int day, string dayTime, float hungry, float fatigue, float social, float morale)
    {
        PlayerData newData = new PlayerData();
        newData.day = day;
        newData.dayTime = dayTime;
        newData.hungry = hungry;
        newData.fatigue = fatigue;
        newData.social = social;
        newData.morale = morale;

        playerDataList.Add(newData); // Listeye veri ekleme
        SaveCSV(); // Her veri eklendiğinde CSV'yi kaydetme

        Debug.Log("Yeni veri eklendi ve CSV dosyasına kaydedildi.");
    }
}