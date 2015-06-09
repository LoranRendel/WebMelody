using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMelody.Models;
namespace WebMelody.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        MelodyDB db = new MelodyDB();
        public ActionResult Index()
        {
            return View(MelodyGeneration.ReadExamples(this.HttpContext.ApplicationInstance));
        }
        [HttpPost]
        public ActionResult GenerateMelody(string melody, bool jsEnabled)
        {
            if (melody == null || melody == string.Empty)
                return View("GenerationError", GenerationError.Empty);
            Piece mel = new Piece()
            {
                Title = null,
                Text = melody,
                UserIP = Request.UserHostAddress
            };
            db.Pieces.Add(mel);
            db.SaveChanges();
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
                var t = MelodyGeneration.ReadExamples(this.HttpContext.ApplicationInstance);
                return View("NonJSGenerateMelody", new NonJSGenerateMelody() { Melody = melody, DataUrl = dataUrl, Examples = t });
            }
        }
        public ActionResult Rules()
        {
            return View("RulesPage");
        }
    }
}