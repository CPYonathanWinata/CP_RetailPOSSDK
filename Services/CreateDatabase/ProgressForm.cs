/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

//Microsoft Dynamics AX for Retail POS Plug-ins 
//The following project is provided as SAMPLE code. Because this software is "as is," we may not provide support services for it.

using System.Windows.Forms;
using LSRetailPosis;

namespace Microsoft.Dynamics.Retail.Pos.CreateDatabaseService
{
	internal partial class ProgressForm : Form
	{
		public ProgressForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(System.EventArgs e)
		{
			if (!DesignMode)
			{
				this.Text = ApplicationLocalizer.Language.Translate(50600);
				lboxStatusText.Items.Clear();
			}

			base.OnLoad(e);
		}

		public void AddMessage(string message)
		{
			lboxStatusText.Items.Add(message);
			lboxStatusText.SelectedIndex = lboxStatusText.Items.Count - 1;
			lboxStatusText.Refresh();
            // It is ok to use DoEvent here as we don't allow closing this progress form.
            Application.DoEvents();
		}
	}
}