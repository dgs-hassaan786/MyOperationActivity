using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using DBAccess;

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
                    Utilities.Utilities.SetPageMessage("The resource which you are trying to access is not available.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }
            }

        }

        private void DisplayBorrowerDeails(int BookID)
        {
            BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();
            Borrower borrower = dbOperations.RetrieveBookBorrowerDetails(BookID);

            if (borrower != null)
            {
                lblName.Text = borrower.Name;
                lblMobile.Text = borrower.MobileNo;
                lblReqReturnDate.Text = borrower.ReturnDate.ToString();
                lblReturnDate.Text = DateTime.Now.ToString();
                hdnField.Value = borrower.Book.ModifiedOn.ToJson();
                double penaltyAmount = CalcultePenaltyAmount(DateTime.Now, borrower.ReturnDate);
                lblPenaltyAmount.Text = String.Format("{0:#,##0.00}", penaltyAmount);

            }
            else
            {
                Utilities.Utilities.SetPageMessage("Book is either already checked in or was not found.", Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }
        }

        private double CalcultePenaltyAmount(DateTime actualReturnDate, DateTime reqReturnedDate)
        {
            if (actualReturnDate <= reqReturnedDate)
            {
                return 0;
            }
            else
            {
                var penalty = 0;
                for (var i = reqReturnedDate.AddDays(1); i <= actualReturnDate; i = i.AddDays(1))
                {
                    if (i.DayOfWeek != DayOfWeek.Friday && i.DayOfWeek != DayOfWeek.Saturday)
                    {
                        penalty += 5;
                    }
                }

                return penalty;
            }
        }

        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
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

                    Utilities.Utilities.SetPageMessage("Either the book is not available or already checked out. Please try to refresh again", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }

                int result = dbOperations.CheckIn(selectedBookID, dt);
                if (result == 0)
                {
                    Utilities.Utilities.SetPageMessage("There was an error occured. Request can not be fulfil at the current movement.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }
                if (result == 404)
                {
                    Utilities.Utilities.SetPageMessage("Either the book is already checked in or was not found.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }

                Utilities.Utilities.SetPageMessage("Book has been checked in successfully.", Utilities.Utilities.Severity.Info, Page.Master);
                btnCheckIn.Enabled = false;
            }
            else
            {
                Utilities.Utilities.SetPageMessage("Book is either already checked in or was not found.", Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }
        }
    }
}