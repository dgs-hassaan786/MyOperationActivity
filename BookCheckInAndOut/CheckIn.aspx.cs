using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using DBAccess;
using Foundation.Shared.Utilities;

namespace BookCheckInAndOut
{
    public partial class CheckIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int selectedBookID = 0;
                if (!String.IsNullOrWhiteSpace(Request.QueryString["bookID"]))
                {
                    selectedBookID = int.Parse(Request.QueryString["bookID"]);
                    DisplayBorrowerDeails(selectedBookID);
                }
                else
                {
                    btnCheckIn.Enabled = false;
                    Utilities.Instance.SetPageMessage("The resource which you are trying to access is not available.", Utilities.Severity.Error, Page.Master);
                    return;
                }
            }

        }
        
        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
            btnCheckIn.Enabled = false;
            BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();

            int selectedBookID = 0;
            if (!String.IsNullOrWhiteSpace(Request.QueryString["bookID"]))
            {
                selectedBookID = int.Parse(Request.QueryString["bookID"]);


                DateTime dt;
                try
                {
                    dt = hdnField.Value.FromJson<DateTime>();

                }
                catch (Exception)
                {

                    Utilities.Instance.SetPageMessage("Either the book is not available or already checked out. Please try to refresh again", Utilities.Severity.Error, Page.Master);
                    return;
                }

                int result = dbOperations.CheckIn(selectedBookID, dt);
                switch (result)
                {
                    case 0:
                        {
                            Utilities.Instance.SetPageMessage("There was an error occured. Request can not be fulfil at the current movement.", Utilities.Severity.Error, Page.Master);
                            return;
                        }
                    case 404:
                        {
                            Utilities.Instance.SetPageMessage("Either the book is already checked in or was not found.", Utilities.Severity.Error, Page.Master);
                            return;
                        }
                    default:
                        Utilities.Instance.SetPageMessage("Book has been checked in successfully.", Utilities.Severity.Info, Page.Master);
                        break;
                }
            }
            else
            {
                Utilities.Instance.SetPageMessage("The resource which you are trying to access is not available.", Utilities.Severity.Error, Page.Master);                
            }
        }

        private void DisplayBorrowerDeails(int bookId)
        {
            try
            {

                BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();
                Borrower borrower = dbOperations.RetrieveBookBorrowerDetails(bookId);

                if (borrower != null)
                {
                    lblName.Text = borrower.Name;
                    lblMobile.Text = borrower.MobileNo;
                    lblReqReturnDate.Text = borrower.ReturnDate.ToString();
                    lblReturnDate.Text = DateTime.Now.ToString();
                    hdnField.Value = borrower.Book.ModifiedOn.ToJson();
                    double penaltyAmount = Utilities.Instance.CalcultePenaltyAmount(DateTime.Now, borrower.ReturnDate);
                    lblPenaltyAmount.Text = String.Format("{0:#,##0.00}", penaltyAmount);

                }
                else
                {
                    btnCheckIn.Enabled = false;
                    Utilities.Instance.SetPageMessage("Book is either already checked in or was not found.", Utilities.Severity.Error, Page.Master);
                    return;
                }

            }
            catch (Exception ex)
            {
                btnCheckIn.Enabled = false;
                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
            }

        }
    }
}