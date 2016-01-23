using MongoRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Ideative.Mvc
{
    public enum FieldType
    {
        TextBox = 0,
        TextArea = 1,
        DropDown = 2,
        CheckBox = 3,
        Label = 4
    }


    public class DataForm : Entity
    {
        string _template;
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Template
        {
            get
            {
                return HttpUtility.HtmlDecode(_template);
            }

            set
            {
                string sanitized = HttpUtility.HtmlEncode(value);
                _template = sanitized;
            }
        }
        public List<DataField> Fields { get; set; }
        public string SubmitUrl { get; set; }
        public string SubmitName { get; set; }
    }
    public class DataField : Entity
    {
        //public int Id { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string DisplayLabel { get; set; }
        [Required]
        public FieldType DisplayType { get; set; }
        [Required]
        public bool IsMandatory { get; set; }

        public string FormId { get; set; }
        [ForeignKey("FormId")]
        [ScriptIgnore]
        public DataForm ParentForm { get; set; }
    }
}
