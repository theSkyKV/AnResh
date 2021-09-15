using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace AnResh.Models
{
    public class UserInput
    {
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Text.txt";

        public string Text { get; private set; }

        public void UpdateFromFile()
        {
            Text = File.ReadAllText(_path);
        }

        public void SaveToFile(string text)
        {
            File.WriteAllText(_path, text);
        }
    }
}