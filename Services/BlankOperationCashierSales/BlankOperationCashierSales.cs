using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Forms;
using LSRetailPosis;
using LSRetailPosis.Settings.FunctionalityProfiles;
using Microsoft.Dynamics.Retail.Pos.Contracts;
using Microsoft.Dynamics.Retail.Pos.Contracts.BusinessObjects;
using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
using Microsoft.Dynamics.Retail.Pos.Contracts.Services;

namespace BlankOperationCashierSales
{
    [Export(typeof(IBlankOperations))]
    public sealed class BlankOperationCashierSales : IBlankOperations
    {
        [Import]
        public IApplication Application { get; set; }

        // Get all text through the Translation function in the ApplicationLocalizer
        // TextID's for BlankOperations are reserved at 50700 - 50999

        /// <summary>
        /// Displays an alert message according operation id passed.
        /// </summary>
        /// <param name="operationInfo"></param>
        /// <param name="posTransaction"></param>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Grandfather")]
        public void BlankOperation(IBlankOperationInfo operationInfo, IPosTransaction posTransaction)
        {
            if (operationInfo.OperationId == "2")
            {
                frmCashierTrans formCashier = new frmCashierTrans(posTransaction);
                formCashier.ShowDialog();
            }
        }
    }

}
