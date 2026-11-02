using System;
using System.Web;

namespace EduX
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            EduShareRepository.Initialize();
        }
    }
}
