using System;
using System.Collections.Generic;
using System.Linq;
using WebMelodyCore.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using WebMelodyCore.OptionClasses;
namespace WebMelodyCore.Controllers
{
    public class MusicController : Controller
    {       
     //   MelodyDB db = new MelodyDB();
        string _examplePath;
        private GeneralOptions _options;
        public static object ConfigurationManager { get; private set; }

        public MusicController(IOptions<GeneralOptions> options)
        {
            _options = options.Value;
            _examplePath = System.IO.Path.Combine(this._options.HostingEnvironment.WebRootPath, this._options.Examples.TrimStart('/').Replace('\\', '/').Replace('/', System.IO.Path.DirectorySeparatorChar));
        }

        public ActionResult Index()
        {
            return View(model:MelodyGeneration.ReadExamples(_examplePath));
        }
        [HttpPost]
        public ActionResult GenerateMelody(string melody, bool jsEnabled)
        {
            if (string.IsNullOrEmpty(melody))
                return View("GenerationError", GenerationError.Empty);
            //If we can connect to the database, then log user input
            //bool writeLog = ConfigurationManager.AppSettings["writeLog"].AsBool();
            //if (writeLog)
            //{
            //    string csn = ConfigurationManager.AppSettings["logDBConnectionStringName"];
            //    var db = new MelodyDB(csn);
            //    Piece mel = new Piece()
            //    {
            //        Title = null,
            //        Text = melody,
            //        UserIP = Request.UserHostAddress
            //    };
            //    db.Pieces.Add(mel);
            //    try
            //    {
            //        db.SaveChanges();
            //    }
            //    catch { }
            //}            
            string dataUrl = null;
            try
            {
                dataUrl = MelodyGeneration.Generate(melody);
            }catch(ArgumentException ex)
            {
                GenerationError error = (GenerationError?) ex.Data["Error"] ?? GenerationError.General;
                return View("GenerationError", error);
            }
            if(dataUrl == null)
                return View("GenerationError", GenerationError.General);
            if (jsEnabled)
                return View((object)dataUrl);
            else
            {
                var t = MelodyGeneration.ReadExamples(_examplePath);
                return View("NonJSGenerateMelody", new NonJSGenerateMelody() { Melody = melody, DataUrl = dataUrl, Examples = new Piece[0] });
            }
        }
        public ActionResult Rules()
        {
            return View("RulesPage");
        }
    }
}