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
        public ConfigEntry<int> Entry { get; set; }
        public OptionBehaviour Behaviour { get; set; }

        public ConfigSelecter(string key, string name, string[] options, string section, int defaultValue)
        {
            this.Key = key;
            this.Name = name;
            this.Options = options;
            this.Entry = MysteryOpertionPlugin.Instance.Config.Bind<int>(section, key, defaultValue);
            this.Index = defaultValue;
            
            ConfigSelecters.selecters.Add(this);
        }

        public void Update(int index)
        {
            Index = index;
            if(Behaviour is not null)
            {
                var stringOption = (StringOption)Behaviour;
                stringOption.oldValue = Index;
                stringOption.Value = Index;
                stringOption.ValueText.text = Options[Index];
            }
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
    }

    public static class ConfigSelecterFactory
    {
        public static ConfigSelecter Produce(string key, string name, string[] options, string section = "Other", int defaultValue = 0)
        {
            return new ConfigSelecter(key, name, options, section, defaultValue);
        }

        public static ConfigSelecter Produce(string key, string name, float min, float max, float interval, string section = "Other", int defaultValue = 0)
        {
            List<string> list = new List<string>();
            for (var i = min; i <= max; i += interval)
            {
                list.Add(i.ToString());
            }

            return new ConfigSelecter(key, name, list.ToArray(), section, defaultValue);
        }
    }

    public static class ConfigSelecters
    {
        public static List<ConfigSelecter> selecters = new List<ConfigSelecter>();
    }
}
