using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace BookCheckInAndOut
{
    public partial class Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int selectedBookID = 0;
                if (!String.IsNullOrWhiteSpace(Request.QueryString["bookID"]))
                {

                    selectedBookID = int.Parse(Request.QueryString["bookID"]);
                    DisplayBookDetails(selectedBookID);
                    DisplayBookCheckOutHistory(selectedBookID);
                }
                else
                {

                }
                // to do error message


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
                Book book = dbOp.RetrieveBookDetails(bookId);
                lblBookTitle.Text = book.Title;
                lblISBN.Text = book.ISBN;
                lblPrice.Text = book.CoverPrice.ToString("0.##", CultureInfo.InvariantCulture);
                lblPublishYear.Text = book.PublishYear;
                lblStatus.Text = book.CheckOutStatusDescription;
            }
            catch (Exception ex)
            {
                Utilities.Utilities.SetPageMessage(ex.Message, Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }


        }
    }
}