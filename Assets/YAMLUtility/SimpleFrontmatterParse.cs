using System;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEditor;

namespace YAMLUtility
{
	public static class SimpleFrontmatterParse
	{
		public static T FromYAML<T>(string data) where T : new()
		{
			var values = new T();
			//split the data up by lines, split each line up by : to make elements.
			//work through the symbols and identify keys and values, and create a list of keyvalues
			//for each piece of parsed data, search T for a member that has the same name as the key
			//attempt to parse value into the type of member.
			int indentLevel = 0;
			List<YAMLKeyValue> matter = new List<YAMLKeyValue>();
			var lines = data.Split('\n');
			foreach (var line in lines)
			{
				var elements = line.Split(':');
				if (elements is { Length: 2 })
				{
					matter.Add(new YAMLKeyValue(elements[0],elements[1]));
				}
				else
				{
					//i need a real YAML parser :(
					//I think i can do it, but first lets figure out how the reflection works.
				}
				
				///
				var type = typeof(T);
				var properties = type.GetFields();
				foreach (var item in matter)
				{
					foreach (var field in properties)
					{
						if (field.Name.ToLower() == item.key.ToLower())
						{
							if (field.FieldType == typeof(string))
							{
								string noQuotes = (item.value[0] == '"' && item.value[^1] == '"') ? item.value.Substring(1, item.value.Length - 2) : item.value;
								field.SetValue(values, noQuotes);
							}else if (field.FieldType == typeof(int))
							{
								field.SetValue(values, int.Parse(item.value));
							}else if (field.FieldType == typeof(float))
							{
								field.SetValue(values,float.Parse(item.value));
							}else if (field.FieldType == typeof(char))
							{
								field.SetValue(values,char.Parse(item.value));
							}else if (field.FieldType == typeof(double))
							{
								field.SetValue(values,double.Parse(item.value));
							}else if (field.FieldType == typeof(bool))
							{
								field.SetValue(values,bool.Parse(item.value));
							}
						}
					}
				}
			}
			return values;
		}
	}


	public class YAMLKeyValue
	{
		public string key;
		public string value;

		public YAMLKeyValue(string key, string val)
		{
			this.key = key.Trim();
			this.value = val.Trim();
		}
	}
}