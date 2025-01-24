using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TacoScript.ClassLibs
{
    public class FormObject
    {
        public RowObject CurrentRow { get; set; }
        public string FormId { get; set; }
        public bool MultipleIteration { get; set; }
        public List<RowObject> OtherRows { get; set; }
    }
}