using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAppServer
{
    public class Options
    {
        public uint Port { get; set; }
        public string WebRoot { get; set; }
        public string RuntimeVersion { get; set; }
        public bool IsAspNetCore { get; set; }

        public string FrameworkWebConfig { get; set; }

        public void Parse(string[] args)
        {
            // defaults
            WebRoot = Path.GetFullPath(".");
            RuntimeVersion = Constants.RuntimeVersion.VersionFourDotZero;
            FrameworkWebConfig = Constants.FrameworkPaths.FourDotZeroWebConfig;
            IsAspNetCore = CheckForAspNetCore();
            
            if (Environment.GetEnvironmentVariable("PORT") != null)
            {
                Console.Out.WriteLine("PORT == {0}", Environment.GetEnvironmentVariable("PORT"));
                Port = uint.Parse(Environment.GetEnvironmentVariable("PORT"));
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-r")
                {
                    WebRoot = args[i + 1];
                }

                if (args[i] == "-v")
                {
                    if (args[i + 1] == "2.0") 
                    {
                        RuntimeVersion = Constants.RuntimeVersion.VersionTwoDotZero;
                        FrameworkWebConfig = Constants.FrameworkPaths.TwoDotZeroWebConfig;
                    }
                   
                }

                if (args[i] != "-p") continue;
                Port = uint.Parse(args[i + 1]);
                
            }

            if (IsAspNetCore)
            {
                RuntimeVersion = Constants.RuntimeVersion.VersionAspNetCore;
            }
        }

        private bool CheckForAspNetCore()
        {
            var webConfig = Path.Combine(WebRoot,"www","web.config");

            if (!File.Exists(webConfig)) return false;

            var doc = new XmlDocument();
            doc.Load(webConfig);
            return doc.SelectSingleNode(Constants.ConfigXPath.WebServer + "/aspNetCore") != null;
        }
    }
}
