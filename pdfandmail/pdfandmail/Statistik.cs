using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace pdfandmail
    {
    class Statistik
        {

        public void geoSpace()
            {
                GeoCoordinate lo = new GeoCoordinate(48.071688, 7.645358);
                GeoCoordinate ru = new GeoCoordinate(47.901415, 7.938556);
                System.Console.WriteLine(lo.GetDistanceTo(ru));
            }
        }
    }
