using System;
using GME_Custom;
using GME_Custom.GME_Propesties;
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
using Microsoft.Dynamics.Retail.Pos.Contracts.Triggers;
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
                        GME_Var.custEmail = inputFrmPublic.custEmail;
                        GME_Var.custName = inputFrmPublic.custName;
                        GME_Var.custPhone = inputFrmPublic.custPhoneNum;
                        GME_Var.custGroup = inputFrmPublic.custGroup;
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
        public static void GME_FormFanBOBB(IApplication applicationParm)
        {
            GME_Custom.GME_Propesties.frmFanBOBBconfirm inputFormFanBOBB = new GME_Custom.GME_Propesties.frmFanBOBBconfirm()
            {
                application = applicationParm,
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
        public static void GME_FormDonasi(IApplication applicationParm)
        {
            FrmDonasiConfirm inputFormDonasi = new FrmDonasiConfirm()
            {
                application = applicationParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFormDonasi,
                () =>
                {
                    if (inputFormDonasi.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //ADD BY MARIA - FORM Card Replacement
        public static void GME_FormCardReplacement(IApplication applicationParm, IPosTransaction posTransaction, string oldCardNumParm)
        {
            GME_Custom.GME_Propesties.FrmCardReplacementConfirm inputfrmEmployeePurchase = new GME_Custom.GME_Propesties.FrmCardReplacementConfirm()
            {
                application = applicationParm,
                oldCardNumber = oldCardNumParm,
                posTransaction = posTransaction
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

        //ADD BY MARIA - FORM UNPRINTED VOUCHER 
        public static void GME_FormUnprintedVoucher(IApplication applicationParm)
        {
            FrmUnprintVoucherConfirm inputfrmEmployeePurchase = new FrmUnprintVoucherConfirm()
            {
                application = applicationParm
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

        public static void GME_FormUpdateCustomer(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            frmUpdateCustomerConfirm inputFrmUpdateCustomer = new frmUpdateCustomerConfirm()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmUpdateCustomer,
                () =>
                {
                    if(inputFrmUpdateCustomer.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormPayCardOfflineApproval(IApplication applicationParm, IPreTriggerResult triggerResultParm, IPosTransaction posTransactionParm)
        {
            frmPayCardOfflineApprovalCode inputFrmPayCardOfflineApproval = new frmPayCardOfflineApprovalCode()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmPayCardOfflineApproval,
                () =>
                {
                    if (inputFrmPayCardOfflineApproval.Confirmed)
                    {
                        if (inputFrmPayCardOfflineApproval.approvalCode == string.Empty)
                        {
                            triggerResultParm.ContinueOperation = false;
                        }
                        else
                        {
                            triggerResultParm.ContinueOperation = true;
                        }
                    }
                    else
                    {
                        triggerResultParm.ContinueOperation = false;
                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormEnrollmentLYBC(IApplication applicationParm, IPosTransaction posTransactionParm)
        {
            frmEnrollmentLYBCConfirm inputFrmEnrollmentLYBC = new frmEnrollmentLYBCConfirm()
            {
                application = applicationParm,
                posTransaction = posTransactionParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputFrmEnrollmentLYBC,
                () =>
                {
                    if (inputFrmEnrollmentLYBC.Confirmed)
                    {
                        
                    }                   
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        public static void GME_FormBirthdayVoucher(IApplication applicationParm, string voucherIdParm)
        {
            GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm inputfrmBirthdayVoucher = new GME_Custom.GME_Propesties.FrmBirthdayVoucherConfirm()
            {
                voucherId = voucherIdParm,

                application = applicationParm

            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmBirthdayVoucher,
                () =>
                {
                    if (inputfrmBirthdayVoucher.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //ADD BY RIZKI for active voucher
        public static void GME_FormItemVoucher(IApplication applicationParm)
        {
            frmItemVoucherConfirm inputfrmItemVoucher = new frmItemVoucherConfirm()
            {
                application = applicationParm
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmItemVoucher,
                () =>
                {
                    if (inputfrmItemVoucher.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Add by Adhi (Form TBSI Voucher Baru)
        public static void GME_FormTBSIVoucherBaru(IApplication applicationParm, string tbsiVoucherIdParm)
        {
            frmTBSIVoucherBaruConfirm inputfrmBirthdayVoucher = new frmTBSIVoucherBaruConfirm()
            {
                tbsiVoucherId = tbsiVoucherIdParm,

                application = applicationParm,

            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmBirthdayVoucher,
                () =>
                {
                    if (inputfrmBirthdayVoucher.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Add by Adhi (Form TBSI Voucher Lama)
        public static void GME_FormTBSIVoucherLama(IApplication applicationParm, string tbsiVoucherIdParm)
        {
            frmTBSIVoucherLamaConfirm inputfrmBirthdayVoucher = new frmTBSIVoucherLamaConfirm()
            {
                tbsiVoucherId = tbsiVoucherIdParm,

                application = applicationParm,

            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmBirthdayVoucher,
                () =>
                {
                    if (inputfrmBirthdayVoucher.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }

        //Add by Adhi (Form Welcome Voucher)
        public static void GME_FormWelcomeVoucher(IApplication applicationParm, string voucherIdParm)
        {
            GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm inputfrmWelcomeVoucher = new GME_Custom.GME_Propesties.FrmWelcomeVoucherConfirm()
            {
                voucherId = voucherIdParm,

                application = applicationParm,

            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(
                inputfrmWelcomeVoucher,
                () =>
                {
                    if (inputfrmWelcomeVoucher.Confirmed)
                    {

                    }
                });

            applicationParm.Services.Interaction.InteractionRequest(request);
        }        
    }
}
