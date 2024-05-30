using System.Text;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessLogic;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.Composition;
using System;
using GME_Custom.GME_EngageServices;
using GME_Custom.GME_EngageServiceHO;
using GME_Custom.GME_EngageFALWSServices;
using GME_Custom.GME_VoucherAIFPROD;
using GME_Custom.GME_LoyaltyAIFPROD;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GME_Custom.GME_Propesties;
using Microsoft.Dynamics.Retail.Pos.BlankOperations;
using System.Xml;

namespace GME_Custom.GME_Data
{
    public class updateCustomer
    {
        private string userCode = "pos";
        private string password = "14-78-40-28-5f-67-9-35-32-28-4f-5a-64-34-60-32-36-1d-62-b";

        private GME_EngageServices.supplInfoDTO setSuppInfo(string code, string value)
        {
            GME_EngageServices.supplInfoDTO supplInfo = new GME_EngageServices.supplInfoDTO();
            supplInfo.code = code;
            supplInfo.value = value;

            return supplInfo;
        }

        public string updatePersonUpdateCustomer(decimal personId, string phoneNumber, string email, string firstName, DateTime birthdate, string gender, string lastName, string skinType)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.personId person = new GME_EngageServices.personId();
            GME_EngageServices.getPersonResponse getPersonRes = new GME_EngageServices.getPersonResponse();
            GME_EngageServices.updatePersonResponse updatePersonRes = new GME_EngageServices.updatePersonResponse();
            GME_EngageServices.updatePersonDTO updatePerson = new GME_EngageServices.updatePersonDTO();
            GME_EngageServices.supplInfoDTO supplInfo = new GME_EngageServices.supplInfoDTO();
            string oldvalue, newvalue;

            List<GME_EngageServices.supplInfoDTO> listsuppInfo = new List<GME_EngageServices.supplInfoDTO>();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                person.userCode = userCode;
                person.password = password;
                person.companyId = 0;
                person.personId1 = personId;
                person.personId1Specified = true;
                getPersonRes.PersonResponse = soap.getPerson(person);
                oldvalue = getPersonRes.PersonResponse.person.firstName;

                updatePerson.companyId = 0;
                updatePerson.userCode = userCode;
                updatePerson.password = password;
                updatePerson.person = getPersonRes.PersonResponse.person;
                updatePerson.person.personId = getPersonRes.PersonResponse.person.personId;
                //updatePerson.person.personIdSpecified = true;
                updatePerson.person.houseHoldId = getPersonRes.PersonResponse.person.houseHoldId;
                //updatePerson.person.houseHoldIdSpecified = true;
                updatePerson.person.language = "EN";
                updatePerson.person.status = getPersonRes.PersonResponse.person.status;
                updatePerson.person.title = getPersonRes.PersonResponse.person.title; ;
                updatePerson.person.titleStr = getPersonRes.PersonResponse.person.titleStr;
                updatePerson.person.type = getPersonRes.PersonResponse.person.type;
                updatePerson.person.identifierList = getPersonRes.PersonResponse.person.identifierList;
                updatePerson.person.addedDate = DateTime.Now;
                //updatePerson.person.addedDateSpecified = true;
                updatePerson.person.cellPhone = phoneNumber;
                updatePerson.person.email = email;
                updatePerson.person.firstName = firstName;
                updatePerson.person.birthDate = birthdate;
                //updatePerson.person.birthDateSpecified = true;
                updatePerson.person.gender = gender;
                updatePerson.person.lastName = lastName;
                newvalue = updatePerson.person.firstName;
                //add
                listsuppInfo.Add(setSuppInfo("INF00087", skinType));

                updatePerson.person.supplementalInfo = new GME_EngageServices.supplInfoDTO[listsuppInfo.Count];

                for (int i = 0; i < listsuppInfo.Count; i++)
                {
                    updatePerson.person.supplementalInfo[i] = listsuppInfo[i];
                }
                updatePerson.person.initDate = DateTime.Now;
                //updatePerson.person.initDateSpecified = true;
                updatePerson.person.validityDate = DateTime.Now;
                //updatePerson.person.validityDateSpecified = true;

                

                System.Threading.Thread.Sleep(2000);
                
                updatePersonRes.UpdatePersonResponse1 = soap.updatePerson(updatePerson);

                success = updatePersonRes.UpdatePersonResponse1.success;
                errCode = updatePersonRes.UpdatePersonResponse1.errorCode;
                errMsg = updatePersonRes.UpdatePersonResponse1.errorMessage;

                writeLog(updatePerson, "Request Update Person");
                writeLog(updatePersonRes, "Response Update Person");

                if (success)
                {
                    result = "Success";
                    BlankOperations.ShowMsgBoxInformation(oldvalue + " To " + newvalue);
                    

                    GME_Var.UpdateCustHouseholdId = getPersonRes.PersonResponse.person.houseHoldId;
                }
                else
                {
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                return error.ToString();
            }

            return result;
        }

        private void writeLog(object objMessage, string type)
        {
            string date = DateTime.Now.Date.ToString("yyyyMMdd");
            string FileName = "EngageLog" + "_" + date;
            string directoryPath = @"D:\EngLog\";
            string path = directoryPath + FileName + ".txt";

            string message = GetXMLFromObject(objMessage);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Engage Log Date : " + System.DateTime.Now + Environment.NewLine);
                    sw.WriteLine("type : " + type + Environment.NewLine);
                    sw.WriteLine(message);

                }
            }
            else if (File.Exists(path))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("Engage Log Date : " + System.DateTime.Now);
                    sw.WriteLine("type : " + type + Environment.NewLine);
                    sw.WriteLine(message + Environment.NewLine);
                }
            }
        }

        private static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }
    }
}
