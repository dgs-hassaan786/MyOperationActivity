using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.Windows.Media.Imaging;
using System.Threading;

namespace BookCheckInAndOut
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DisplayBooks();
            }


        }

        protected void BtnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(hdnField.Value))
                {
                    Utilities.Utilities.SetPageMessage("Please select the book first.", Utilities.Utilities.Severity.Error, Page.Master);
                    return;
                }

                Response.Redirect("CheckOut.aspx?bookID=" + int.Parse(hdnField.Value));
            }
            catch (ThreadAbortException ex)
            {

            }
        }

        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(hdnField.Value))
            {
                Utilities.Utilities.SetPageMessage("Please select the book first.", Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }

            Response.Redirect("CheckIn.aspx?bookID=" + int.Parse(hdnField.Value));
        }

        protected void BtnDetails_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(hdnField.Value))
            {
                Utilities.Utilities.SetPageMessage("Please select the book first.", Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }

            Response.Redirect("Details.aspx?bookID=" + int.Parse(hdnField.Value));
        }


        private void DisplayBooks()
        {
           

            try
            {
                BusinessLogicDBOperations dbOp = new BusinessLogicDBOperations();
                List<Book> books = dbOp.RetrieveBooksList();

                BooksList.DataSource = books;
                BooksList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.Utilities.SetPageMessage(ex.Message, Utilities.Utilities.Severity.Error, Page.Master);
                return;
            }
        }

    }
}