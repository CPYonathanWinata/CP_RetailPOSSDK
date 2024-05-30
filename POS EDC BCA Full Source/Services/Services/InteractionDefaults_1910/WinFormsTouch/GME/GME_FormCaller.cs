using System;
using GME_Custom;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using LSRetailPosis.POSProcesses;
using LSRetailPosis.Transaction;
using LSRetailPosis.Transaction.Line.TenderItem;
using System.Collections.Generic;
using LSRetailPosis.Transaction.Line;
using LSRetailPosis.Transaction.Line.SaleItem;
using System.Windows.Forms;
using System.Data;

using Microsoft.Dynamics.Retail.Notification.Contracts;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.WinFormsTouch
{
    public class GME_FormCaller
    {        
        public static void GME_FormCustomerInfo(IApplication application)
        {
            GME_Custom.GME_Propesties.frmCustInfoConfirm inputFrmCustInfo = new GME_Custom.GME_Propesties.frmCustInfoConfirm()
            {
                
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmCustInfo,
                () =>
                {
                    if (inputFrmCustInfo.Confirmed)
                    {
                        
                    }
                });

            application.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormCustomerType(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmCustTypeConfirm inputFrmCustType = new GME_Custom.GME_Propesties.frmCustTypeConfirm()
            {
                posTransaction = posTransactionParm,
                application = applicationParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmCustType,
                () =>
                {
                    if (inputFrmCustType.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormFamilyMember(IApplication application)
        {
            GME_Custom.GME_Propesties.frmFamilyMemberConfirm inputFrmFamMember = new GME_Custom.GME_Propesties.frmFamilyMemberConfirm()
            {

            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmFamMember,
                () =>
                {
                    if (inputFrmFamMember.Confirmed)
                    {

                    }
                });
             
            application.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormBonManual(IApplication application, string storeId)
        {
            GME_Custom.GME_Propesties.frmBonManualConfirm inputFrmBonManual = new GME_Custom.GME_Propesties.frmBonManualConfirm()
            {
                apps = application,
                storeId = storeId
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmBonManual,
                () =>
                {
                    if (inputFrmBonManual.Confirmed)
                    {

                    }
                });

            application.Services.Interaction.InteractionRequest(request);
        }              

        public static void GME_FormPublic(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmPublicConfirm inputFrmPublic = new GME_Custom.GME_Propesties.frmPublicConfirm()
            {
                posTransaction = posTransactionParm,
                application = applicationParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmPublic,
                () =>
                {
                    if (inputFrmPublic.Confirmed)
                    {
                        GME_Custom.GME_Propesties.GME_Var.custEmail = inputFrmPublic.custEmail;
                        GME_Custom.GME_Propesties.GME_Var.custName = inputFrmPublic.custName;
                        GME_Custom.GME_Propesties.GME_Var.custPhone = inputFrmPublic.custPhoneNum;
                        GME_Custom.GME_Propesties.GME_Var.custGroup = inputFrmPublic.custGroup;
                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormLYBCMember(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmLYBCMemberConfirm inputFrmLYBCMember = new GME_Custom.GME_Propesties.frmLYBCMemberConfirm()
            {
                posTransaction = posTransactionParm,
                application = applicationParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmLYBCMember,
                () =>
                {
                    if (inputFrmLYBCMember.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormTest(IApplication applicationParm, string custNameParm, string custEmailParm)
        {
            GME_Custom.GME_Propesties.frmTestConmfirm inputFrmTest = new GME_Custom.GME_Propesties.frmTestConmfirm()
            {                
                application = applicationParm,
                CustName = custNameParm,
                CustEmail = custEmailParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmTest,
                () =>
                {
                    if (inputFrmTest.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Added by Adhi 
        public static void GME_FormEmployeePurchase(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm inputfrmEmployeePurchase = new GME_Custom.GME_Propesties.frmEmployeePurchaseConfirm()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmEmployeePurchase,
                () =>
                {
                    if (inputfrmEmployeePurchase.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Added by Adhi 
        public static void GME_FormFamilyPurchase(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm inputfrmFamilyPurchase = new GME_Custom.GME_Propesties.frmFamilyPurchaseConfirm()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmFamilyPurchase,
                () =>
                {
                    if (inputfrmFamilyPurchase.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        ///  ADD BY MARIA-WCS  BBOB FORM    
        public static void GME_FormFanBOBB(IApplication applicationParm)//, string pointParm, string convPointParm, string paymAmtParm, string availPointsParm)
        {
            GME_Custom.GME_Propesties.frmFanBOBBconfirm inputFormFanBOBB = new GME_Custom.GME_Propesties.frmFanBOBBconfirm()
            {
                application = applicationParm,
                // point       = pointParm,
                //// convPoint   = convPointParm,
                //  paymAmt     = paymAmtParm,
                //  availPoints = availPointsParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFormFanBOBB,
                () =>
                {
                    if (inputFormFanBOBB.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }
        ///  END  BBOB FORM  
        
        public static void GME_FormEnrollment(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            GME_Custom.GME_Propesties.frmEnrollmentConfirm inputFromEnrollment = new GME_Custom.GME_Propesties.frmEnrollmentConfirm()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFromEnrollment,
                () =>
                {
                    if (inputFromEnrollment.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Add by Adhi (Form Sales Person)
        public static void GME_FormSalesPerson(IApplication applicationParm)
        {
            GME_Custom.GME_Propesties.frmSalesPersonConfirm inputfrmSalesPerson = new GME_Custom.GME_Propesties.frmSalesPersonConfirm()
            {
                application = applicationParm                
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmSalesPerson,
                () =>
                {
                    if (inputfrmSalesPerson.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //ADD BY MARIA - FORM DONASI 
        public static void GME_FormDonasi(IApplication applicationParm)//, IPosTransaction posTransactionParm)//, string amtDonasiParm)
        {
            GME_Custom.GME_Propesties.FrmDonasiConfirm inputfrmEmployeePurchase = new GME_Custom.GME_Propesties.FrmDonasiConfirm()
            {
                application = applicationParm,
                //posTransaction = posTransactionParm,
                // AmtDonasi = amtDonasiParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmEmployeePurchase,
                () =>
                {
                    if (inputfrmEmployeePurchase.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }
    }
}
