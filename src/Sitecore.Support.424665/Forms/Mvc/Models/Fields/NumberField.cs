using Sitecore.Data.Items;
using Sitecore.Form.Core.Utility;
using Sitecore.Forms.Mvc.Attributes;
using Sitecore.Forms.Mvc.Models;
using Sitecore.Forms.Mvc.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public class NumberField : FieldModel
  {
    private object value;

    public NumberField(Item item) : base(item)
    {
      this.SetProperties();
    }

    protected string GetParameter(Dictionary<string, string> dictionary, string key, string defaultValue)
    {
      if (string.IsNullOrEmpty(key))
      {
        return defaultValue;
      }
      KeyValuePair<string, string> pair = dictionary.FirstOrDefault<KeyValuePair<string, string>>(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
      if (pair.Key == null)
      {
        return defaultValue;
      }
      return pair.Value;
    }

    private void SetProperties()
    {
      Dictionary<string, string> dictionary = base.ParametersDictionary.Union<KeyValuePair<string, string>>(base.LocalizedParametersDictionary).ToDictionary<KeyValuePair<string, string>, string, string>(x => x.Key, y => y.Value);
      foreach (PropertyInfo info in base.GetType().GetProperties())
      {
        ParameterNameAttribute attribute = info.GetCustomAttributes(typeof(ParameterNameAttribute)).FirstOrDefault<Attribute>() as ParameterNameAttribute;
        if (attribute != null)
        {
          DefaultValueAttribute attribute2 = info.GetCustomAttributes(typeof(DefaultValueAttribute)).FirstOrDefault<Attribute>() as DefaultValueAttribute;
          string str = this.GetParameter(dictionary, attribute.Name, (attribute2 != null) ? attribute2.Value.ToString() : null);
          if (str != null)
          {
            ReflectionUtils.SetProperty(this, info.Name, str, true);
          }
        }
      }
    }

    [DefaultValue(0x7fffffff), ParameterName("MaximumValue")]
    public int MaximumValue { get; set; }

    [DefaultValue(0), ParameterName("MinimumValue")]
    public int MinimumValue { get; set; }

    [RegularExpression(@"^[-,+]{0,1}\d*\.{0,1}\d+$", ErrorMessage = "Field contains an invalid number."), DataType(DataType.Text), DynamicRange("MinimumValue", "MaximumValue", ErrorMessage = "The number in {0} must be at least {1} and no more than {2}.")]
    public override object Value
    {
      get
      {
        return this.value;
      }
      set
      {
        int num;
        if (!int.TryParse(value.ToString(), out num))
        {
          this.value = string.Empty;
        }
        else
        {
          this.value = num;
        }
      }
    }
  }
}
