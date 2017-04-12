using Sitecore.Data.Items;
using Sitecore.Form.Core.Controls.Data;
using Sitecore.StringExtensions;
using System.ComponentModel.DataAnnotations;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public class PasswordField : SingleLineTextField
  {
    public PasswordField(Item item) : base(item)
    {
    }

    public override ControlResult GetResult() =>
        new ControlResult(base.ID.ToString(), base.Title, this.Value, "secure:<schidden>{0}</schidden>".FormatWith(new object[] { this.Value }));

    [DataType(DataType.Password)]
    public override object Value { get; set; }
  }
}
