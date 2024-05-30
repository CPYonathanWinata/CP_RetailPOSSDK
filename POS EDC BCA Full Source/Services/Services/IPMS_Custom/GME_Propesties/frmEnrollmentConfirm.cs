using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Data;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;

namespace GME_Custom.GME_Propesties
{
    public class frmEnrollmentConfirm : Confirmation
    {
        public string cardMemberNum { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int gender { get; set; }
        public string email { get; set; }
        public string phoneNum { get; set;}
        public string dateOfBirth { get; set; }
        public string skinType { get; set; }
        public bool sendEmail { get; set; }
        public bool sendSMS { get; set; }
        public IApplication application { get; set; }
        public IPosTransaction posTransaction { get; set; }
    }
}
