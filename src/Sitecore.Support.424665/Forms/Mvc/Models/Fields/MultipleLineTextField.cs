using Sitecore.Data.Items;
using Sitecore.Forms.Mvc.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public class MultipleLineTextField : SingleLineTextField
  {
    public MultipleLineTextField(Item item) : base(item)
    {
    }

    public int Columns { get; set; }

    [ParameterName("MaxLength"), DefaultValue(0x200)]
    public override int MaxLength { get; set; }

    public override string ResultParameters =>
        "multipleline";

    public int Rows { get; set; }

    [DataType(DataType.MultilineText)]
    public override object Value { get; set; }
  }
}
