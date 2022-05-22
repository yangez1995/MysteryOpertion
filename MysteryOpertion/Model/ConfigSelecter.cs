using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model
{
    public class ConfigSelecter
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string[] Options { get; set; }
        public int Index { get; set; }
        public ConfigSelecter Parent { get; set; }
        public ConfigEntry<int> Entry { get; set; }
        public OptionBehaviour Behaviour { get; set; }
        public bool MarginTop { get; set; }
        

        public ConfigSelecter(string key, string name, string[] options, string section, int defaultValue, ConfigSelecter parent, bool marginTop)
        {
            this.Key = key;
            this.Name = name;
            this.Options = options;
            this.Entry = MysteryOpertionPlugin.Instance.Config.Bind<int>(section, key, defaultValue);
            this.Index = Entry.Value;
            this.Parent = parent;
            this.MarginTop = marginTop;

            ConfigLoader.selecters.Add(key, this);
        }

        public void Update(int index)
        {
            Index = index;
            if (Behaviour is not null)
            {
                var stringOption = (StringOption)Behaviour;
                stringOption.oldValue = Index;
                stringOption.Value = Index;
                stringOption.ValueText.text = Options[Index];
            }

            Entry.Value = Index;
            
        }

        public void Increase()
        {
            var max = Options.Length - 1;
            var index = Index == max ? 0 : Index + 1;
            Update(index);
        }

        public void Decrease()
        {
            var max = Options.Length - 1;
            var index = Index == 0 ? max : Index - 1;
            Update(index);
        }


        public string GetValue()
        {
            return Options[Index];
        }
        public int GetInt32Value()
        {
            return Convert.ToInt32(Options[Index]);
        }
        public float GetFloatValue()
        {
            return (float)Convert.ToDouble(Options[Index]);
        }
        public bool GetBoolValue()
        {
            return Options[Index] == "是";
        }
    }

    public static class ConfigSelecterFactory
    {
        public static ConfigSelecter Produce(string key, string name, string[] options, string section = "Other", int defaultValue = 0, ConfigSelecter parent = null, bool marginTop = false)
        {
            return new ConfigSelecter(key, name, options, section, defaultValue, parent, marginTop);
        }

        public static ConfigSelecter Produce(string key, string name, float min, float max, float interval, string section = "Other", int defaultValue = 0, ConfigSelecter parent = null, bool marginTop = false)
        {
            List<string> list = new List<string>();
            for (var i = min; i <= max; i += interval)
            {
                list.Add(i.ToString());
            }

            return new ConfigSelecter(key, name, list.ToArray(), section, defaultValue, parent, marginTop);
        }
    }
}
