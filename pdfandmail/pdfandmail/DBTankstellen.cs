using pdfandmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSpezifikation
{
    class DBTankstellen
    {       
        public string getTankstellen(string carId)
        {                        
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from t in p2e.Tankstelle select t;


            return null;
        }

        public string getAvailableTankstellen(string message)
        {
            string[] parameter = message.Split(new Char[] { '$' });
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from t in p2e.Tankstelle select t;

            foreach (Tankstelle t in queryResults)
            {
                //überprüfen ob an der Tankstelle eine Tanksäule frei ist
            }

            return null;
        }
    }
}
