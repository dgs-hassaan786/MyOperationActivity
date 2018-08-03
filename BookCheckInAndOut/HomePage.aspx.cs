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
using Foundation.Shared.Utilities;

namespace BookCheckInAndOut
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = false;
                btnDetails.Enabled = false;
                DisplayBooks();
            }
        }

        protected void BtnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(hdnField.Value))
                {
                    Utilities.Instance.SetPageMessage("Please select the book first.", Utilities.Severity.Error, Page.Master);
                    return;
                }

                Response.Redirect("CheckOut.aspx?bookID=" + int.Parse(hdnField.Value));
            }
            catch (Exception ex)
            {
                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
            }
        }

        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(hdnField.Value))
                {
                    Utilities.Instance.SetPageMessage("Please select the book first.", Utilities.Severity.Error, Page.Master);
                    return;
                }

                Response.Redirect("CheckIn.aspx?bookID=" + int.Parse(hdnField.Value));
            }
            catch (Exception ex)
            {

                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
            }
            
        }

        protected void BtnDetails_Click(object sender, EventArgs e)
        {
            try
            {

                if (String.IsNullOrWhiteSpace(hdnField.Value))
                {
                    Utilities.Instance.SetPageMessage("Please select the book first.", Utilities.Severity.Error, Page.Master);
                    return;
                }

                Response.Redirect("Details.aspx?bookID=" + int.Parse(hdnField.Value));
            }
            catch (Exception ex)
            {

                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
            }
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
                Utilities.Instance.SetPageMessage(ex.Message, Utilities.Severity.Error, Page.Master);
                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = false;
                btnDetails.Enabled = false;
                return;
            }
        }

    }
}