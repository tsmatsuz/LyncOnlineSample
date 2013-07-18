using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LyncHelpDeskWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // for this workaround ! (Lync のバグのための対策)
            // http://dotnetslackers.com/IIS/re-399484_Workaround_for_Client_is_not_trusted_Bug_in_Lync_Controls_and_Lync_API_in_Default_Page_of_IIS_Website_Hosting_a_Silverlight_Application.aspx

            //Response.Redirect("ApplicationMain.htm");
        }
    }
}