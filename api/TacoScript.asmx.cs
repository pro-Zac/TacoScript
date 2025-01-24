using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TacoScript.ClassLibs;

namespace TacoScript.api
{
    /// <summary>
    /// Summary description for TacoScript
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TacoScript : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetVersion()
        {
            return "2025.01.24";
        }

        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 inputObject, string parameter)
        {
            OptionObject2015 returnObject = new OptionObject2015();
            switch (parameter)
            {
                case "require":
                    returnObject = Require(inputObject);
                    break;
                case "notify":
                    returnObject = Notify(inputObject);
                    break;

            }

            return returnObject;
        }

        public static OptionObject2015 CopyObject(OptionObject2015 inputObject)
        {
            OptionObject2015 returnObject = new OptionObject2015();
            returnObject.EntityID = inputObject.EntityID;
            returnObject.EpisodeNumber = inputObject.EpisodeNumber;
            returnObject.OptionId = inputObject.OptionId;
            returnObject.Facility = inputObject.Facility;
            returnObject.SystemCode = inputObject.SystemCode;
            returnObject.ServerName = inputObject.ServerName;
            returnObject.NamespaceName = inputObject.NamespaceName;
            returnObject.ParentNamespace = inputObject.ParentNamespace;
            return returnObject;
        }

        private static OptionObject2015 Require(OptionObject2015 inputObject)
        {
            OptionObject2015 returnObject = CopyObject(inputObject);

            RowObject row = inputObject.Forms[0].CurrentRow;

            FieldObject dobField = new FieldObject();
            RowObject dobRow = new RowObject();
            FormObject dobForm = new FormObject();

            foreach (FieldObject field in row.Fields)
            {
                switch (field.FieldNumber)
                {
                    case "948.26":
                        dobField = field;
                        dobRow.RowId = row.RowId;
                        dobForm.FormId = inputObject.Forms[0].FormId;
                        dobForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                }
            }

            dobField.Enabled = "1";
            dobField.Required = "1";
            //dobField.FieldValue = 
            dobRow.Fields = new List<FieldObject>() { dobField };
            dobRow.RowAction = "EDIT";
            dobForm.CurrentRow = dobRow;
            dobForm.CurrentRow.ParentRowId = "0";

            returnObject.Forms = new List<FormObject>() { dobForm };

            return returnObject;
        }

        private static OptionObject2015 Notify(OptionObject2015 inputObject)
        {
            OptionObject2015 returnObject = CopyObject(inputObject); 
            
            RowObject row = inputObject.Forms[0].CurrentRow;

            string sourCream = null;
            foreach (FieldObject field in row.Fields)
            {
                switch (field.FieldNumber)
                {
                    case "948.3":
                        sourCream = field.FieldValue;
                        break;
                }
            }

            returnObject.ErrorCode = 1;
            returnObject.ErrorMesg = sourCream;

            return returnObject;
        }
    }
}
