using BusinessLogic;
using DBAccess;
using Foundation.Shared.ConfigProvider;
using Foundation.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Web.UI;

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
                    lblReturnDate.Text = Utilities.Instance.GetDateAfterSpecifiedBusinessDays(AppConfigurationManager.Instance.Keys.TotalAllowedDaysToReturnBook).ToString();
                    DisplayBookDetails(selectedBookID);
                    DisplayBookCheckOutHistory(selectedBookID);
                }
                else
                {
                    Utilities.Instance.SetPageMessage("The resource which you are trying to access is not available.", Utilities.Severity.Error, Page.Master);
                    return;
                }
            }
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
                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
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
            btnCheckOut.Enabled = false;
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

                    Utilities.Instance.SetPageMessage("Either the book is not available or already checked out. Please try to refresh again", Utilities.Severity.Error, Page.Master);
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
                switch (result)
                {
                    case 0:
                        {
                            Utilities.Instance.SetPageMessage("There was an error occured. Request can not be fulfil at the current movement.", Utilities.Severity.Error, Page.Master);
                            return;
                        }
                    case 404:
                        {
                            Utilities.Instance.SetPageMessage("Either the book is not available or already checked out", Utilities.Severity.Error, Page.Master);
                            return;
                        }
                    default:
                        {
                            Utilities.Instance.SetPageMessage("Book has been checked out in the name of " + txtName.Text, Utilities.Severity.Info, Page.Master);
                            DisplayBookCheckOutHistory(selectedBookID);
                            break;
                        }
                }
            }
            else
            {
                Utilities.Instance.SetPageMessage("The resource you are trying to access is not available", Utilities.Severity.Error, Page.Master);
            }
        }


        /// <summary>
        /// This function is responsible for populating the book history grid.
        /// </summary>
        /// <param name="bookId">Book ID</param>
        private void DisplayBookCheckOutHistory(int bookId)
        {
            try
            {
                BusinessLogicDBOperations dbOperations = new BusinessLogicDBOperations();
                List<Borrower> borrowers = dbOperations.RetrieveBookCheckOutHistory(bookId);

                HistoryList.DataSource = borrowers;
                HistoryList.DataBind();
            }
            catch (Exception ex)
            {

                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
            }

        }

    }
}