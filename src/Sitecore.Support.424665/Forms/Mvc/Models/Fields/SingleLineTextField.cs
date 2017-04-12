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
  public class SingleLineTextField : FieldModel
  {
    [ParameterName("MaxLength"), DefaultValue(256)]
    public virtual int MaxLength
    {
      get;
      set;
    }

    [ParameterName("MinLength"), DefaultValue(0)]
    public int MinLength
    {
      get;
      set;
    }

    [DynamicStringLength("MinLength", "MaxLength", ErrorMessage = "The field {0} must be a string with a minimum length of {1} and a maximum length of {2}."), DataType(DataType.Text)]
    public override object Value
    {
      get;
      set;
    }

    public SingleLineTextField(Item item) : base(item)
    {
      this.SetProperties();
    }

    protected string GetParameter(Dictionary<string, string> dictionary, string key, string defaultValue)
    {
      if (string.IsNullOrEmpty(key))
      {
        return defaultValue;
      }
      KeyValuePair<string, string> keyValuePair = dictionary.FirstOrDefault((KeyValuePair<string, string> x) => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
      if (keyValuePair.Key == null)
      {
        return defaultValue;
      }
      return keyValuePair.Value;
    }

    private void SetProperties()
    {
      Dictionary<string, string> dictionary = base.ParametersDictionary.Union(base.LocalizedParametersDictionary).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> y) => y.Value);
      PropertyInfo[] properties = base.GetType().GetProperties();
      for (int i = 0; i < properties.Length; i++)
      {
        PropertyInfo propertyInfo = properties[i];
        ParameterNameAttribute parameterNameAttribute = propertyInfo.GetCustomAttributes(typeof(ParameterNameAttribute)).FirstOrDefault<Attribute>() as ParameterNameAttribute;
        if (parameterNameAttribute != null)
        {
          DefaultValueAttribute defaultValueAttribute = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute)).FirstOrDefault<Attribute>() as DefaultValueAttribute;
          string parameter = this.GetParameter(dictionary, parameterNameAttribute.Name, (defaultValueAttribute != null) ? defaultValueAttribute.Value.ToString() : null);
          if (parameter != null)
          {
            ReflectionUtils.SetProperty(this, propertyInfo.Name, parameter, true);
          }
        }
      }
    }
  }
}
