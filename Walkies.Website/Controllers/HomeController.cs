using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Walkies.Framework.Models.Home;

namespace Walkies.Website.Controllers
{

    [RoutePrefix( "" )]
    public class HomeController : Controller
    {
        [Route( "" )]
        public ActionResult Homepage()
        {
            var model = new HomepageModel();
            return View( model );
        }

        [Route( "booking" )]
        public ActionResult Booking()
        {
            var model = new BookingModel();
            return View( model );
        }

        [Route( "about" )]
        public ActionResult About()
        {
            var model = new AboutModel();
            return View( model );
        }

        [Route( "testimonials" )]
        public ActionResult Testimonials()
        {
            var model = new TestimonialsModel();
            return View( model );
        }

        [Route( "services" )]
        public ActionResult Services()
        {
            var model = new ServicesModel();
            return View( model );
        }

        [Route( "contact" )]
        public ActionResult Contact()
        {
            var model = new ContactModel();
            return View( model );
        }
    }
}