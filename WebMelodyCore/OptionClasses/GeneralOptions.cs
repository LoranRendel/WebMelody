using System;
using Microsoft.AspNetCore.Hosting;

namespace WebMelodyCore.OptionClasses
{
    public class GeneralOptions
    {
        public IHostingEnvironment HostingEnvironment { get; set; }
        public string Examples { get; set; }
    }
}
