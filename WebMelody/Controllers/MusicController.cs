using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebMelody.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GenerateMelody(string melody, bool jsEnabled)
        {
            if (melody == null || melody == string.Empty)
                return View("GenerationError", GenerationError.Empty);
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
                return View((object)MelodyGeneration.Generate(melody));
            else
                return View("NonJSGenerateMelody", new NonJSGenerateMelody() { Melody = melody, DataUrl = dataUrl });
        }
    }
}