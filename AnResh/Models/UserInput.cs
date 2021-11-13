using System;
using System.IO;
using System.Web.Configuration;

namespace AnResh.Models
{
    public class UserInput
    {
        private static string _relativeFilePath = WebConfigurationManager.AppSettings["RelativeTextFilePath"];
        private string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _relativeFilePath);

        public string Text { get; private set; }

        public void UpdateFromFile()
        {
            try
            {
                Text = File.ReadAllText(_path);
            }
            catch
            {
                File.Create(_path);
            }
        }

        public void SaveToFile(string text)
        {
            File.WriteAllText(_path, text);
        }
    }
}