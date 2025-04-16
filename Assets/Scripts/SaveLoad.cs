using System.Linq;
using UnityEngine;

public static class SaveLoad
{
    public class SaveModel
    {
        public int Item0 {get; set;}
        public int Item1 {get; set;}
        public int Item2 {get; set;}
        public int Item3 {get; set;}
        public int Item4 {get; set;}
        public int Item5 {get; set;}
        public int Item6 {get; set;}
        public int Item7 {get; set;}
        public int Item8 {get; set;}
        public int Item9 {get; set;}
        public int Money {get; set;}

        public string ToDelimitedString()
        {
            var values = GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(int) && p.Name.StartsWith("Item"))
                .OrderBy(p => int.Parse(p.Name.Substring(4)))
                .Select(p => p.GetValue(this).ToString())
                .ToList();

            values.Add(Money.ToString());

            return string.Join("|", values);
        }
    }

    public static void Save(SaveModel saveModel)
    {
        var saveString = saveModel.ToDelimitedString();

        PlayerPrefs.SetString("IdleSave", saveString);

        Debug.Log("Game saved");
    }

    public static string Load()
    {
        string data = PlayerPrefs.GetString("IdleSave");

        Debug.Log("Game loaded");

        return data;
    }
}
