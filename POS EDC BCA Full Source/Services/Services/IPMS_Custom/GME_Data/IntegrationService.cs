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
    public class IntegrationService
    {
        private string userCode = "pos";
        private string password = "14-78-40-28-5f-67-9-35-32-28-4f-5a-64-34-60-32-36-1d-62-b";

        public string createIdentifierGuest(short bindId, string identifierCode)
        {
            GME_EngageServiceHO.AccountServicesClient soap = new GME_EngageServiceHO.AccountServicesClient();
            GME_EngageServiceHO.createIdentifierId createIdentifier = new GME_EngageServiceHO.createIdentifierId();
            GME_EngageServiceHO.createIdentifierResponse createResponse = new GME_EngageServiceHO.createIdentifierResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                createIdentifier.companyId = 0;
                createIdentifier.userCode = userCode;
                createIdentifier.password = password;
                createIdentifier.binId = bindId;
                createIdentifier.identifierCode = identifierCode;
                createIdentifier.persontypeId = 0;

                createResponse.CreateIdentifierResponse1 = soap.createIdentifier(createIdentifier);
                success = createResponse.CreateIdentifierResponse1.success;
                errMsg = createResponse.CreateIdentifierResponse1.errorMessage;
                errCode = createResponse.CreateIdentifierResponse1.errorCode;

                writeLog(createIdentifier, "Request Create Identifier");
                writeLog(createResponse, "Response Create Identifier");

                if (success)
                {
                    result = "Success";

                    if (bindId == 5)
                    {
                        GME_Var.publicIdentifier = createResponse.CreateIdentifierResponse1.identifierCode;
                        GME_Var.publicPersonId = createResponse.CreateIdentifierResponse1.personId;
                    }

                    if (bindId == 10)
                    {
                        GME_Var.enrollPersonId = createResponse.CreateIdentifierResponse1.personId;
                        GME_Var.enrollIdentiferId = createResponse.CreateIdentifierResponse1.identifierCode;
                    }
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

        public string createIdentifier(short bindId, string identifierCode)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.createIdentifierId createIdentifier = new GME_EngageServices.createIdentifierId();
            GME_EngageServices.createIdentifierResponse createResponse = new GME_EngageServices.createIdentifierResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                createIdentifier.companyId = 0;
                createIdentifier.userCode = userCode;
                createIdentifier.password = password;
                createIdentifier.binId = bindId;
                createIdentifier.identifierCode = identifierCode;
                createIdentifier.persontypeId = 0;

                createResponse.CreateIdentifierResponse1 = soap.createIdentifier(createIdentifier);
                success = createResponse.CreateIdentifierResponse1.success;
                errMsg = createResponse.CreateIdentifierResponse1.errorMessage;
                errCode = createResponse.CreateIdentifierResponse1.errorCode;

                writeLog(createIdentifier, "Request Create Identifier");
                writeLog(createResponse, "Response Create Identifier");

                if (success)
                {
                    result = "Success";

                    if (bindId == 5)
                    {
                        GME_Var.publicIdentifier = createResponse.CreateIdentifierResponse1.identifierCode;
                        GME_Var.publicPersonId = createResponse.CreateIdentifierResponse1.personId;
                    }

                    if (bindId == 10)
                    {
                        GME_Var.enrollPersonId = createResponse.CreateIdentifierResponse1.personId;
                        GME_Var.enrollIdentiferId = createResponse.CreateIdentifierResponse1.identifierCode;
                    }
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

        public string updateIdentifierOldCard(string identifierNumberOld, string identifierNumberNew)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.identifierId identifierReq = new GME_EngageServices.identifierId();                        
            GME_EngageServices.updateIdentifierResponse responseOld = new GME_EngageServices.updateIdentifierResponse();
            GME_EngageServices.updateIdentifierDTO updtIdentifierDTOOld = new GME_EngageServices.updateIdentifierDTO();
            GME_EngageServices.updateIdentifierDTO updtIdentifierDTONew = new GME_EngageServices.updateIdentifierDTO();
            GME_EngageServices.getIdentifierRequest identifierRequest = new GME_EngageServices.getIdentifierRequest();
            GME_EngageServices.getIdentifierResponse identifierResponse = new GME_EngageServices.getIdentifierResponse();
            GME_EngageServices.updateIdentifierResponse responseNew = new GME_EngageServices.updateIdentifierResponse();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;
            DateTime createDate = DateTime.MinValue;

            try
            {
                identifierReq.companyId = 0;
                identifierReq.userCode = userCode;
                identifierReq.password = password;
                identifierReq.identifier = identifierNumberOld;

                identifierRequest.identifierId = identifierReq;                

                identifierResponse.IdentifierResponse = soap.getIdentifier(identifierRequest.identifierId);

                errMsg = identifierResponse.IdentifierResponse.errorMessage;
                errCode = identifierResponse.IdentifierResponse.errorCode;
                createDate = identifierResponse.IdentifierResponse.identifier.createDate;

                updtIdentifierDTONew.identifier = identifierResponse.IdentifierResponse.identifier;
                updtIdentifierDTONew.identifier.identifierCode = identifierNumberNew;
                updtIdentifierDTONew.identifier.status = 4;
                updtIdentifierDTONew.identifier.cardType = identifierResponse.IdentifierResponse.identifier.cardType;
                updtIdentifierDTONew.identifier.personId = identifierResponse.IdentifierResponse.identifier.personId;
                updtIdentifierDTONew.identifier.createDate = createDate;
                //updtIdentifierDTONew.identifier.createDateSpecified = true;
                updtIdentifierDTONew.identifier.allowBurn = true;
                updtIdentifierDTONew.identifier.allowEarn = true;
                updtIdentifierDTONew.companyId = 0;
                updtIdentifierDTONew.userCode = userCode;
                updtIdentifierDTONew.password = password;
                responseNew.UpdateIdentifierResponse1 = soap.updateIdentifier(updtIdentifierDTONew);

                success = responseNew.UpdateIdentifierResponse1.success;
                errMsg = responseNew.UpdateIdentifierResponse1.errorMessage;
                errCode = responseNew.UpdateIdentifierResponse1.errorCode;

                writeLog(updtIdentifierDTONew, "Request Update Identifier New");
                writeLog(responseNew, "Response Update Identifier New");

                updtIdentifierDTOOld.identifier = identifierResponse.IdentifierResponse.identifier;
                updtIdentifierDTOOld.identifier.identifierCode = identifierNumberOld;
                updtIdentifierDTOOld.identifier.status = 6;
                updtIdentifierDTOOld.identifier.cardType = identifierResponse.IdentifierResponse.identifier.cardType;
                updtIdentifierDTOOld.identifier.createDate = identifierResponse.IdentifierResponse.identifier.createDate;
                //updtIdentifierDTOOld.identifier.createDateSpecified = true;
                updtIdentifierDTOOld.identifier.allowBurn = true;
                updtIdentifierDTOOld.identifier.allowEarn = true;
                updtIdentifierDTOOld.companyId = 0;
                updtIdentifierDTOOld.userCode = userCode;
                updtIdentifierDTOOld.password = password;
                responseOld.UpdateIdentifierResponse1 = soap.updateIdentifier(updtIdentifierDTOOld);

                success = responseOld.UpdateIdentifierResponse1.success;
                errMsg = responseOld.UpdateIdentifierResponse1.errorMessage;
                errCode = responseOld.UpdateIdentifierResponse1.errorCode;

                GME_Var.cardReplacePersonIdOld = identifierResponse.IdentifierResponse.identifier.personId;
                GME_Var.cardReplaceCardTypeOld = identifierResponse.IdentifierResponse.identifier.cardType;
                GME_Var.cardReplaceCreateDateOld = identifierResponse.IdentifierResponse.identifier.createDate;                

                writeLog(updtIdentifierDTOOld, "Request Update Identifier Old");
                writeLog(responseOld, "Response Update Identifier Old");

                if (success)
                {
                    result = "Success";
                    GME_Var.personId = identifierResponse.IdentifierResponse.identifier.personId;
                }
                else
                {
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                if (errCode == 12)
                    BlankOperations.ShowMsgBox("Nomor kartu member yang anda masukkan salah, silahkan masukkan nomor yang benar");
                else
                    BlankOperations.ShowMsgBox(errMsg);

                return error.ToString();
            }

            return result;
        }

        public string updateIdentifierNewCard(string identifierNumberNew, string identifierNumberOld)
        {
            GME_EngageServiceHO.AccountServicesClient soap = new GME_EngageServiceHO.AccountServicesClient();
            GME_EngageServiceHO.identifierId identifierReq = new GME_EngageServiceHO.identifierId();
            GME_EngageServiceHO.identifierId identifierReqOld = new GME_EngageServiceHO.identifierId();
            GME_EngageServiceHO.getIdentifierResponse identifierRes = new GME_EngageServiceHO.getIdentifierResponse();
            GME_EngageServiceHO.getIdentifierResponse identifierResOld = new GME_EngageServiceHO.getIdentifierResponse();
            GME_EngageServiceHO.updateIdentifierResponse response = new GME_EngageServiceHO.updateIdentifierResponse();
            GME_EngageServiceHO.updateIdentifierDTO updtIdentifierDTO = new GME_EngageServiceHO.updateIdentifierDTO();
            GME_EngageServiceHO.updateIdentifierRequest request = new GME_EngageServiceHO.updateIdentifierRequest();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {                           
                identifierReq.companyId = 0;
                identifierReq.userCode = userCode;
                identifierReq.password = password;
                identifierReq.identifier = identifierNumberOld;
                identifierRes.IdentifierResponse = soap.getIdentifier(identifierReq);

                GME_Var.cardReplaceCreateDateOld = identifierRes.IdentifierResponse.identifier.createDate;

                updtIdentifierDTO.companyId = 0;
                updtIdentifierDTO.userCode = userCode;
                updtIdentifierDTO.password = password;
                updtIdentifierDTO.identifier = identifierRes.IdentifierResponse.identifier;
                updtIdentifierDTO.identifier.identifierCode = identifierNumberNew;
                updtIdentifierDTO.identifier.status = 4;
                updtIdentifierDTO.identifier.cardType = identifierRes.IdentifierResponse.identifier.cardType;
                updtIdentifierDTO.identifier.createDate = identifierRes.IdentifierResponse.identifier.createDate;
                updtIdentifierDTO.identifier.allowBurn = true;
                updtIdentifierDTO.identifier.allowEarn = true;
                updtIdentifierDTO.identifier.personId = identifierRes.IdentifierResponse.identifier.personId;
                updtIdentifierDTO.identifier.typeOfIdentifier = 1;                
                response.UpdateIdentifierResponse1 = soap.updateIdentifier(updtIdentifierDTO);

                success = response.UpdateIdentifierResponse1.success;
                errMsg = response.UpdateIdentifierResponse1.errorMessage;
                errCode = response.UpdateIdentifierResponse1.errorCode;

                writeLog(updtIdentifierDTO, "Request Update Identifier");
                writeLog(response, "Response Update Identifier");

                if (success)
                {
                    result = "Success";
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

        public string updateIdentifierOldCard(string identifierNumberOld)
        {
            GME_EngageServiceHO.AccountServicesClient soap = new GME_EngageServiceHO.AccountServicesClient();
            GME_EngageServiceHO.identifierId identifierReq = new GME_EngageServiceHO.identifierId();
            GME_EngageServiceHO.identifierId identifierReqOld = new GME_EngageServiceHO.identifierId();
            GME_EngageServiceHO.getIdentifierResponse identifierRes = new GME_EngageServiceHO.getIdentifierResponse();
            GME_EngageServiceHO.getIdentifierResponse identifierResOld = new GME_EngageServiceHO.getIdentifierResponse();
            GME_EngageServiceHO.updateIdentifierResponse response = new GME_EngageServiceHO.updateIdentifierResponse();
            GME_EngageServiceHO.updateIdentifierDTO updtIdentifierDTO = new GME_EngageServiceHO.updateIdentifierDTO();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {                
                identifierReq.companyId = 0;
                identifierReq.userCode = userCode;
                identifierReq.password = password;
                identifierReq.identifier = identifierNumberOld;
                identifierRes.IdentifierResponse = soap.getIdentifier(identifierReq);

                updtIdentifierDTO.identifier = identifierRes.IdentifierResponse.identifier;
                updtIdentifierDTO.identifier.identifierCode = identifierNumberOld;
                updtIdentifierDTO.identifier.status = 6;
                updtIdentifierDTO.identifier.cardType = identifierRes.IdentifierResponse.identifier.cardType;
                updtIdentifierDTO.identifier.createDate = identifierRes.IdentifierResponse.identifier.createDate;
                updtIdentifierDTO.identifier.allowBurn = true;
                updtIdentifierDTO.identifier.allowEarn = true;                
                updtIdentifierDTO.identifier.typeOfIdentifier = 1;
                updtIdentifierDTO.companyId = 0;
                updtIdentifierDTO.userCode = userCode;
                updtIdentifierDTO.password = password;
                response.UpdateIdentifierResponse1 = soap.updateIdentifier(updtIdentifierDTO);

                success = response.UpdateIdentifierResponse1.success;
                errMsg = response.UpdateIdentifierResponse1.errorMessage;
                errCode = response.UpdateIdentifierResponse1.errorCode;

                writeLog(updtIdentifierDTO, "Request Update Identifier");
                writeLog(response, "Response Update Identifier");

                if (success)
                {
                    result = "Success";
                    GME_Var.personId = identifierRes.IdentifierResponse.identifier.personId;
                }
                else
                {
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                if (errCode == 12)
                    BlankOperations.ShowMsgBox("Nomor kartu member yang anda masukkan salah, silahkan masukkan nomor yang benar");
                else
                    BlankOperations.ShowMsgBox(errMsg);

                return error.ToString();
            }

            return result;
        }

        public string updateIdentifier(string identifierNumber, short cardType)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.identifierId identifierReq = new GME_EngageServices.identifierId();
            GME_EngageServices.getIdentifierResponse identifierRes = new GME_EngageServices.getIdentifierResponse();
            GME_EngageServices.updateIdentifierResponse response = new GME_EngageServices.updateIdentifierResponse();
            GME_EngageServices.updateIdentifierDTO updtIdentifierDTO = new GME_EngageServices.updateIdentifierDTO();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                identifierReq.companyId = 0;
                identifierReq.userCode = userCode;
                identifierReq.password = password;
                identifierReq.identifier = identifierNumber;                
                identifierRes.IdentifierResponse = soap.getIdentifier(identifierReq);

                updtIdentifierDTO.identifier = identifierRes.IdentifierResponse.identifier;
                updtIdentifierDTO.identifier.identifierCode = identifierNumber;
                updtIdentifierDTO.identifier.status = 4;
                updtIdentifierDTO.identifier.cardType = cardType;
                updtIdentifierDTO.identifier.createDate = DateTime.Today;
                updtIdentifierDTO.identifier.allowBurn = true;
                updtIdentifierDTO.identifier.allowEarn = true;
                updtIdentifierDTO.companyId = 0;
                updtIdentifierDTO.userCode = userCode;
                updtIdentifierDTO.password = password;
                response.UpdateIdentifierResponse1 = soap.updateIdentifier(updtIdentifierDTO);

                success = response.UpdateIdentifierResponse1.success;
                errMsg = response.UpdateIdentifierResponse1.errorMessage;
                errCode = response.UpdateIdentifierResponse1.errorCode;

                writeLog(updtIdentifierDTO, "Request Update Identifier");
                writeLog(response, "Response Update Identifier");

                if (success)
                {
                    result = "Success";
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

        public string updatePersonPublic(decimal personId, string phoneNumber, string email, string firstName, string lastName)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.personId person = new GME_EngageServices.personId();
            GME_EngageServices.getPersonRequest getPersonReq = new GME_EngageServices.getPersonRequest();
            GME_EngageServices.getPersonResponse getPersonRes = new GME_EngageServices.getPersonResponse();
            GME_EngageServices.updatePersonResponse updatePersonRes = new GME_EngageServices.updatePersonResponse();
            GME_EngageServices.updatePersonDTO updatePerson = new GME_EngageServices.updatePersonDTO();
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

                updatePerson.companyId = 0;
                updatePerson.userCode = userCode;
                updatePerson.password = password;
                updatePerson.person = getPersonRes.PersonResponse.person;

                updatePerson.person.addedDate = DateTime.Now;
                updatePerson.person.cellPhone = phoneNumber;
                updatePerson.person.email = email;
                updatePerson.person.firstName = firstName;
                updatePerson.person.lastName = lastName;
                updatePerson.person.initDate = DateTime.Now;
                updatePerson.person.validityDate = DateTime.Now;

                updatePersonRes.UpdatePersonResponse1 = soap.updatePerson(updatePerson);

                success = updatePersonRes.UpdatePersonResponse1.success;
                errCode = updatePersonRes.UpdatePersonResponse1.errorCode;
                errMsg = updatePersonRes.UpdatePersonResponse1.errorMessage;

                writeLog(updatePerson, "Request Update Person");
                writeLog(updatePersonRes, "Response Update Person");

                if (success)
                {
                    result = "Success";

                    GME_Var.publicHouseholdId = getPersonRes.PersonResponse.person.houseHoldId;
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

        private GME_EngageServices.supplInfoDTO setSuppInfo(string code, string value)
        {
            GME_EngageServices.supplInfoDTO supplInfo = new GME_EngageServices.supplInfoDTO();
            supplInfo.code = code;
            supplInfo.value = value;

            return supplInfo;
        }

        private GME_EngageServiceHO.supplInfoDTO setSuppInfoHO(string code, string value)
        {
            GME_EngageServiceHO.supplInfoDTO supplInfo = new GME_EngageServiceHO.supplInfoDTO();
            supplInfo.code = code;
            supplInfo.value = value;

            return supplInfo;
        }

        public string updatePersonEnrollment(decimal personId, string phoneNumber, string email, string firstName, DateTime birthdate, string gender, string lastName, string skinType)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.personId person = new GME_EngageServices.personId();
            GME_EngageServices.getPersonResponse getPersonRes = new GME_EngageServices.getPersonResponse();
            GME_EngageServices.updatePersonResponse updatePersonRes = new GME_EngageServices.updatePersonResponse();
            GME_EngageServices.updatePersonDTO updatePerson = new GME_EngageServices.updatePersonDTO();
            //GME_EngageServices.supplInfoDTO supplInfo = new GME_EngageServices.supplInfoDTO();

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

                updatePerson.companyId = 0;
                updatePerson.userCode = userCode;
                updatePerson.password = password;
                updatePerson.person = getPersonRes.PersonResponse.person;

                updatePerson.person.addedDate = DateTime.Now;
                updatePerson.person.birthDate = birthdate;
                updatePerson.person.birthDateSpecified = true;                
                updatePerson.person.cellPhone = phoneNumber;
                updatePerson.person.email = email;
                updatePerson.person.firstName = firstName;
                updatePerson.person.gender = gender;
                updatePerson.person.initDate = DateTime.Now;
                updatePerson.person.lastName = lastName;
                if (gender == "M")
                {
                    updatePerson.person.title = 1;
                    updatePerson.person.titleStr = "MR.";
                }
                if (gender == "F")
                {
                    updatePerson.person.title = 2;
                    updatePerson.person.titleStr = "MRS.";
                }

                //add
                listsuppInfo.Add(setSuppInfo("INF00087", skinType));

                updatePerson.person.supplementalInfo = new GME_EngageServices.supplInfoDTO[listsuppInfo.Count];

                for (int i = 0; i < listsuppInfo.Count; i++)
                {
                    updatePerson.person.supplementalInfo[i] = listsuppInfo[i];
                }


                updatePerson.person.validityDate = DateTime.Now;
                updatePersonRes.UpdatePersonResponse1 = soap.updatePerson(updatePerson);

                success = updatePersonRes.UpdatePersonResponse1.success;
                errCode = updatePersonRes.UpdatePersonResponse1.errorCode;
                errMsg = updatePersonRes.UpdatePersonResponse1.errorMessage;

                writeLog(updatePerson, "Request Update Person");
                writeLog(updatePersonRes, "Response Update Person");

                if (success)
                {
                    result = "Success";

                    GME_Var.enrollHouseholdId = getPersonRes.PersonResponse.person.houseHoldId;
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

        public string updatePersonUpdateCustomer(decimal personId, string phoneNumber, string email, string firstName, DateTime birthdate, string gender, string lastName, string skinType)
        {
            GME_EngageServiceHO.AccountServicesClient soap = new GME_EngageServiceHO.AccountServicesClient();
            GME_EngageServiceHO.personId person = new GME_EngageServiceHO.personId();
            GME_EngageServiceHO.getPersonResponse getPersonRes = new GME_EngageServiceHO.getPersonResponse();
            GME_EngageServiceHO.updatePersonResponse updatePersonRes = new GME_EngageServiceHO.updatePersonResponse();
            GME_EngageServiceHO.updatePersonDTO updatePerson = new GME_EngageServiceHO.updatePersonDTO();
            GME_EngageServiceHO.supplInfoDTO supplInfo = new GME_EngageServiceHO.supplInfoDTO();            

            List<GME_EngageServiceHO.supplInfoDTO> listsuppInfo = new List<GME_EngageServiceHO.supplInfoDTO>();

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
                
               
                updatePerson.companyId = 0;
                updatePerson.userCode = userCode;
                updatePerson.password = password;
                updatePerson.person = getPersonRes.PersonResponse.person;
                updatePerson.person.personId = getPersonRes.PersonResponse.person.personId;
                updatePerson.person.personIdSpecified = true;
                updatePerson.person.houseHoldId = getPersonRes.PersonResponse.person.houseHoldId;
                updatePerson.person.houseHoldIdSpecified = true;                                
                updatePerson.person.language = "EN";
                updatePerson.person.status = getPersonRes.PersonResponse.person.status;
                updatePerson.person.title = getPersonRes.PersonResponse.person.title; ;
                updatePerson.person.titleStr = getPersonRes.PersonResponse.person.titleStr;
                updatePerson.person.type = getPersonRes.PersonResponse.person.type;                                
                updatePerson.person.identifierList = getPersonRes.PersonResponse.person.identifierList;
                updatePerson.person.addedDate = DateTime.Now;
                updatePerson.person.addedDateSpecified = true;
                updatePerson.person.cellPhone = phoneNumber;
                updatePerson.person.email = email;                
                updatePerson.person.firstName = firstName;
                updatePerson.person.birthDate = birthdate;
                updatePerson.person.birthDateSpecified = true;
                updatePerson.person.gender = gender;
                updatePerson.person.lastName = lastName;
                                
                if (gender == "M")
                {
                    updatePerson.person.title = 1;
                    updatePerson.person.titleStr = "MR.";
                }
                if (gender == "F")
                {
                    updatePerson.person.title = 2;
                    updatePerson.person.titleStr = "MRS.";
                }

                //add
                listsuppInfo.Add(setSuppInfoHO("INF00087", skinType));

                updatePerson.person.supplementalInfo = new GME_EngageServiceHO.supplInfoDTO[listsuppInfo.Count];

                for (int i = 0; i < listsuppInfo.Count; i++)
                {
                    updatePerson.person.supplementalInfo[i] = listsuppInfo[i];
                }
                updatePerson.person.initDate = DateTime.Now;
                updatePerson.person.initDateSpecified = true;
                updatePerson.person.validityDate = DateTime.Now;
                updatePerson.person.validityDateSpecified = true;

                updatePersonRes.UpdatePersonResponse1 = soap.updatePerson(updatePerson);

                success = updatePersonRes.UpdatePersonResponse1.success;
                errCode = updatePersonRes.UpdatePersonResponse1.errorCode;
                errMsg = updatePersonRes.UpdatePersonResponse1.errorMessage;

                writeLog(updatePerson, "Request Update Person");
                writeLog(updatePersonRes, "Response Update Person");

                if (success)
                {
                    result = "Success" ;

                    GME_Var.updateMemberResp = "UPDATED";
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

        public string updateContactabilitySMS(decimal personId)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.updateContactabilityDTO contactDTO = new GME_EngageServices.updateContactabilityDTO();
            GME_EngageServices.commTypeDTO commTypeDTO = new GME_EngageServices.commTypeDTO();
            GME_EngageServices.commOption commOption = new GME_EngageServices.commOption();
            GME_EngageServices.contactabilityDTO contactability = new GME_EngageServices.contactabilityDTO();
            GME_EngageServices.updateContactabilityResponse contactResponse = new GME_EngageServices.updateContactabilityResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                string[] arrcommOptionDescription = { "At Checkout", "Reminders", "SMS" };

                List<GME_EngageServices.commOption> listcommOption = new List<GME_EngageServices.commOption>();

                for (int k = 0; k < arrcommOptionDescription.Length; k++)
                {
                    listcommOption.Add(setcommOption(arrcommOptionDescription[k], k));
                }


                commTypeDTO.commOptions = new GME_EngageServices.commOption[listcommOption.Count];

                for (int k = 0; k < listcommOption.Count; k++)
                {
                    commTypeDTO.commOptions[k] = listcommOption[k];
                }
                commTypeDTO.commTypeDescription = "Opt in for SMS/TextMessage";
                commTypeDTO.communicationType = 1;
                commTypeDTO.subscription = true;

                contactability.commTypes = new GME_EngageServices.commTypeDTO[1];
                contactability.commTypes[0] = commTypeDTO;
                contactability.personId = personId;
                contactability.personIdSpecified = true;

                contactDTO.companyId = 0;
                contactDTO.userCode = userCode;
                contactDTO.password = password;
                contactDTO.contactability = contactability;

                contactResponse.UpdateContactabilityResponse1 = soap.updateContactability(contactDTO);

                success = contactResponse.UpdateContactabilityResponse1.success;
                errCode = contactResponse.UpdateContactabilityResponse1.errorCode;
                errMsg = contactResponse.UpdateContactabilityResponse1.errorMessage;

                writeLog(contactDTO, "Request Update Contactability By Phone");
                writeLog(contactResponse, "Response Update Contactability By Phone");

                if (success)
                {
                    result = "Success";
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

        public string updateContactabilityEmail(decimal personId)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.updateContactabilityDTO contactDTO = new GME_EngageServices.updateContactabilityDTO();
            GME_EngageServices.commTypeDTO commTypeDTO = new GME_EngageServices.commTypeDTO();
            GME_EngageServices.commOption commOption = new GME_EngageServices.commOption();
            GME_EngageServices.contactabilityDTO contactability = new GME_EngageServices.contactabilityDTO();
            GME_EngageServices.updateContactabilityResponse contactResponse = new GME_EngageServices.updateContactabilityResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                string[] arrcommOptionDescription = { "At Checkout", "Reminders", "Mailing" };

                List<GME_EngageServices.commOption> listcommOption = new List<GME_EngageServices.commOption>();

                for (int k = 0; k < arrcommOptionDescription.Length; k++)
                {
                    listcommOption.Add(setcommOption(arrcommOptionDescription[k], k));
                }


                commTypeDTO.commOptions = new GME_EngageServices.commOption[listcommOption.Count];

                for (int k = 0; k < listcommOption.Count; k++)
                {
                    commTypeDTO.commOptions[k] = listcommOption[k];
                }
                commTypeDTO.commTypeDescription = "Opt in for Email Message";
                commTypeDTO.communicationType = 0;
                commTypeDTO.subscription = true;

                contactability.commTypes = new GME_EngageServices.commTypeDTO[1];
                contactability.commTypes[0] = commTypeDTO;
                contactability.personId = personId;
                contactability.personIdSpecified = true;

                contactDTO.companyId = 0;
                contactDTO.userCode = userCode;
                contactDTO.password = password;
                contactDTO.contactability = contactability;

                contactResponse.UpdateContactabilityResponse1 = soap.updateContactability(contactDTO);

                success = contactResponse.UpdateContactabilityResponse1.success;
                errCode = contactResponse.UpdateContactabilityResponse1.errorCode;
                errMsg = contactResponse.UpdateContactabilityResponse1.errorMessage;

                writeLog(contactDTO, "Request Update Contactability By Email");
                writeLog(contactResponse, "Response Update Contactability By Email");

                if (success)
                {
                    result = "Success";
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

        public string deleteIdentifier(string identifier)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.identifierId identifierId = new GME_EngageServices.identifierId();
            GME_EngageServices.deleteIdentifierResponse deleteResponse = new GME_EngageServices.deleteIdentifierResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                identifierId.companyId = 0;
                identifierId.userCode = userCode;
                identifierId.password = password;
                identifierId.identifier = identifier;

                deleteResponse.DeleteIdentifierResponse1 = soap.deleteIdentifier(identifierId);

                success = deleteResponse.DeleteIdentifierResponse1.success;
                errCode = deleteResponse.DeleteIdentifierResponse1.errorCode;
                errMsg = deleteResponse.DeleteIdentifierResponse1.errorMessage;

                writeLog(identifierId, "Request Delete Identifier");
                writeLog(deleteResponse, "Response Delete Identifier");

                if (success)
                {
                    result = "Delete identifier success";
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

        public string findHouseHoldsByInfo(string infoCode, string infoValue)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.findByInfoId findByInfoId = new GME_EngageServices.findByInfoId();
            GME_EngageServices.findHouseholdsByInfoResponse findResponse = new GME_EngageServices.findHouseholdsByInfoResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                findByInfoId.companyId = 0;
                findByInfoId.userCode = userCode;
                findByInfoId.password = password;
                findByInfoId.infoCode = infoCode;
                findByInfoId.infoValue = infoValue;

                findResponse.HouseHoldsResponse = soap.findHouseholdsByInfo(findByInfoId);

                success = findResponse.HouseHoldsResponse.success;
                errCode = findResponse.HouseHoldsResponse.errorCode;
                errMsg = findResponse.HouseHoldsResponse.errorMessage;

                writeLog(findByInfoId, "Request Find Household By Info");
                writeLog(findResponse, "Response Find Household By Info");

                if (success)
                {
                    result = "Find household by info success";
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

        public string findIdentifiersByInfo(string infoCode, string infoValue)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.findByInfoId findByInfoId = new GME_EngageServices.findByInfoId();
            GME_EngageServices.findIdentifiersByInfoResponse findResponse = new GME_EngageServices.findIdentifiersByInfoResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                findByInfoId.companyId = 0;
                findByInfoId.userCode = userCode;
                findByInfoId.password = password;
                findByInfoId.infoCode = infoCode;
                findByInfoId.infoValue = infoValue;

                findResponse.IdentifiersResponse = soap.findIdentifiersByInfo(findByInfoId);

                success = findResponse.IdentifiersResponse.success;
                errCode = findResponse.IdentifiersResponse.errorCode;
                errMsg = findResponse.IdentifiersResponse.errorMessage;

                writeLog(findByInfoId, "Request Find Identifier By Info");
                writeLog(findResponse, "Response Find Identifier By Info");

                if (success)
                {
                    result = "Find identifier by info success";
                }
                else
                {
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                error.ToString();
            }

            return result;
        }

        public string findPersonsByInfo(string infoCode, string infoValue)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.findByInfoId findByInfoId = new GME_EngageServices.findByInfoId();
            GME_EngageServices.findPersonsByInfoResponse findResponse = new GME_EngageServices.findPersonsByInfoResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                findByInfoId.companyId = 0;
                findByInfoId.userCode = userCode;
                findByInfoId.password = password;
                findByInfoId.infoCode = infoCode;
                findByInfoId.infoValue = infoValue;

                findResponse.PersonsResponse = soap.findPersonsByInfo(findByInfoId);

                success = findResponse.PersonsResponse.success;
                errCode = findResponse.PersonsResponse.errorCode;
                errMsg = findResponse.PersonsResponse.errorMessage;

                writeLog(findByInfoId, "Request find person by info");
                writeLog(findResponse, "Response  find person by info");

                if (success)
                {
                    result = "Find person by info success";
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

        public string generateHouseHoldNumber()
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.generateHouseHoldNumberId genHouseholdNumberId = new GME_EngageServices.generateHouseHoldNumberId();
            GME_EngageServices.generateHouseHoldNumberResponse genResponse = new GME_EngageServices.generateHouseHoldNumberResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                genHouseholdNumberId.companyId = 0;
                genHouseholdNumberId.userCode = userCode;
                genHouseholdNumberId.password = password;

                genResponse.GenerateHouseHoldNumberResponse1 = soap.generateHouseHoldNumber(genHouseholdNumberId);

                success = genResponse.GenerateHouseHoldNumberResponse1.success;
                errCode = genResponse.GenerateHouseHoldNumberResponse1.errorCode;
                errMsg = genResponse.GenerateHouseHoldNumberResponse1.errorMessage;

                writeLog(genHouseholdNumberId, "Request Generate HouseHold Number");
                writeLog(genResponse, "Response  Generate HouseHold Number");

                if (success)
                {
                    result = "Generate household success";
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

        public string getBalances(decimal houseHold)
        {
            GME_EngageServiceHO.AccountServicesClient soap = new GME_EngageServiceHO.AccountServicesClient();
            GME_EngageServiceHO.houseHoldId houseHoldId = new GME_EngageServiceHO.houseHoldId();
            GME_EngageServiceHO.getBalancesResponse balanceResponse = new GME_EngageServiceHO.getBalancesResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                houseHoldId.companyId = 0;
                houseHoldId.userCode = userCode;
                houseHoldId.password = password;
                houseHoldId.houseHoldId1 = houseHold;
                houseHoldId.houseHoldId1Specified = true;

                balanceResponse.BalanceResponse = soap.getBalances(houseHoldId);

                success = balanceResponse.BalanceResponse.success;
                errCode = balanceResponse.BalanceResponse.errorCode;
                errMsg = balanceResponse.BalanceResponse.errorMessage;

                writeLog(houseHoldId, "Request Get Balances");
                writeLog(balanceResponse, "Response  Get Balances");

                if (success)
                {
                    for (int i = 0; i < balanceResponse.BalanceResponse.totals.Length; i++)
                    {
                        if (balanceResponse.BalanceResponse.totals[i].key == "PTS")
                        {
                            result = "Success";
                            GME_Var.memberPoint = balanceResponse.BalanceResponse.totals[i].value;
                        }
                    }

                    if (result == string.Empty)
                    {
                        result = "0";
                    }
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

        public string getContactability(decimal person)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.personId personId = new GME_EngageServices.personId();
            GME_EngageServices.getContactabilityResponse contactResponse = new GME_EngageServices.getContactabilityResponse();
            GME_EngageServices.contactabilityDTO contactabilityDTO = new GME_EngageServices.contactabilityDTO();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                personId.companyId = 0;
                personId.userCode = userCode;
                personId.password = password;
                personId.personId1 = person;
                personId.personId1Specified = true;

                contactResponse.ContactabilityResponse = soap.getContactability(personId);

                success = contactResponse.ContactabilityResponse.success;
                errCode = contactResponse.ContactabilityResponse.errorCode;
                errMsg = contactResponse.ContactabilityResponse.errorMessage;
                contactabilityDTO = contactResponse.ContactabilityResponse.contactability;   
                
                for (int i = 0; i < contactabilityDTO.commTypes.Length; i++)
                {
                    if (contactabilityDTO.commTypes[i].commTypeDescription == "Opt in for Email Messages")
                    {
                        GME_Var.contactAbilitysubscriptionEmail = contactabilityDTO.commTypes[i].subscription;
                    }

                    if (contactabilityDTO.commTypes[i].commTypeDescription == "Opt in for SMS/Text Messages")
                    {
                        GME_Var.contactAbilitysubscriptionSMS = contactabilityDTO.commTypes[i].subscription;
                    }
                }

                writeLog(personId, "Request Get Contactability");
                writeLog(contactResponse, "Response  Get Contactability");

                if (success)
                {
                    result = "Find person by info success";
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

        public string getCustomerHistory(int daysInPast, decimal houseHold, string identifier)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.customerHistoryId custHistId = new GME_EngageServices.customerHistoryId();
            GME_EngageServices.getCustomerHistoryResponse custHistResponse = new GME_EngageServices.getCustomerHistoryResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                custHistId.companyId = 0;
                custHistId.userCode = userCode;
                custHistId.password = password;
                custHistId.daysInPast = daysInPast;
                custHistId.houseHoldId = houseHold;
                custHistId.houseHoldIdSpecified = true;
                custHistId.identifier = identifier;

                custHistResponse.CustomerHistoryResponse = soap.getCustomerHistory(custHistId);

                success = custHistResponse.CustomerHistoryResponse.success;
                errCode = custHistResponse.CustomerHistoryResponse.errorCode;
                errMsg = custHistResponse.CustomerHistoryResponse.errorMessage;

                writeLog(custHistId, "Request Get Customer History");
                writeLog(custHistResponse, "Response  Get Customer History");

                if (success)
                {
                    result = "Get customer history success";
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

        public string getHouseHold(decimal houseHold)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.houseHoldId houseHoldId = new GME_EngageServices.houseHoldId();
            GME_EngageServices.getHouseHoldResponse houseHoldResponse = new GME_EngageServices.getHouseHoldResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                houseHoldId.companyId = 0;
                houseHoldId.userCode = userCode;
                houseHoldId.password = password;
                houseHoldId.houseHoldId1 = houseHold;
                houseHoldId.houseHoldId1Specified = true;

                houseHoldResponse.HouseHoldResponse = soap.getHouseHold(houseHoldId);

                success = houseHoldResponse.HouseHoldResponse.success;
                errCode = houseHoldResponse.HouseHoldResponse.errorCode;
                errMsg = houseHoldResponse.HouseHoldResponse.errorMessage;

                writeLog(houseHoldId, "Request Get Household");
                writeLog(houseHoldResponse, "Response Get Household");

                if (success)
                {
                    result = "Get household success";
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

        public string getIdentifier(string identifierCode)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.identifierId identifier = new GME_EngageServices.identifierId();
            GME_EngageServices.getIdentifierResponse identifierResponse = new GME_EngageServices.getIdentifierResponse();
            GME_EngageServices.identifierDTO identifierDTO = new GME_EngageServices.identifierDTO();
            GME_EngageServices.getIdentifierRequest identifierRequest = new GME_EngageServices.getIdentifierRequest();
            GME_EngageServices.identifierResponseDTO identifierResponseDTO = new GME_EngageServices.identifierResponseDTO();

            

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;
            
            try
            {
                identifier.companyId = 0;
                identifier.userCode = userCode;
                identifier.password = password;
                identifier.identifier = identifierCode;

                identifierRequest.identifierId = identifier;

                using (soap) {
                    soap.Endpoint.Binding.ReceiveTimeout = new TimeSpan(10);
                    identifierResponse.IdentifierResponse = soap.getIdentifier(identifierRequest.identifierId);
                }
                success = identifierResponse.IdentifierResponse.success;
                errCode = identifierResponse.IdentifierResponse.errorCode;
                errMsg = identifierResponse.IdentifierResponse.errorMessage;

                writeLog(identifier, "Request Get Identifier");
                writeLog(identifierResponse, "Response Get Identifier");

                if (success)
                {
                    result = "Success";

                    if (identifierResponse.IdentifierResponse.identifier.cardType == 5)
                        GME_Var.identifierCardType = "GUEST";
                    else if (identifierResponse.IdentifierResponse.identifier.cardType == 10)
                        GME_Var.identifierCardType = "Pre-LYBC";
                    else if (identifierResponse.IdentifierResponse.identifier.cardType == 20)
                        GME_Var.identifierCardType = "LYBC Club";
                    else if (identifierResponse.IdentifierResponse.identifier.cardType == 30)
                        GME_Var.identifierCardType = "LYBC Fan";

                    GME_Var.personId = identifierResponse.IdentifierResponse.identifier.personId;
                }
                else
                {
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                error.ToString();
            }

            return result;
        }

        public string getPerson(decimal person)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.personId personId = new GME_EngageServices.personId();
            GME_EngageServices.getPersonResponse personResponse = new GME_EngageServices.getPersonResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;
            string month = string.Empty;
            string day = string.Empty;
            string year = string.Empty;

            try
            {
                personId.companyId = 0;
                personId.userCode = userCode;
                personId.password = password;
                personId.personId1 = person;
                personId.personId1Specified = true;

                personResponse.PersonResponse = soap.getPerson(personId);

                success = personResponse.PersonResponse.success;
                errCode = personResponse.PersonResponse.errorCode;
                errMsg = personResponse.PersonResponse.errorMessage;

                writeLog(personId, "Request Get Person");
                writeLog(personResponse, "Response Get Person");

                if (success)
                {
                    result = "Success";

                    GME_Var.personFirstName = personResponse.PersonResponse.person.firstName;
                    GME_Var.personLastName = personResponse.PersonResponse.person.lastName;
                    GME_Var.personEmail = personResponse.PersonResponse.person.email;
                    GME_Var.personPhone = personResponse.PersonResponse.person.cellPhone;

                    month = personResponse.PersonResponse.person.birthDate.Month.ToString();
                    day = personResponse.PersonResponse.person.birthDate.Day.ToString();
                    year = personResponse.PersonResponse.person.birthDate.Year.ToString();

                    if (month.Length < 2)
                        month = "0" + month;

                    if (day.Length < 2)
                        day = "0" + day;

                    GME_Var.personBirthdate = month + "/" + day + "/" + year;

                    GME_Var.personGender = personResponse.PersonResponse.person.gender;
                    GME_Var.personHouseHoldId = personResponse.PersonResponse.person.houseHoldId;

                    for (int i = 0; i < personResponse.PersonResponse.person.supplementalInfo.Length; i++)
                    {
                        if (personResponse.PersonResponse.person.supplementalInfo[i].code == "INF00087")
                        {
                            if (personResponse.PersonResponse.person.supplementalInfo[i].value == "NORMAL")
                            {
                                GME_Var.personSkinType = "NORMAL";
                            }
                            if (personResponse.PersonResponse.person.supplementalInfo[i].value == "OILY")
                            {
                                GME_Var.personSkinType = "OILY";
                            }
                            if (personResponse.PersonResponse.person.supplementalInfo[i].value == "DRY")
                            {
                                GME_Var.personSkinType = "DRY";
                            }
                            if (personResponse.PersonResponse.person.supplementalInfo[i].value == "COMBINATION")
                            {
                                GME_Var.personSkinType = "COMBINATION";
                            }
                        }
                    }

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

        public string getTransactionDetails(decimal logAudit)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.logAuditId logAuditId = new GME_EngageServices.logAuditId();
            GME_EngageServices.getTransactionDetailsResponse transResponse = new GME_EngageServices.getTransactionDetailsResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                logAuditId.companyId = 0;
                logAuditId.userCode = userCode;
                logAuditId.password = password;
                logAuditId.logAudit = logAudit;
                logAuditId.logAuditSpecified = true;

                transResponse.TransactionDetailsResponse = soap.getTransactionDetails(logAuditId);

                success = transResponse.TransactionDetailsResponse.success;
                errCode = transResponse.TransactionDetailsResponse.errorCode;
                errMsg = transResponse.TransactionDetailsResponse.errorMessage;

                writeLog(logAuditId, "Request Transaction Detail");
                writeLog(transResponse, "Response Transaction Detail");

                if (success)
                {
                    result = "Get transaction details success";
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

        public string getTransactionHistoryHeader(decimal houseHold, string identifier, int daysInPast)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.transactionSummaryId transSumId = new GME_EngageServices.transactionSummaryId();
            GME_EngageServices.getTransactionHistoryHeaderResponse transHeaderResponse = new GME_EngageServices.getTransactionHistoryHeaderResponse();

            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                transSumId.companyId = 0;
                transSumId.userCode = userCode;
                transSumId.password = password;
                transSumId.houseHoldId = houseHold;
                transSumId.houseHoldIdSpecified = true;
                transSumId.identifier = identifier;
                transSumId.daysInPast = daysInPast;

                transHeaderResponse.TransactionSummaryResponse = soap.getTransactionHistoryHeader(transSumId);

                success = transHeaderResponse.TransactionSummaryResponse.success;
                errCode = transHeaderResponse.TransactionSummaryResponse.errorCode;
                errMsg = transHeaderResponse.TransactionSummaryResponse.errorMessage;

                writeLog(transSumId, "Request Transaction History");
                writeLog(transHeaderResponse, "Response Transaction History");


                if (success)
                {
                    for (int i = 0; i < transHeaderResponse.TransactionSummaryResponse.logHistory.Length; i++)
                    {
                        result = string.Format("Total belanja anda sebesar {0}",
                            transHeaderResponse.TransactionSummaryResponse.logHistory[i].logAmount.ToString());

                        GME_Var.totalAmountBelanja = transHeaderResponse.TransactionSummaryResponse.logHistory[i].logAmount;
                    }
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

        public string getValidVouchersForCard(string identifier)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.identifierId identifierId = new GME_EngageServices.identifierId();
            GME_EngageServices.getValidVouchersForCardResponse voucResponse = new GME_EngageServices.getValidVouchersForCardResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;
            decimal total = GME_Propesties.GME_Var.wlcmvouchamount;

            try
            {
                identifierId.companyId = 0;
                identifierId.userCode = userCode;
                identifierId.password = password;
                identifierId.identifier = identifier;

                voucResponse.VoucherCheckResponse = soap.getValidVouchersForCard(identifierId);

                success = voucResponse.VoucherCheckResponse.success;
                errCode = voucResponse.VoucherCheckResponse.errorCode;
                errMsg = voucResponse.VoucherCheckResponse.errorMessage;

                writeLog(identifierId, "Request Valid Voucher");
                writeLog(voucResponse, "Response Valid Voucher");

                if (success)
                {

                    result = "Get voucher for card success";
                    GME_Var.vouchersResp = voucResponse.VoucherCheckResponse.vouchers;

                    //Added by Adhi (Welcome Voucher)
                    if (GME_Custom.GME_Propesties.GME_Var.isgetidentifiersukses == true && GME_Var.iswelcomevoucher == true)
                    {
                        GME_Custom.GME_Propesties.GME_Var.pilihvoucher = new string[voucResponse.VoucherCheckResponse.vouchers.Length];
                        GME_Custom.GME_Propesties.GME_Var.amount = new string[voucResponse.VoucherCheckResponse.vouchers.Length];

                        for (int i = 0; i < voucResponse.VoucherCheckResponse.vouchers.Length; i++)
                        {
                            if (voucResponse.VoucherCheckResponse.vouchers[i].voucherStatus == 1)
                            {
                                GME_Custom.GME_Propesties.GME_Var.getPilihVoucher = voucResponse.VoucherCheckResponse.vouchers[i].voucherId;
                                GME_Custom.GME_Propesties.GME_Var.pilihvoucher[i] = GME_Custom.GME_Propesties.GME_Var.getPilihVoucher;
                                
                                GME_Custom.GME_Propesties.GME_Var.getAmount = voucResponse.VoucherCheckResponse.vouchers[i].amount.ToString();
                                GME_Custom.GME_Propesties.GME_Var.amount[i] = GME_Custom.GME_Propesties.GME_Var.getAmount;
                            }
                        }
                        GME_Custom.GME_Propesties.GME_Var.getPilihVoucher = null;
                        GME_Custom.GME_Propesties.GME_Var.getAmount = null;
                        int count = 0;

                        for (int i = 0; i < GME_Custom.GME_Propesties.GME_Var.pilihvoucher.Length; i++)
                        {
                            if (GME_Custom.GME_Propesties.GME_Var.voucherdipake == GME_Custom.GME_Propesties.GME_Var.pilihvoucher[i])
                            {
                                var convertDecimal = Convert.ToDecimal(GME_Custom.GME_Propesties.GME_Var.amount[i]);
                                total = convertDecimal + total;

                                GME_Propesties.GME_Var.wlcmvouchamount = total;
                                GME_Propesties.GME_Var.isWelcomeVoucherApplied = true;
                            }
                            else
                            {
                                count++;
                                if (count == GME_Var.pilihvoucher.Length)
                                {
                                    GME_Propesties.GME_Var.isWelcomeVoucherApplied = false;
                                }                                
                            }
                        }
                        GME_Custom.GME_Propesties.GME_Var.pilihvoucher = null;
                        GME_Custom.GME_Propesties.GME_Var.amount = null;
                    }

                    // add by mar unprint voucher
                    if (GME_Var.isVoucherPrinted == true)
                    {
                        GME_Var.unPrintVouch = new string[voucResponse.VoucherCheckResponse.vouchers.Length];

                        for (int i = 0; i < voucResponse.VoucherCheckResponse.vouchers.Length; i++)
                        {
                            if (voucResponse.VoucherCheckResponse.vouchers[i].voucherStatus == 1)
                            {
                                GME_Var.getVoucher = voucResponse.VoucherCheckResponse.vouchers[i].messages[0].message; //oucherCheck.voucherId;
                                GME_Var.unPrintVouch[i] = GME_Var.getVoucher;

                            }
                        }
                        GME_Var.getVoucher = "";
                    }
                }
                else
                {
                    GME_Propesties.GME_Var.isWelcomeVoucherApplied = false;
                    result = errMsg + " ('" + errCode.ToString() + "')";
                }
            }
            catch (Exception error)
            {
                return error.ToString();
            }

            return result;
        }

        public string mergeHouseHolds(decimal sourceHouseHold, decimal targetHouseHold)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.mergeHouseHoldId mergeId = new GME_EngageServices.mergeHouseHoldId();
            GME_EngageServices.mergeHouseholdsResponse mergeResponse = new GME_EngageServices.mergeHouseholdsResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                mergeId.companyId = 0;
                mergeId.userCode = userCode;
                mergeId.password = password;
                mergeId.sourceHouseholdId = sourceHouseHold;
                mergeId.sourceHouseholdIdSpecified = true;
                mergeId.targetHouseholdId = targetHouseHold;
                mergeId.targetHouseholdIdSpecified = true;

                mergeResponse.MergeHouseholdsResponse1 = soap.mergeHouseholds(mergeId);

                success = mergeResponse.MergeHouseholdsResponse1.success;
                errCode = mergeResponse.MergeHouseholdsResponse1.errorCode;
                errMsg = mergeResponse.MergeHouseholdsResponse1.errorMessage;

                writeLog(mergeId, "Request Merge Household");
                writeLog(mergeResponse, "Response Merge Household");

                if (success)
                {
                    result = "Merge household success";
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

        public string mergePersons(decimal sourcePerson, decimal targetPerson)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.mergePersonId mergeId = new GME_EngageServices.mergePersonId();
            GME_EngageServices.mergePersonsResponse mergeResponse = new GME_EngageServices.mergePersonsResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                mergeId.companyId = 0;
                mergeId.userCode = userCode;
                mergeId.password = password;
                mergeId.sourcePersonId = sourcePerson;
                mergeId.sourcePersonIdSpecified = true;
                mergeId.targetPersonId = targetPerson;
                mergeId.targetPersonIdSpecified = true;

                mergeResponse.MergePersonsResponse1 = soap.mergePersons(mergeId);

                success = mergeResponse.MergePersonsResponse1.success;
                errCode = mergeResponse.MergePersonsResponse1.errorCode;
                errMsg = mergeResponse.MergePersonsResponse1.errorMessage;

                writeLog(mergeId, "Request Merge Person");
                writeLog(mergeResponse, "Response Merge Person");

                if (success)
                {
                    result = "Merge person success";
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

        public string renameIdentifier(string newId, string oldId)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.renameIdentifierId renameId = new GME_EngageServices.renameIdentifierId();
            GME_EngageServices.renameIdentifierResponse renameResponse = new GME_EngageServices.renameIdentifierResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                renameId.companyId = 0;
                renameId.userCode = userCode;
                renameId.password = password;
                renameId.newIdCode = newId;
                renameId.oldIdCode = oldId;

                renameResponse.RenameIdentifierResponse1 = soap.renameIdentifier(renameId);

                success = renameResponse.RenameIdentifierResponse1.success;
                errCode = renameResponse.RenameIdentifierResponse1.errorCode;
                errMsg = renameResponse.RenameIdentifierResponse1.errorMessage;

                writeLog(renameId, "Request Rename Identifier");
                writeLog(renameResponse, "Response Rename Identifier");

                if (success)
                {
                    result = "Rename identifier success";
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

        public string splitPerson(decimal sourcePerson, decimal targetHouseHold)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.splitPersonId splitPersonId = new GME_EngageServices.splitPersonId();
            GME_EngageServices.splitPersonResponse splitResponse = new GME_EngageServices.splitPersonResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                splitPersonId.companyId = 0;
                splitPersonId.userCode = userCode;
                splitPersonId.password = password;
                splitPersonId.sourcePersonId = sourcePerson;
                splitPersonId.sourcePersonIdSpecified = true;
                splitPersonId.targetHouseholdId = targetHouseHold;
                splitPersonId.targetHouseholdIdSpecified = true;

                splitResponse.SplitPersonResponse1 = soap.splitPerson(splitPersonId);

                success = splitResponse.SplitPersonResponse1.success;
                errCode = splitResponse.SplitPersonResponse1.errorCode;
                errMsg = splitResponse.SplitPersonResponse1.errorMessage;

                writeLog(splitPersonId, "Request split person");
                writeLog(splitResponse, "Response split person");

                if (success)
                {
                    result = "Split person success";
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

        public string updateHousehold(decimal householdId, string storeNumber, string email, string phoneNumber)
        {
            GME_EngageServices.AccountServicesClient soap = new GME_EngageServices.AccountServicesClient();
            GME_EngageServices.houseHoldId houseHoldId = new GME_EngageServices.houseHoldId();
            GME_EngageServices.getHouseHoldResponse getHouseHoldResponse = new GME_EngageServices.getHouseHoldResponse();
            GME_EngageServices.houseHoldDTO houseHold = new GME_EngageServices.houseHoldDTO();
            GME_EngageServices.updateHouseHoldDTO houseHoldDTO = new GME_EngageServices.updateHouseHoldDTO();
            GME_EngageServices.updateHouseHoldResponse houseHoldResponse = new GME_EngageServices.updateHouseHoldResponse();
            bool success = false;
            int errCode = 0;
            string errMsg = string.Empty;
            string result = string.Empty;

            try
            {
                houseHoldId.companyId = 0;
                houseHoldId.userCode = userCode;
                houseHoldId.password = password;
                houseHoldId.houseHoldId1 = householdId;
                houseHoldId.houseHoldId1Specified = true;

                getHouseHoldResponse.HouseHoldResponse = soap.getHouseHold(houseHoldId);

                houseHold.addedDate = DateTime.Today;
                houseHold.addedDateSpecified = true;
                houseHold.email = email;
                houseHold.homePhone = phoneNumber;
                houseHold.houseHoldId = householdId;
                houseHold.houseHoldIdSpecified = true;
                houseHold.personList = getHouseHoldResponse.HouseHoldResponse.houseHold.personList;
                houseHold.status = 1;
                houseHold.storeNumber = storeNumber;
                houseHold.streetType = 1;

                houseHoldDTO.companyId = 0;
                houseHoldDTO.userCode = userCode;
                houseHoldDTO.password = password;
                houseHoldDTO.houseHold = houseHold;

                houseHoldResponse.UpdateHouseHoldResponse1 = soap.updateHouseHold(houseHoldDTO);

                success = houseHoldResponse.UpdateHouseHoldResponse1.success;
                errCode = houseHoldResponse.UpdateHouseHoldResponse1.errorCode;
                errMsg = houseHoldResponse.UpdateHouseHoldResponse1.errorMessage;

                writeLog(houseHoldDTO, "Request update household");
                writeLog(houseHoldResponse, "Response update house hold");

                if (success)
                {
                    result = "Success";
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

        #region FALWS Services
        public string deleteBrand(string brandCode)
        {
            string responseMsg;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.deleteBrandResponse delResponse = new GME_EngageFALWSServices.deleteBrandResponse();

            delResponse.ResponseMessage = soap.deleteBrand(brandCode);
            responseMsg = delResponse.ResponseMessage.success.ToString();

            return responseMsg;
        }

        public string lookupCardByPhone(string phoneNumber)
        {
            string result = string.Empty;
            decimal personId = 0;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.cardData cardData = new GME_EngageFALWSServices.cardData();
            GME_EngageFALWSServices.cardLookupRequest request = new GME_EngageFALWSServices.cardLookupRequest();
            GME_EngageFALWSServices.cardLookupResponse response = new GME_EngageFALWSServices.cardLookupResponse();
            GME_EngageFALWSServices.lookupCardsResponse cardResponse = new GME_EngageFALWSServices.lookupCardsResponse();

            if (phoneNumber == string.Empty)
                phoneNumber = "0";

            try
            {
                request.phoneNumber = phoneNumber;
                request.searchAll = true;

                using (soap)
                {
                    soap.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 0, 5, 0);//100000000
                    cardResponse.CardLookupResponse = soap.lookupCards(request);
                }

                writeLog(request, "Request Lookup Card Phone");
                writeLog(response, "Response Lookup Card Phone");

                if (cardResponse.CardLookupResponse.cards != null)
                {
                    for (int i = 0; i < cardResponse.CardLookupResponse.cards.Length; i++)
                    {
                        result = cardResponse.CardLookupResponse.cards[i].cardStatus;

                        if (result == "VALID")
                        {                            
                            GME_Var.lookupCardNumber = cardResponse.CardLookupResponse.cards[i].cardNumber;
                            GME_Var.lookupCardTier = cardResponse.CardLookupResponse.cards[i].cardTypeDescription;
                            GME_Var.lookupCardFirstName = cardResponse.CardLookupResponse.cards[i].firstName;
                            GME_Var.lookupCardLastName = cardResponse.CardLookupResponse.cards[i].lastName;
                            GME_Var.lookupCardPhoneNumber = cardResponse.CardLookupResponse.cards[i].phoneNumber;
                            GME_Var.lookupCardPhoneNumber = cardResponse.CardLookupResponse.cards[i].phoneNumber;
                            decimal.TryParse(cardResponse.CardLookupResponse.cards[i].personNumber, out personId);
                            GME_Var.lookupCardPersonId = personId;

                            break;
                        }
                    }
                }
                else
                {
                    if (cardResponse.CardLookupResponse.onlineMode == true)
                        result = "INVALID";
                    else
                        throw new Exception("Disconnected to engage central");
                }
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Koneksi terputus");

                result = er.Message;
            }

            GME_Var.lookupCardStatus = result;
            return result;
        }

        public string lookupCardByEmail(string email)
        {
            string result = string.Empty;
            decimal personId = 0;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.cardData cardData = new GME_EngageFALWSServices.cardData();
            GME_EngageFALWSServices.cardLookupRequest request = new GME_EngageFALWSServices.cardLookupRequest();
            GME_EngageFALWSServices.cardLookupResponse response = new GME_EngageFALWSServices.cardLookupResponse();
            GME_EngageFALWSServices.lookupCardsResponse cardResponse = new GME_EngageFALWSServices.lookupCardsResponse();

            if (email == string.Empty)
                email = "@";

            try
            {
                request.email = email;
                request.searchAll = true;

                using (soap)
                {
                    soap.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 0, 5, 0);//100000000
                    cardResponse.CardLookupResponse = soap.lookupCards(request);
                }

                request.ToString();

                writeLog(request, "Request Lookup Card Email");
                writeLog(cardResponse, "Response Lookup Card Email");

                if (cardResponse.CardLookupResponse.cards != null)
                {
                    for (int i = 0; i < cardResponse.CardLookupResponse.cards.Length; i++)
                    {
                        result = cardResponse.CardLookupResponse.cards[i].cardStatus;

                        if (result == "VALID")
                        {                            
                            GME_Var.lookupCardNumber = cardResponse.CardLookupResponse.cards[i].cardNumber;
                            GME_Var.lookupCardTier = cardResponse.CardLookupResponse.cards[i].cardTypeDescription;
                            GME_Var.lookupCardFirstName = cardResponse.CardLookupResponse.cards[i].firstName;
                            GME_Var.lookupCardLastName = cardResponse.CardLookupResponse.cards[i].lastName;
                            GME_Var.lookupCardPhoneNumber = cardResponse.CardLookupResponse.cards[i].phoneNumber;
                            GME_Var.lookupCardEmail = cardResponse.CardLookupResponse.cards[i].email;
                            decimal.TryParse(cardResponse.CardLookupResponse.cards[i].personNumber, out personId);
                            GME_Var.lookupCardPersonId = personId;                            

                            break;
                        }
                    }
                }
                else
                {
                    if (cardResponse.CardLookupResponse.onlineMode == true)
                        result = "INVALID";
                    else
                        throw new Exception("Disconnected to engage central");
                }                
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Data tidak ditemukan");

                result = er.ToString();
            }

            GME_Var.lookupCardStatus = result;
            return result;
        }

        public string lookupCard(string cardNumber)
        {
            string errMsg = string.Empty;
            string result = string.Empty;
            decimal personId = 0;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.cardData cardData = new GME_EngageFALWSServices.cardData();
            GME_EngageFALWSServices.cardLookupRequest request = new GME_EngageFALWSServices.cardLookupRequest();
            GME_EngageFALWSServices.cardLookupResponse response = new GME_EngageFALWSServices.cardLookupResponse();
            GME_EngageFALWSServices.lookupCardsResponse cardResponse = new GME_EngageFALWSServices.lookupCardsResponse();

            try
            {
                request.cardNumber = cardNumber;
                request.searchAll = true;

                using (soap)
                {
                    soap.Endpoint.Binding.SendTimeout = new TimeSpan(0,0,0,5,0);//100000000
                    cardResponse.CardLookupResponse = soap.lookupCards(request);
                }

                writeLog(request, "Request Lookup Card");
                writeLog(cardResponse, "Response Lookup Card");

                if (cardResponse.CardLookupResponse.cards != null)
                {
                    for (int i = 0; i < cardResponse.CardLookupResponse.cards.Length; i++)
                    {
                        result = cardResponse.CardLookupResponse.cards[i].cardStatus;

                        if (result == "VALID")
                        {
                            GME_Var.lookupCardNumber = cardResponse.CardLookupResponse.cards[i].cardNumber;
                            GME_Var.lookupCardTier = cardResponse.CardLookupResponse.cards[i].cardTypeDescription;
                            GME_Var.lookupCardFirstName = cardResponse.CardLookupResponse.cards[i].firstName;
                            GME_Var.lookupCardLastName = cardResponse.CardLookupResponse.cards[i].lastName;
                            GME_Var.lookupCardPhoneNumber = cardResponse.CardLookupResponse.cards[i].phoneNumber;
                            GME_Var.lookupCardEmail = cardResponse.CardLookupResponse.cards[i].email;
                            GME_Var.lookupCardStatusCard = cardResponse.CardLookupResponse.cards[i].cardStatus;                            
                            decimal.TryParse(cardResponse.CardLookupResponse.cards[i].personNumber, out personId);
                            GME_Var.lookupCardPersonId = personId;
                        }
                    }
                }
                else
                {
                    if (cardResponse.CardLookupResponse.onlineMode == true)
                        result = "INVALID";
                    else
                        throw new Exception("Disconnected to engage central");
                }
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Data tidak ditemukan");

                result = er.ToString();
            }

            GME_Var.lookupCardStatus = result;
            return result;
        }

        public string requestReward0302(string cardNumber, string storeNumber)
        {
            bool success = false;
            string result = string.Empty;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.requestRewardsRequest request = new GME_EngageFALWSServices.requestRewardsRequest();
            GME_EngageFALWSServices.requestRewardsResponse response = new GME_EngageFALWSServices.requestRewardsResponse();
            GME_EngageFALWSServices.basketRequest basketReq = new GME_EngageFALWSServices.basketRequest();
            GME_EngageFALWSServices.customerDetails custDetail = new GME_EngageFALWSServices.customerDetails();
            GME_EngageFALWSServices.customerName custName = new GME_EngageFALWSServices.customerName();

            try
            {
                basketReq.PAN = cardNumber;
                basketReq.functionID = "0302";
                basketReq.processingCode = "900000";
                basketReq.currencyCode = "IDR";
                basketReq.siteCode = storeNumber;                

                request.RewardsRequest = basketReq;

                response.RewardsResponse = soap.requestRewards(request.RewardsRequest);

                success = response.RewardsResponse.success;
                custDetail = response.RewardsResponse.customerDetails;

                writeLog(request, "Request Reward 0302");
                writeLog(response, "Response Reward 0302");

                if (custDetail != null)
                {
                    GME_Var.custDetailReq0302 = true;
                }
                else
                {
                    GME_Var.custDetailReq0302 = false;
                }

                if (success)
                {
                    GME_Var.req0302Response = true;
                    result = "Success";
                }
                else
                {
                    GME_Var.req0302Response = false;
                    result = "Not success";
                }
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Data tidak ditemukan");

                result = er.ToString();
            }

            return result;
        }

        public string requestReward900017(string cardNumber, string storeNumber, string transactionNumber, string cashierID, string terminalID, string transactionDate, string transactionTime, GME_EngageFALWSServices.ticket tick)
        {

            bool success = false;
            string result = string.Empty;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.requestRewardsRequest request = new GME_EngageFALWSServices.requestRewardsRequest();
            GME_EngageFALWSServices.basketRequest basketRequest = new GME_EngageFALWSServices.basketRequest();
            GME_EngageFALWSServices.requestRewardsResponse response = new GME_EngageFALWSServices.requestRewardsResponse();

            terminalID = substrTerminalID(terminalID);

            basketRequest.PAN = cardNumber;
            basketRequest.transactionNumber = transactionNumber;
            basketRequest.functionID = "9000";
            basketRequest.processingCode = "900017";
            basketRequest.currencyCode = "IDR";
            basketRequest.terminalID = terminalID;
            basketRequest.transactionDate = transactionDate;
            basketRequest.transactionTime = transactionTime;
            basketRequest.terminalType = "POS";
            basketRequest.cashierID = cashierID;
            basketRequest.siteCode = storeNumber;

            basketRequest.ticket = tick;
            request.RewardsRequest = basketRequest;
            response.RewardsResponse = soap.requestRewards(request.RewardsRequest);

            success = response.RewardsResponse.success;

            writeLog(request, "Request Reward 9000");
            writeLog(response, "Response Reward 9000");

            if (success)
            {
                result = "Success";

                GME_EngageFALWSServices.rewardsField rewardsField = new GME_EngageFALWSServices.rewardsField();
                rewardsField = response.RewardsResponse.rewardsGranted;


                GME_Var.pointBalance = Convert.ToInt32(rewardsField.pointsBalance);
                GME_Var.tempPointBalance = Convert.ToInt32(rewardsField.tempPointsBalance);


            }
            else
            {
                result = "Not success";
            }

            return result;
        }


        public string requestReward9100(string cardNumber, string storeNumber, string transactionNumber, string cashierID, string terminalID, string transactionDate, string transactionTime, GME_EngageFALWSServices.ticket tick)
        {

            bool success = false;
            string result = string.Empty;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.requestRewardsRequest request = new GME_EngageFALWSServices.requestRewardsRequest();
            GME_EngageFALWSServices.basketRequest basketRequest = new GME_EngageFALWSServices.basketRequest();
            GME_EngageFALWSServices.requestRewardsResponse response = new GME_EngageFALWSServices.requestRewardsResponse();

            try
            {
                terminalID = substrTerminalID(terminalID);

                basketRequest.PAN = cardNumber;
                basketRequest.transactionNumber = transactionNumber;
                basketRequest.functionID = "9100";
                basketRequest.processingCode = "900000";
                basketRequest.currencyCode = "IDR";
                basketRequest.terminalID = terminalID;
                basketRequest.transactionDate = transactionDate;
                basketRequest.transactionTime = transactionTime;
                basketRequest.terminalType = "POS";
                basketRequest.cashierID = cashierID;
                basketRequest.siteCode = storeNumber;



                basketRequest.ticket = tick;
                request.RewardsRequest = basketRequest;
                response.RewardsResponse = soap.requestRewards(request.RewardsRequest);

                success = response.RewardsResponse.success;

                writeLog(request, "Request Reward 9100");
                writeLog(response, "Response Reward 9100");

                if (success)
                {
                    result = "Success";
                    GME_EngageFALWSServices.rewardsField rewardsField = new GME_EngageFALWSServices.rewardsField();
                    GME_EngageFALWSServices.coupons coupons = new GME_EngageFALWSServices.coupons();
                    rewardsField = response.RewardsResponse.rewardsGranted;
                    coupons = response.RewardsResponse.coupons;

                    if (rewardsField.auditNumber != null)
                    {
                        GME_Var.auditNumber = rewardsField.auditNumber.ToString();
                        GME_Var.tempPointBalance = Convert.ToInt32(rewardsField.tempPointsBalance);
                        GME_Var.PointVisit = Convert.ToInt32(rewardsField.pointsEarned);

                    }

                    if (coupons != null)
                    {
                        try
                        {
                            if (coupons.coupon != null)
                            {
                                GME_Var.isGrantedCoupon = true;
                                for (int i = 0; i < coupons.coupon.Length; i++)
                                {
                                    GME_Var.GratedCoupons.Add(coupons.coupon[i].messages.message[0]);
                                }
                            }
                        }
                        catch(Exception e)
                        {

                        }
                        
                       
                    }
                }
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Data tidak ditemukan");

                result = er.ToString();
            }

            return result;
        }

        public string requestReward9220(string cardNumber, string storeNumber, string transactionNumber, string cashierID, string terminalID, string transactionDate, string transactionTime, GME_EngageFALWSServices.paymentMethods paymentMethods, GME_EngageFALWSServices.ticket tick)
        {

            bool success = false;
            string result = string.Empty;

            GME_EngageFALWSServices.FALWebServicesImplClient soap = new GME_EngageFALWSServices.FALWebServicesImplClient();
            GME_EngageFALWSServices.requestRewardsRequest request = new GME_EngageFALWSServices.requestRewardsRequest();
            GME_EngageFALWSServices.basketRequest basketRequest = new GME_EngageFALWSServices.basketRequest();
            GME_EngageFALWSServices.requestRewardsResponse response = new GME_EngageFALWSServices.requestRewardsResponse();

            try
            {
                terminalID = substrTerminalID(terminalID);

                basketRequest.PAN = cardNumber;
                basketRequest.transactionNumber = transactionNumber;
                basketRequest.functionID = "9220";
                basketRequest.processingCode = "900000";
                basketRequest.currencyCode = "IDR";
                basketRequest.terminalID = terminalID;
                basketRequest.transactionDate = transactionDate;
                basketRequest.transactionTime = transactionTime;
                basketRequest.terminalType = "POS";
                basketRequest.cashierID = cashierID;
                basketRequest.siteCode = storeNumber;


                basketRequest.paymentMethods = paymentMethods;
                basketRequest.ticket = tick;

                request.RewardsRequest = basketRequest;
                response.RewardsResponse = soap.requestRewards(request.RewardsRequest);

                success = response.RewardsResponse.success;

                writeLog(request, "Request Reward 9220");
                writeLog(response, "Response Reward 9220");

                if (success)
                {
                    result = "Success";
                    customerDetails custDetail = response.RewardsResponse.customerDetails;
                    if (custDetail != null)
                    {
                        balances balances = custDetail.balances;
                        if (balances != null)
                        {
                            for (int i = 0; i < balances.balance.Length; i++)
                            {
                                if (balances.balance[i].code == "PTS")
                                {
                                    GME_Var.totalPoint = Convert.ToInt32(balances.balance[i].amount);
                                }

                            }

                        }
                    }
                }
                else
                {
                    result = "Not success";
                }
            }
            catch (Exception er)
            {
                //BlankOperations.ShowMsgBox("Data tidak ditemukan");

                result = er.ToString();
            }

            return result;
        }

        /// <summary>
        /// added this method for substring terminal id
        /// Engage can't process terminalid format SEATTLE-10 so needed to substring get the digit only
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string substrTerminalID(string val)
        {
            string[] tempTerminalID = val.Split('-');

            if (tempTerminalID[1].Length < 2)
            {
                string frontpad = String.Empty;
                int loop = 2 - tempTerminalID[1].Length;
                for (int k = 0; k < loop; k++)
                {
                    frontpad += "0";
                }
                val = frontpad + tempTerminalID[1];

            }
            else
            {
                val = tempTerminalID[1];
            }
            return val;
        }
        #endregion

        #region AIF Voucher
        public vouchersList lookup(string companyId, string voucherCode, string siteCode)
        {
            vouchersList voucherList = new vouchersList();
            voucherList.VoucherItems = new List<voucher>();

            TBS_AifVoucherSvcClient client = new TBS_AifVoucherSvcClient();
            GME_VoucherAIFPROD.CallContext context = new GME_VoucherAIFPROD.CallContext();
            context.Company = companyId;

            try
            {
                if(GME_Var.ListAttLoginConfig.Count == 3)
                {
                    client.ClientCredentials.Windows.ClientCredential.Domain = GME_Var.ListAttLoginConfig[0];
                    client.ClientCredentials.Windows.ClientCredential.UserName = GME_Var.ListAttLoginConfig[1];
                    client.ClientCredentials.Windows.ClientCredential.Password = GME_Var.ListAttLoginConfig[2];
                } else
                {
                    BlankOperations.ShowMsgBoxInformation("pengaturan ke pusat tidak ditemukan. Silahkan hubungi Help Desk");
                    goto outthisfunc;
                }


                TBS_Voucher[] voucher;
                voucher = client.getVoucherType(context, voucherCode, siteCode);

                for (int i = 0; i < voucher.Length; i++)
                {

                    voucher item = new voucher();
                    if (GME_Var.isCash == true)
                    {
                        if (GME_Var.isCashVoucher && voucher[i].voucherType == "Cash Voucher")
                        {
                            item.responseCode = voucher[i].responseCode;
                            item.responseMessage = voucher[i].responseMessage;
                            GME_Var.isDiscVoucher = false;
                        }
                        else
                        {
                            item.responseCode = voucher[i].responseCode;
                            item.responseMessage = voucher[i].responseMessage;
                            GME_Var.isCashVoucher = false;
                        }
                    }
                    if (GME_Var.isDisc == true)
                    {
                        if (GME_Var.isDisc && voucher[i].voucherType == "Discount Voucher")
                        {
                            item.responseCode = voucher[i].responseCode;
                            item.responseMessage = voucher[i].responseMessage;
                            GME_Var.isCashVoucher = false;
                        }
                        else
                        {
                            item.responseCode = voucher[i].responseCode;
                            item.responseMessage = voucher[i].responseMessage;
                            GME_Var.isDiscVoucher = false;
                        }
                    }
                    if (GME_Var.isAktifVoucher == true)
                    {
                        item.responseCode = voucher[i].responseCode;
                        item.responseMessage = voucher[i].responseMessage;
                    }
                    voucherList.VoucherItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                voucherList = null;
            }
            outthisfunc:;
            return voucherList;
        }

        public vouchersList updateVoucherRedeem(string companyId, string voucherCode, string siteCode)
        {
            vouchersList voucherList = new vouchersList();
            voucherList.VoucherItems = new List<voucher>();

            TBS_AifVoucherSvcClient client = new TBS_AifVoucherSvcClient();
            GME_VoucherAIFPROD.CallContext context = new GME_VoucherAIFPROD.CallContext();
            context.Company = companyId;

            if (GME_Var.ListAttLoginConfig.Count == 3)
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = GME_Var.ListAttLoginConfig[0];
                client.ClientCredentials.Windows.ClientCredential.UserName = GME_Var.ListAttLoginConfig[1];
                client.ClientCredentials.Windows.ClientCredential.Password = GME_Var.ListAttLoginConfig[2];
            }
            else
            {
                BlankOperations.ShowMsgBoxInformation("pengaturan ke pusat tidak ditemukan. Silahkan hubungi Help Desk");
                goto outthisfunc;
            }

            TBS_Voucher[] voucher;
            voucher = client.redeem(context, voucherCode, siteCode);

            for (int i = 0; i < voucher.Length; i++)
            {
                voucher item = new voucher();
                item.responseCode = voucher[i].responseCode;
                item.responseMessage = voucher[i].responseMessage;
                voucherList.VoucherItems.Add(item);
            }

            outthisfunc:;
            return voucherList;
        }

        public vouchersList updateVoucherActive(string companyId, string voucherCode, string siteCode)
        {
            vouchersList voucherList = new vouchersList();
            voucherList.VoucherItems = new List<voucher>();

            TBS_AifVoucherSvcClient client = new TBS_AifVoucherSvcClient();
            GME_VoucherAIFPROD.CallContext context = new GME_VoucherAIFPROD.CallContext();
            context.Company = companyId;

            if (GME_Var.ListAttLoginConfig.Count == 3)
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = GME_Var.ListAttLoginConfig[0];
                client.ClientCredentials.Windows.ClientCredential.UserName = GME_Var.ListAttLoginConfig[1];
                client.ClientCredentials.Windows.ClientCredential.Password = GME_Var.ListAttLoginConfig[2];
            }
            else
            {
                BlankOperations.ShowMsgBoxInformation("pengaturan ke pusat tidak ditemukan. Silahkan hubungi Help Desk");
                goto outthisfunc;
            }

            TBS_Voucher[] voucher;
            voucher = client.activate(context, voucherCode, siteCode);

            for (int i = 0; i < voucher.Length; i++)
            {
                voucher item = new voucher();
                item.responseCode = voucher[i].responseCode;
                item.responseMessage = voucher[i].responseMessage;
                voucherList.VoucherItems.Add(item);
            }

            outthisfunc:;
            return voucherList;
        }
        #endregion

        #region AIF Loyalty
        public int getPoint(string cardNumber, string companyId)
        {
            TBS_AifLoyaltySvcClient client = new TBS_AifLoyaltySvcClient();
            GME_LoyaltyAIFPROD.CallContext context = new GME_LoyaltyAIFPROD.CallContext();
            TBS_AifLoyaltySvcUpdatePointRequest req = new TBS_AifLoyaltySvcUpdatePointRequest();
            TBS_AifLoyaltySvcUpdatePointResponse res = new TBS_AifLoyaltySvcUpdatePointResponse();
            context.Company = companyId;
            int result = 0;

            try
            {
                if (GME_Var.ListAttLoginConfig.Count == 3)
                {
                    client.ClientCredentials.Windows.ClientCredential.Domain = GME_Var.ListAttLoginConfig[0];
                    client.ClientCredentials.Windows.ClientCredential.UserName = GME_Var.ListAttLoginConfig[1];
                    client.ClientCredentials.Windows.ClientCredential.Password = GME_Var.ListAttLoginConfig[2];
                }
                else
                {
                    BlankOperations.ShowMsgBoxInformation("pengaturan ke pusat tidak ditemukan. Silahkan hubungi Help Desk");
                    goto outthisfunc;
                }

                result = client.getLastPoint(context, cardNumber);
            }
            catch (Exception er)
            {
                BlankOperations.ShowMsgBox("Mohon maaf transaksi sedang offline, tidak dapat menampilkan informasi point");

                result = 0;
            }

            outthisfunc:;
            return result;
        }

        public void updatePoint(string cardNumber, decimal point, string companyId)
        {
            TBS_AifLoyaltySvcClient client = new TBS_AifLoyaltySvcClient();
            GME_LoyaltyAIFPROD.CallContext context = new GME_LoyaltyAIFPROD.CallContext();
            TBS_AifLoyaltySvcUpdatePointRequest req = new TBS_AifLoyaltySvcUpdatePointRequest();
            TBS_AifLoyaltySvcUpdatePointResponse res = new TBS_AifLoyaltySvcUpdatePointResponse();
            context.Company = companyId;
            string result;

            try
            {
                if (GME_Var.ListAttLoginConfig.Count == 3)
                {
                    client.ClientCredentials.Windows.ClientCredential.Domain = GME_Var.ListAttLoginConfig[0];
                    client.ClientCredentials.Windows.ClientCredential.UserName = GME_Var.ListAttLoginConfig[1];
                    client.ClientCredentials.Windows.ClientCredential.Password = GME_Var.ListAttLoginConfig[2];
                }
                else
                {
                    BlankOperations.ShowMsgBoxInformation("pengaturan ke pusat tidak ditemukan. Silahkan hubungi Help Desk");
                    goto outthisfunc;
                }

                result = client.updatePoint(context, cardNumber, point);
                GME_Var.isUpdatePointAX = true;
            }
            catch (Exception err)
            {
                GME_Var.isUpdatePointAX = false;
                BlankOperations.ShowMsgBox("Saat ini tidak dapat melakukan update point, karena system tidak tersambung kepusat");
            }
            outthisfunc:;
        }
        #endregion


        private GME_EngageServices.commOption setcommOption(string desc, int option)
        {
            GME_EngageServices.commOption commOption = new GME_EngageServices.commOption();
            commOption.commOptionDescription = desc;
            commOption.communicationOption = option;
            commOption.optin = true;

            return commOption;
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
