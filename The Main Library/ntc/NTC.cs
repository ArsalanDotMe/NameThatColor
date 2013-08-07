using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace NameThatColor
{
    /// <summary>
    /// Main class.
    /// </summary>
    public class NTC
    {
        private XElement root;
        private XElement mainx;
        /// <summary>
        /// Default constructor for the class.
        /// </summary>
        public NTC()
        {
            System.IO.Stream xmlfile;
            xmlfile = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameThatColor.names.xml");
            root = XElement.Load(xmlfile);
            xmlfile = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameThatColor.main.xml");
            mainx = XElement.Load(xmlfile);
        }

        private double GetDistance(double r1, double g1, double b1, double r2, double g2, double b2)
        {
            return Math.Sqrt(Math.Pow(r1-r2, 2) + Math.Pow(g1-g2, 2) + Math.Pow(b1-b2, 2));
        }
        
        /// <summary>
        /// Gets the name of the closest matching color.
        /// </summary>
        /// <param name="R">Value of Red. Ranges from 0x00 to 0xFF</param>
        /// <param name="G">Value of Green. Ranges from 0x00 to 0xFF</param>
        /// <param name="B">Value of Blue. Ranges from 0x00 to 0xFF</param>
        /// <returns></returns>
        public string getColorName(byte R, byte G, byte B)
        {
            string name = string.Empty;
            double mindist = (from dist in root.Descendants("color") select GetDistance((double)R,(double)G,(double)B,
                                  int.Parse((string)dist.Attribute("R"), System.Globalization.NumberStyles.AllowHexSpecifier),
                                  int.Parse((string)dist.Attribute("G"), System.Globalization.NumberStyles.AllowHexSpecifier),
                                  int.Parse((string)dist.Attribute("B"), System.Globalization.NumberStyles.AllowHexSpecifier)
                                  )).Min();
            name = (from dist in root.Descendants("color") 
                    where GetDistance((double)R, (double)G, (double)B,
                        int.Parse((string)dist.Attribute("R"), System.Globalization.NumberStyles.AllowHexSpecifier),
                        int.Parse((string)dist.Attribute("G"), System.Globalization.NumberStyles.AllowHexSpecifier),
                        int.Parse((string)dist.Attribute("B"), System.Globalization.NumberStyles.AllowHexSpecifier)
                        ) == mindist select dist.Value).FirstOrDefault();
            
            return name;
        }
        public string getMainColorName(byte R, byte G, byte B)
        {
            string name = string.Empty;
            double mindist = (from dist in mainx.Descendants("color")
                              select GetDistance((double)R, (double)G, (double)B,
                                  int.Parse((string)dist.Attribute("R"), System.Globalization.NumberStyles.AllowHexSpecifier),
                                  int.Parse((string)dist.Attribute("G"), System.Globalization.NumberStyles.AllowHexSpecifier),
                                  int.Parse((string)dist.Attribute("B"), System.Globalization.NumberStyles.AllowHexSpecifier)
                                  )).Min();
            name = (from dist in mainx.Descendants("color")
                    where GetDistance((double)R, (double)G, (double)B,
                        int.Parse((string)dist.Attribute("R"), System.Globalization.NumberStyles.AllowHexSpecifier),
                        int.Parse((string)dist.Attribute("G"), System.Globalization.NumberStyles.AllowHexSpecifier),
                        int.Parse((string)dist.Attribute("B"), System.Globalization.NumberStyles.AllowHexSpecifier)
                        ) == mindist
                    select dist.Value).FirstOrDefault();

            return name;
        }
    }
}
