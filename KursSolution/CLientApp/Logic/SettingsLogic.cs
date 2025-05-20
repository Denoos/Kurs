using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CLientApp.Model;

namespace CLientApp.Logic
{
    public class SettingsLogic : INotifyPropertyChanged
    {
        private static SettingsLogic instance;
        public static SettingsLogic Instance { get => instance ??= new(); }

        private CustomSettings sett;
        public CustomSettings Settings { get => sett; set { sett = value; Signal(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Signal([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        SettingsLogic()
        {
            SettingsFileIsCreated();
            ReadCurrentSettings();
        }

        public void SaveNewSettings(CustomSettings settings)
        {
            if (settings.Color is not null)
                Settings.Color = settings.Color;
            if (settings.FontSize != 0)
                Settings.FontSize = settings.FontSize;
            if (settings.RadioIsWorking != null)
                Settings.RadioIsWorking = settings.RadioIsWorking;

            var currentSettings = $"color:#FF4500;fontsize:16;radioworking:true;-;color:{Settings.Color};fontsize:{Settings.FontSize};radioworking:{Settings.RadioIsWorking};";

            File.WriteAllText($"{Environment.CurrentDirectory}/config.txt", currentSettings);
        }

        private void ReadCurrentSettings()
        {
            var allText = File.ReadAllText($"{Environment.CurrentDirectory}/config.txt");

            var splittedForDefCust = allText.Split('-', StringSplitOptions.RemoveEmptyEntries);
            var checkCustom = splittedForDefCust[1].Split(';', StringSplitOptions.RemoveEmptyEntries);

            bool isCustomColor = !checkCustom[0].EndsWith(':');
            bool isCustomFont = !checkCustom[1].EndsWith(':');
            bool isCustomRadio = !checkCustom[2].EndsWith(':');

            var color = "";
            var font = "";
            var radio = "";

            if (isCustomColor)
                color = splittedForDefCust[1].Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            else
                color = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            if (isCustomFont)
                font = splittedForDefCust[1].Split(';', StringSplitOptions.RemoveEmptyEntries)[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            else
                font = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            if (isCustomRadio)
                radio = splittedForDefCust[1].Split(';', StringSplitOptions.RemoveEmptyEntries)[2].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            else
                radio = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[2].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            SetSettings(color, int.Parse(font), bool.Parse(radio));
        }

        public void UseDeafaultSettings()
        {
            var allText = File.ReadAllText($"{Environment.CurrentDirectory}/config.txt");

            var splittedForDefCust = allText.Split('-', StringSplitOptions.RemoveEmptyEntries);

            var color = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            var font = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            var radio = splittedForDefCust[0].Split(';', StringSplitOptions.RemoveEmptyEntries)[2].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            SetSettings(color, int.Parse(font), bool.Parse(radio));
        }

        private void SettingsFileIsCreated()
        {
            if (!File.Exists($"{Environment.CurrentDirectory}/config.txt"))
            {
                File.Create($"{Environment.CurrentDirectory}/config.txtconfig.txt");
                File.WriteAllText($"{Environment.CurrentDirectory}/config.txt", "color:#FF4500;fontsize:16;radioworking:true;-;color:;fontsize:;radioworking:");
            }
        }

        private void SetSettings(string color, int fontsize, bool radioIsWork)
            => Settings = new() { Color = color, FontSize = fontsize, RadioIsWorking = radioIsWork };
    }
}