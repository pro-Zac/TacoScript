using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TacoScript.ClassLibs;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Odbc;
using System.Web.UI.WebControls;

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
        // this silly thing is required for some reason. All you need to do is ensure that it returns a string. Can be anything. Just has to be here. 
        [WebMethod]
        public string GetVersion()
        {
            return "2025.04.22"; // i change this to be the date of the last script modification. can check this against previous dates in github
        }

        // the actual Method()s you call for your scripts live here vvv This section must be RunScript(), and will contain (OptionObject2015 inputObject, string parameter) args
        // feel free to copy verbatim if you like. 
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 inputObject, string parameter)
        {
            OptionObject2015 returnObject = new OptionObject2015();
            switch (parameter)
            {
                // these are the parameters you put in the scriptlink section in Form Designer to call your script. You will change these parameters to match your Method()
                case "require":
                    returnObject = RequireField(inputObject);
                    break;
                case "pullinfo":
                    returnObject = PullInfo(inputObject);
                    break;
                case "configuretaco":
                    returnObject = ConfigureTaco(inputObject);
                    break;
            }

            return returnObject;
        }

        // this is a helper method that dramatically cuts down on the amount of typing you need to do when calling other scripts. 
        // Eventually you will understand what it is doing as you learn more about OOP. Until then, just copy it into your own script solution regardless. 
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

        /* a ScriptLink function executes a Method() based on an OptionObject2015 class. This will eventually make more sense, but if you want to 
         * create your own scripts, follow this format
         * private static [these can be left as is for now] OptionObject2015 YourMethod(OptionObject2015 inputObject) */
        private static OptionObject2015 ConfigureTaco(OptionObject2015 inputObject)
        {
            // invokes CopyObject() method to copy inputObject back to ReturnObject. copy verbatim
            OptionObject2015 returnObject = CopyObject(inputObject);
            // basically, this here states the current database row is the one you're calling up and modifying. just copy verbatim
            RowObject row = inputObject.Forms[0].CurrentRow;

            /* If you want to modify data in a form with ScriptLink, you MUST instantiate the FieldObject, RowObject, and FormObject for EACH field you are modifying
             * In the envelope provided, we will require both the Date of Birth field and the Client Info and Stuff field in the form. If you want to add more fields to require, 
             * then follow the same format as these */
            // instntiating the field, row, and form objects for the client name field, as we intend to modify it via ScriptLink
            FieldObject clientNameField = new FieldObject();
            RowObject clientNameRow = new RowObject();
            FormObject clientNameForm = new FormObject();

            // instntiating the field, row, and form objects for the client info scrolling freetext field, as we intend to modify it via ScriptLink
            FieldObject clientInfoField = new FieldObject();
            RowObject clientInfoRow = new RowObject();
            FormObject clientInfoForm = new FormObject();

            // same for tacoConfig element in the form. We are modifying it, so we need Field, Row, and Form Objects.
            FieldObject tacoConfigField = new FieldObject();
            RowObject tacoConfigRow = new RowObject();
            FormObject tacoConfigForm = new FormObject();

            /* if you want to just pull data from a field or use it to calculate something else, you don't have to instantiate the objects, you will just
             * declare your variables so your foreach loop will know what you're talking about. Since we've identified the fields we want ScriptLink to modify above, 
             * we can just declare variables as normal below. For simplicity's sake and to prevent logic issues, we will declare variables as null when possible.
             * This isn't always necessary, but when starting out it can simplify certain issues. */
            string likeTacos = null; string tacoToppings = null; string sourCream = null; string cheese = null;
            foreach (FieldObject field in row.Fields)
            {
                // You can find the field numbers in Form and Table Documentation for your form. 
                switch (field.FieldNumber)
                {
                    case "948.25": // you will need to change this number to match the element number in your system. The rest can be copied as-is.
                        clientNameField = field; // yes, all this in the current and next three lines is required to get ScriptLink to be able to modify this field in mA.
                        clientNameRow.RowId = row.RowId;
                        clientNameForm.FormId = inputObject.Forms[0].FormId;
                        clientNameForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                    /* myAvatar form designer truncates the trailing 0 from the below element number (shows "963.2" in the system). It is dumb. 
                     * If you have a field number that ends in a .x where x is any number, ScriptLink will require the trailing 0 or it won't match the field you 
                     * think you're trying to match to and it'll take hours from your life as you go
                     * insane wondering why nothing works and the logic checks out and it should work dammit. ask me how i know \(>_< )/    */
                    case "963.20":
                        clientInfoField = field;
                        clientInfoRow.RowId = row.RowId;
                        clientInfoForm.FormId = inputObject.Forms[0].FormId;
                        clientInfoForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                    // if youre pulling fields that you aren't going to modify, then you can just define as such
                    case "948.28":
                        likeTacos = field.FieldValue; // the string likeTacos is equal to the Field Value of Field 948.28 in the form.
                        break;
                    case "948.29":
                        tacoToppings = field.FieldValue; // same here
                        break;
                    case "948.30": // same trailing 0 truncation in mA. Note: in ScriptLink, 948.30 is not the same as 948.3
                        sourCream = field.FieldValue;
                        break;
                    case "948.32":
                        cheese = field.FieldValue;
                        break;
                    case "963.21":
                        tacoConfigField = field; // as contrasted with above, we are just assigning this variable as a FieldObject "field". We can pull the FieldValue of tacoConfigField elsewhere
                        tacoConfigRow.RowId = row.RowId;
                        tacoConfigForm.FormId = inputObject.Forms[0].FormId;
                        tacoConfigForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                }
            }

            string taco;
            bool isAHorriblePerson = false; // we set this variable as flase initially so we can construct our logic below, if not, we would get an error for using an unassigned variable. mA will overwrite the initially assigned false value with whatever is returned from the form when the script is called

            // string vars need to equal the dictionary code in myAvatar, not the dictionary value, so we have to redefine these in the present scope. 
            if (sourCream == "5")
            {
                isAHorriblePerson = true;
            }
            if (sourCream == "10")
            {
                isAHorriblePerson = false;
            }
            // if a field in a form is empty, it does not rerun as null, it returns as empty in C#, which can be calulated
            if (string.IsNullOrEmpty(sourCream))
            {
                isAHorriblePerson = false;
            }

            if (isAHorriblePerson)
            {
                if (likeTacos == "5")
                {
                    // $ denotes an interpolated string. you can add your variables with curly braces around them 
                    taco = $"Horrible human being {clientNameField.FieldValue} wants one taco with {tacoToppings}, {cheese}, and sour cream.";
                }   
            }
            // !variableName is the shorthand for != or == false for bools
            if (!isAHorriblePerson)
            {
                if (likeTacos == "5")
                {
                    taco = $"Morally upstanding pillar of righteousness, {clientNameField.FieldValue} wants one taco with {tacoToppings}, {cheese}, and NO SOUR CREAM.";
                }
            }

            return returnObject;
        }


        private static OptionObject2015 PullInfo(OptionObject2015 inputObject)
        {
            // invokes CopyObject() method to copy inputObject back to ReturnObject
            OptionObject2015 returnObject = CopyObject(inputObject); 
            RowObject row = inputObject.Forms[0].CurrentRow;

            // same as above. we instantiate Field, Row, and Form Objects for the element we wish to modify with ScriptLink.
            FieldObject textField = new FieldObject();
            RowObject textRow = new RowObject();
            FormObject textForm = new FormObject();

            // this is the ID for the currently selected client
            string clientID = inputObject.EntityID; 

            foreach (FieldObject field in row.Fields)
            {
                switch (field.FieldNumber)
                {
                    case "963.20":
                        textField = field;
                        textRow.RowId = row.RowId;
                        textForm.FormId = inputObject.Forms[0].FormId;
                        textForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                }
            }

            try
            {
                // invokes PullSomething() as defined below on current clientID
                List<string> something = PullSomething(clientID);

                // push each string in List<string> something to textField 
                textField.FieldValue = something[0] + something[1] + something[2] + something[3] + something[4];
                textRow.Fields = new List<FieldObject>() { textField }; 
                textRow.RowAction = "EDIT";
                textForm.CurrentRow = textRow;
                textForm.CurrentRow.ParentRowId = "0";

                returnObject.Forms = new List<FormObject>() { textForm };
            }

            // catch in case something doesn't work right
            catch (ArgumentNullException)
            {
                returnObject.ErrorCode = 1;
                returnObject.ErrorMesg = "whoopsie, something went wrong!";
                return returnObject;
            }

            return returnObject;
        }

        // Method to require feilds in a form. Fields are identified in the foreach() loop by field number.
        private static OptionObject2015 RequireField(OptionObject2015 inputObject)
        {
            // invokes CopyObject() method to copy inputObject back to ReturnObject and RowObject row to identify the data we're modifying as the current data row in the database
            OptionObject2015 returnObject = CopyObject(inputObject);
            RowObject row = inputObject.Forms[0].CurrentRow;

            // instantiating the field, row, and form objects for the Date of Birth Field in the form, as we will make it required via ScriptLink.
            FieldObject dobField = new FieldObject();
            RowObject dobRow = new RowObject();
            FormObject dobForm = new FormObject();

            foreach (FieldObject field in row.Fields)
            {
                switch (field.FieldNumber)
                {
                    // same as above. for any field we want to modify with ScriptLink, we define as such below
                    case "948.26":
                        dobField = field;
                        dobRow.RowId = row.RowId;
                        dobForm.FormId = inputObject.Forms[0].FormId;
                        dobForm.MultipleIteration = inputObject.Forms[0].MultipleIteration;
                        break;
                }
            }

            // this is really all you have to do to require a field. There is no other Method to call or anything else to return but the returnObject
            dobField.Enabled = "1"; // enables the field. 1 == true and 0 == false
            dobField.Required = "1"; // requires. same as above
            // dobField.FieldValue = this is where you can give a field a different value based on another variable if necessary. as we are just making the field required, we will not assign a new value
            dobRow.Fields = new List<FieldObject>() { dobField }; // this will eventually make more sense the more you do it. for now, just follow the pattern.
            dobRow.RowAction = "EDIT";
            dobForm.CurrentRow = dobRow;
            dobForm.CurrentRow.ParentRowId = "0";

            returnObject.Forms = new List<FormObject>() { dobForm };

            return returnObject;
        }

        /* pulls information from patient_current_demographics. queries the database to return a single datarow as a List<string>. 
         * List index [x] correspond to order of returned colum in the query */
        private static List<string> PullSomething(string entity) // 'string entity' here corresponds to variable 'string clientID = inputObject.EntityID' in PullInfo()
        {
            // instantiate new List<string> called 'something' that will contain client info from cache
            var something = new List<string>();

            // connectionString is variable for Database Connection defined in Web.config <connectionStrings> section
            var connectionString = ConfigurationManager.ConnectionStrings["CacheODBC"].ConnectionString;

            // sql query that you will pass
            var commandText = $@"SELECT date_of_birth , patient_name , patient_home_phone , STRING(patient_add_street_1, ', ', patient_add_city, ',' , patient_add_state_code, ' ',patient_add_zipcode) , primary_language_value FROM SYSTEM.patient_current_demographics WHERE PATID = '{entity}'";

            // defines the connection and uses it
            using (var connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // adds column data to the List<string>. Lists in C# begin at index 0
                            something.Add(reader[0].ToString()); // corresponds to date_of_birth
                            something.Add(reader[1].ToString()); // patient_name
                            something.Add(reader[2].ToString()); // patient_home_phone
                            something.Add(reader[3].ToString()); // STRING(address stuff)
                            something.Add(reader[4].ToString()); // language
                        }
                    }
                }
                connection.Close();
                // connection closed 
            }
            // returns List<string> 'something' that we can call in PullInfo() when we invoke PullSomething() method
            return something;
        }
    }
}
