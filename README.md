# TacoScript

### How to use this guide ✔✔

<p>This guide makes some assumptions about where you are in your development journey.</p>
- You have a ScriptLink server already<br>
- Your server is configured to connect to myAvatar for your org<br>
- You have permissions to do all of this<br>
- You have a base-level familiarity with GitHub<br>
<br>
<p>If all of the above prerequisites are met, then we can carry on with the tutorial</p>

#### Housekeeping and prerequisite software

You will need the following software to follow along with this tutorial:<br>
GitHub Desktop (https://github.com/apps/desktop)... *or some other way to interact with git*<br>
Microsoft Visual Studio (**not VSCode**) (https://visualstudio.microsoft.com/downloads/)<br>

#### Getting Started 🤞

**First**, clone/open with GitHub Desktop(or download as zip, but please, _please_ learn to use git at some point) the TacoScript Repo.<br>
![image](https://github.com/user-attachments/assets/3e2aeba6-d925-4d0f-bd57-a09221cd716b)<br>

This file will contain the myAvatar form envelope, the ScriptLink solution, and all accompanying class libraries you wil need<br>

**Next**, import the envelope into your Build environment, then export the envelope to your UAT environment.
This will provide you with the needed element numbers for the script. You will have to change the element numbers to match what yours are in your system. 

So that the element numbers here...<br>
![image](https://github.com/user-attachments/assets/1d1dc115-df7b-4f09-9e12-8891bea1eebc)

.. match up with the element numbers in the script<br>
![image](https://github.com/user-attachments/assets/3470ab22-ae06-45ad-ba7f-0a578e77afc2)

**At this point** you will open the file TacoScript.sln in the script directory<br>
![image](https://github.com/user-attachments/assets/7ed3595b-bdc6-4cd7-bbb0-73a563546a12)

It will open up the solution containing the script. From there, you will edit the field.FieldNumber in **every** switch loop that you find so they will match the numbers in Form and Table Documentation in your system.<br>
<br>
The variables in the ScriptLink solution that I wrote don't always line up with the names in Form and Table Documentation, but I tried to keep it fairly consistent. <br>

**After reassigning the element numbers**, you will click on Build and click Build Solution from the dropdown menu in Visual Studio.<br>
![image](https://github.com/user-attachments/assets/14d9cebc-1fc3-447e-bf32-e870d0401a7a)

This will build the solution into a production application, which can then be placed in the application pool of your IIS Manager. As this is likely a new script for you, you will have to create the directory yourself.<br>
<br>
I have made the directory TacoScript in my LocalDisk(C:)/inetpub/UAT/ICWebservices/TacoScript, as the parent folders are for our UAT ScriptLink solutions per our configuration in IIS Manager<br>
![image](https://github.com/user-attachments/assets/dc3e623a-2a0b-42da-8296-1c4f43134120)

**Copy the contents of the solution directory into the directory in your IIS Manager pool**
Select all the items in the local GitHub repo (you can exlucde the .git directory, as this is data for Git and will only serve to slow down the copy/paste process), then paste into the directory for your IIS Manager's TacoScript application.<br>
![image](https://github.com/user-attachments/assets/aa00eb50-71b7-4765-bc38-1841989c9c18)

From there, go to IIS Manager, right click on the solution directory, and click "Convert to Application". Mine is already an application, hence the "Manage Application" menu item. If it is not already converted to an application, the option to do so should be there.<br>
![image](https://github.com/user-attachments/assets/8cfbe23f-64ed-4cb8-bae0-c3867563d959)

**This next part used to be easier**, but something happened along the way to obfuscate the process. 
To get the WSDL for your api, in IIS Manager, click on your application to bring up this screen: 

Click on the Browse your.server.address:port (http) link<br>
![image](https://github.com/user-attachments/assets/a28ca746-f787-48ac-a9ba-d85efa59131f)

From there, a browser window should pop up, likely with an HHTP error.<br>
![image](https://github.com/user-attachments/assets/3bb545ad-7d4e-40b0-be40-38ccfa215040)

You will have to complete the path to the .asmx *as it exists in your directory structure*.<br>
<br>
**On my server**, the path to the file is C:\inetpub\UAT\ICWebservices\TacoScript\api\TacoScript.asmx<br>
![image](https://github.com/user-attachments/assets/d1ce9507-3dab-43d7-9db5-68c2e49ada55)

**Back in the browser window**, copy/paste/type/whatever the remaining directory path to the .asmx file, **and append "?WSDL"** to the end of the path.<br>
<br>
In this instance, I only had to add "/api/TacoScript/TacoScript.asmx?WSDL" to the address.<br>
![image](https://github.com/user-attachments/assets/59fda6f1-01c5-4bb6-a7f0-06aa903ca040)

Going to that address should show a screen that looks like this: <br>
![image](https://github.com/user-attachments/assets/150bc5df-9990-4249-8735-81b27c9d4135)

If you got a screen like this, then congratulations! Your script is running on the server.<br>
<br>
From there, copy the address in the browser. This is the WSDL address (e.g. 10.XXX.XX.XX/ICWebservices/TacoScript/api/TacoScript.asmx?WSDL).<br>
<br>
To make the address importable into myAvatar, we add "https://" to the front of the WSDL address, so my final importable WSDL ends up as ``https://10.XXX.XX.XX/ICWebservices/TacoScript/api/TacoScript.asmx?WSDL``.<br>
<br>

#### Deploying the script in myAvatar

I like to push the WSDL to the README in git, so it can be found easily. <br>
Once you have the WSDL, go to Form Designer for the Testvelope III Return of the Test form (it's dumb. I am sorry).<br>
<br>
Click on the Edit link in the ScriptLink section on the left side of Form Designer.<br>
![image](https://github.com/user-attachments/assets/6b490a74-7085-4b28-ba67-66b579e97ade)

Then, import the WSDL in the window that opens, ensuring that it is https!<br>
![image](https://github.com/user-attachments/assets/8d2e264f-eed7-469d-ab9d-9a3dfaed5f09)

Recall the parameters in the RunScript() method here: <br>
![image](https://github.com/user-attachments/assets/c3ff8cb9-3a3f-4198-b87e-928a6dbd1e15)

These are the parameters that you will enter in Form Designer. Since we want the Date of Birth field to be required when the form loads, then we will select TacoScript from the Available Scripts dropdown in Form Load, and then type "require" in the script parameter text field as such: <br>
![image](https://github.com/user-attachments/assets/545270ed-8799-41cc-a976-c707bb6722ab)
Since the field was not set as required in Form Definition, this will make it required on form load.<br>

We will set "pullinfo" on the Do Something button in the form. When clicked, this will call the PullInfo() method in the script, pulling client name, date of birth, and other details into the Client Information textbox below.<br>
To do this, retun to the main form view in Form Designer, and then click on the Do Something button. In the left column, you can see a ScriptLink setting for that field.<br>
![image](https://github.com/user-attachments/assets/22ec5bb8-f024-4c66-8488-099737400b9e)

Click on the link in that field to bring up the following window. Select TacoScript from the dropdown, and enter "pullinfo" into the textbox. Remember, you call the scripts with the text you pass as the parameter in the case statement in RunScript(), not necessarily the methods that perform the action themselves.<br>
![image](https://github.com/user-attachments/assets/0dc1e53d-a649-47d6-8337-7d991178490b)

We will do the same on the Configure Taco button.<br>
![image](https://github.com/user-attachments/assets/a5a4a580-0384-4d70-a881-0338e6ab5f1f)

When clicked, it will pull the value of the vairable we defined in the script to the field in question.









