/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

namespace Microsoft.Dynamics.Retail.Pos.Loyalty
{
    using System;
    using System.Windows.Forms;
    using LSRetailPosis;
    using LSRetailPosis.POSProcesses;
    using LSRetailPosis.Transaction;
    using Microsoft.Dynamics.Retail.Notification.Contracts;
    using Microsoft.Dynamics.Retail.Pos.Contracts.DataEntity;
    using Microsoft.Dynamics.Retail.Pos.Contracts.Services;
    using Microsoft.Dynamics.Retail.Pos.Contracts.UI;
    using Microsoft.Dynamics.Retail.Pos.DataEntity.Loyalty;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Loyalty GUI class. 
    /// </summary>
    internal sealed class LoyaltyGUI
    {
        /// <summary>
        /// Prompts user to enter loyalty card number.
        /// </summary>
        /// <returns>Card info if user pressed OK, else null.</returns>
        public static ICardInfo GetCardInfo()
        {
            ICardInfo cardInfo = null;
            using (frmInputNumpad inputDialog = new frmInputNumpad(true, true))
            {
                inputDialog.EntryTypes = NumpadEntryTypes.AlphaNumeric;
                inputDialog.PromptText = ApplicationLocalizer.Language.Translate(50056);   //Loyalty card number
                inputDialog.Text = ApplicationLocalizer.Language.Translate(50062); // Add loyalty card
                POSFormsManager.ShowPOSForm(inputDialog);
                DialogResult result = inputDialog.DialogResult;
                
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    cardInfo = LoyaltyHelper.Instance.Application.BusinessLogic.Utility.CreateCardInfo();
                    cardInfo.CardEntryType = CardEntryTypes.MANUALLY_ENTERED;
                    cardInfo.CardNumber = inputDialog.InputText;
                    cardInfo.CardType = CardTypes.LoyaltyCard;
                    LoyaltyHelper.Instance.Application.Services.Card.GetCardType(ref cardInfo);
                }
            }

            return cardInfo;
        }

        /// <summary>
        /// Prompts user to enter new loyalty card number and optionally select customer.
        /// </summary>
        /// <param name="retailTransaction">Retail transaction</param>
        /// <returns>Loyalty card.</returns>
        public static LoyaltyCard IssueLoyaltyCard(IRetailTransaction retailTransaction)
        {
            RetailTransaction transaction = retailTransaction as RetailTransaction;

            if (retailTransaction == null)
            {
                throw new ArgumentNullException("retailTransaction");
            }

            LoyaltyCardIssuedConfirmation loyaltyCardIssuedConfirmation = new LoyaltyCardIssuedConfirmation()
            {
                PosTransaction = retailTransaction
            };

            bool confirmed = false;
            string loyaltyCardNumber = string.Empty;

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(loyaltyCardIssuedConfirmation, () =>
            {
                confirmed = loyaltyCardIssuedConfirmation.Confirmed;
                loyaltyCardNumber = loyaltyCardIssuedConfirmation.LoyaltyCardNumber;
            });

            LoyaltyHelper.Instance.Application.Services.Interaction.InteractionRequest(request);

            LoyaltyCard loyaltyCard = null;
            
            if (confirmed)
            {
                loyaltyCard = LoyaltyCard.GetInstance();
                loyaltyCard.Number = loyaltyCardNumber;
                loyaltyCard.TenderType = LoyaltyCardTenderType.AsCardTender;

                if (transaction.Customer != null && !transaction.Customer.IsEmptyCustomer())
                {
                    loyaltyCard.PartyNumber = transaction.Customer.PartyNumber;
                }
            }

            return loyaltyCard;
        }

        /// <summary>
        /// Prompts question.
        /// </summary>
        /// <param name="messageId">Question message id.</param>
        /// <returns>True, if user accepted.</returns>
        public static bool PromptQuestion(int messageId)
        {
            bool result = false;

            using (frmMessage dialog = new frmMessage(messageId, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
				POSFormsManager.ShowPOSForm(dialog);
                result = dialog.DialogResult == System.Windows.Forms.DialogResult.Yes;
            }

            return result;
        }

        /// <summary>
        /// Prompts information message.
        /// </summary>
        /// <param name="messageId">Information message id.</param>
        public static void PromptInfo(int messageId)
        {
            using (frmMessage dialog = new frmMessage(messageId)) 
            {
                POSFormsManager.ShowPOSForm(dialog);
            }
        }

        /// <summary>
        /// Prompts error message.
        /// </summary>
        /// <param name="messageId">Error message id.</param>
        public static void PromptError(int messageId)
        {
            using (frmMessage dialog = new frmMessage(messageId,MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                POSFormsManager.ShowPOSForm(dialog);
            }
        }

        /// <summary>
        /// Prompts error message.
        /// </summary>
        /// <param name="messageId">Error message.</param>
        public static void PromptError(string message)
        {
            using (frmMessage dialog = new frmMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                POSFormsManager.ShowPOSForm(dialog);
            }
        }
        /// <summary>
        /// Prompts information message and executes action.
        /// </summary>
        /// <param name="messageId">Information message id.</param>
        /// <param name="execute">Action.</param>
        public static void PromptInfoAndExecute(int messageId, Action execute)
        {
            using (LSRetailPosis.POSControls.Touch.frmMessage popupDialog = new LSRetailPosis.POSControls.Touch.frmMessage(messageId, LSRetailPosis.POSControls.Touch.LSPosMessageTypeButton.NoButtons, MessageBoxIcon.Information))
            {
                POSFormsManager.ShowPOSModelessForm(popupDialog);
                if (execute != null)
                {
                    execute.Invoke();
                }
            }
        }

        public static ICardInfo PayByLoyalty(ICardInfo cardInfo)
        {
            LoyaltyCardPayConfirmation loyaltyCardPayConfirmation = new LoyaltyCardPayConfirmation()
            {
                LoyaltyCardNumber = cardInfo.CardNumber,
                BalanceAmount = cardInfo.Amount,
                CurrencyCode = LSRetailPosis.Settings.ApplicationSettings.Terminal.StoreCurrency
            };

            InteractionRequestedEventArgs request = new InteractionRequestedEventArgs(loyaltyCardPayConfirmation, () =>
            {
                if (loyaltyCardPayConfirmation.Confirmed)
                {
                    cardInfo.CardNumber = loyaltyCardPayConfirmation.LoyaltyCardNumber;
                    cardInfo.Amount = loyaltyCardPayConfirmation.RegisteredAmount;
                }
                else
                {
                    cardInfo = null;
                }
            });

            LoyaltyHelper.Instance.Application.Services.Interaction.InteractionRequest(request);

            return cardInfo;
        }
    }
}
