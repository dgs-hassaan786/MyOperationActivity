using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookCheckInAndOut.Utilities;
using BusinessLogic;
using System.Data;
using DBAccess;

namespace BookCheckInAndOut
{
    public partial class CheckOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int selectedBookID = 0;
                if (!String.IsNullOrWhiteSpace(Request.QueryString["bookID"]))
                {
                    selectedBookID = int.Parse(Request.QueryString["bookID"]);
                    lblCheckOutDate.Text = DateTime.Now.ToString();
                    lblReturnDate.Text = Utilities.Utilities.GetDateAfterSpecifiedBusinessDays(15).ToString();
                    DisplayBookDetails(selectedBookID);
                    DisplayBookCheckOutHistory(selectedBookID);
                }
                else
                {
                    Utilities.Utilities.SetPageMessage("The resource which you are trying to access is not available.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }
            }
        }

        /// <summary>
        /// This function is responsible for populating the book history grid.
        /// </summary>
        /// <param name="bookID">Book ID</param>
        private void DisplayBookCheckOutHistory(int bookID)
        {
            BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();
            List<Borrower> borrowers = dbOperations.RetrieveBookCheckOutHistory(bookID);

            HistoryList.DataSource = borrowers;
            HistoryList.DataBind();
        }

        private void DisplayBookDetails(int bookId)
        {
            BusinessLogicDBOperations dbOp = new BusinessLogicDBOperations();
            try
            {
                var bookInfo = dbOp.RetrieveBookDetails(bookId);
                hdnField.Value = bookInfo.ModifiedOn.ToJson();
            }
            catch (Exception ex)
            {
                Utilities.Utilities.SetPageMessage(ex.Message, Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }
        }

        /// <summary>
        /// Check Out button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCheckOut_Click(object sender, EventArgs e)
        {
            int selectedBookID = 0;
            if (!String.IsNullOrWhiteSpace(Request.QueryString["bookID"]))
            {
                selectedBookID = int.Parse(Request.QueryString["bookID"]);
                BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();

                string bookName = txtName.Text;
                string mobileNo = txtMobile.Text;
                string nationalID = txtNationalID.Text;
                DateTime checkOutDate = DateTime.Parse(lblCheckOutDate.Text);
                DateTime returnDate = DateTime.Parse(lblReturnDate.Text);

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

                int result = dbOperations.CheckOut(selectedBookID,
                     bookName,
                     mobileNo,
                     nationalID,
                     checkOutDate,
                     returnDate,
                     dt
                     );
                if (result == 0)
                {
                    Utilities.Utilities.SetPageMessage("There was an error occured. Request can not be fulfil at the current movement.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }

                if (result == 404)
                {
                    Utilities.Utilities.SetPageMessage("Either the book is not available or already checked out", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }

                btnCheckOut.Enabled = false;

                Utilities.Utilities.SetPageMessage("Book has been checked out in the name of " + txtName.Text, Utilities.Utilities.Severity.Info, Page.Master);

                DisplayBookCheckOutHistory(selectedBookID);
            }
            else
            {
                Utilities.Utilities.SetPageMessage("Please select a book.", Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }
        }

    }
}