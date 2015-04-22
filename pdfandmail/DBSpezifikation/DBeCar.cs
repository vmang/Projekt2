using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSpezifikation
{
    class DBeCar
    {
        //Grunddaten zur Initialisierung
        public string initECars(string message)
        {
            //vorziehen mit allgemein und unterscheidung anhand ersten parameter
            //Extrahieren Der Parameter, an dritter Stelle
            string[] split = message.Split(new Char [] {'%'});
            string[] parameter = split[2].Split(new Char[] {'$'});

            //erster eCar ID
            //zweiter eCar Serial
            Projekt2Entities p2e = new Projekt2Entities();


            string[] paraReturn;
            if(){
                //1.Nachrichtentyp, 2.eCar Byte 3.Ladezustand 4.KM 5.Status
                
            }else {

            }

            string returnMessage = "%";
            return null;
        }
    }
}
