using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;
using System.Diagnostics;

namespace MVCUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sw = new Stopwatch();
            sw.Start();

            List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All();

            long elapsedMilliseconds = sw.ElapsedMilliseconds;

            return View(tournaments);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}