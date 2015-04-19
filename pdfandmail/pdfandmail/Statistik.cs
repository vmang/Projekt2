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
        GeoCoordinate upperLeft;
        GeoCoordinate lowerRight;
        public void setGeoSpace()
        {
            upperLeft = new GeoCoordinate(48.071688, 7.645358);
            lowerRight = new GeoCoordinate(47.901415, 7.938556);
            System.Console.WriteLine(upperLeft.GetDistanceTo(lowerRight));
        }
        
        public void calcTankstationsInSpace()
        {
        Projekt2Entities p2e = new Projekt2Entities();
        var queryResults =
            from f in p2e.Fahrt
            //where f.Endzeit == "London"
            orderby f.Kunde_ID ascending
            select f;

            foreach( Fahrt f in queryResults)
            {
                System.Console.WriteLine(f.Tanksaeule.Tankstelle.Stasse + " : " + upperLeft.GetDistanceTo(new GeoCoordinate((double)f.Tanksaeule.Tankstelle.breitengrad, (double)f.Tanksaeule.Tankstelle.laengengrad)));

            }

        }
        public void carsInSpace() 
        { 

        }

        private void getData()
        {
        Projekt2Entities p2e = new Projekt2Entities();
        var queryResults =
            from f in p2e.Fahrt
            //where f.Endzeit == "London"
            orderby f.Kunde_ID ascending
            select f;

        }
    }
}
