using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMelody.Models;
using System.Configuration;
using System.Globalization;
using System.Web.WebPages;

namespace WebMelody.Controllers
{
    public class MusicController : Controller
    {       
     //   MelodyDB db = new MelodyDB();
        string examplePath = ConfigurationManager.AppSettings["examlpeTunePath"].ToString();       
        public ActionResult Index()
        {
            
            return View(MelodyGeneration.ReadExamples(Server.MapPath(examplePath)));
            
        }
        [HttpPost]
        public ActionResult GenerateMelody(string melody, bool jsEnabled)
        {
            if (melody == null || melody == string.Empty)
                return View("GenerationError", GenerationError.Empty);
            //If we can connect to the database, then log user input
            bool writeLog = ConfigurationManager.AppSettings["writeLog"].AsBool();
            if (writeLog)
            {
                string csn = ConfigurationManager.AppSettings["logDBConnectionStringName"];
                var db = new MelodyDB(csn);
                Piece mel = new Piece()
                {
                    Title = null,
                    Text = melody,
                    UserIP = Request.UserHostAddress
                };
                db.Pieces.Add(mel);
                try
                {
                    db.SaveChanges();
                }
                catch { }
            }            
            string dataUrl = null;
            try
            {
                dataUrl = MelodyGeneration.Generate(melody);
            }catch(ArgumentException ex)
            {
                GenerationError error = ex.Data["Error"] == null ? GenerationError.General : (GenerationError)ex.Data["Error"];
                return View("GenerationError", error);
            }
            if(dataUrl == null)
                return View("GenerationError", GenerationError.General);
            if (jsEnabled)
                return View((object)dataUrl);
            else
            {
                var t = MelodyGeneration.ReadExamples(Server.MapPath(examplePath));
                return View("NonJSGenerateMelody", new NonJSGenerateMelody() { Melody = melody, DataUrl = dataUrl, Examples = t });
            }
        }
        public ActionResult Rules()
        {
            return View("RulesPage");
        }
    }
}