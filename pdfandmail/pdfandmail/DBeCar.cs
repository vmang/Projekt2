using pdfandmail;
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

            var queryResults = from c in p2e.Car select c;

            //überprüfen ob eCar id vorhanden, und hat keinen oder die übergebene serial zugewiesen
            string messageType = "e";
            int errorCode = 1;
            foreach(Car c in queryResults){
                if(c.Car_ID.Equals(parameter[0])){
                    if(c.Seriennummer.Equals(parameter[1])){
                        messageType = "s";
                        break;
                    }
                    else if (c.Seriennummer.Equals(null))
                    {
                        c.Seriennummer = parameter[1];
                        messageType = "s";
                        break;
                    }
                    else
                    {
                        errorCode = 2;
                    }
                }
            } 
            
            if(messageType.Equals("s")){
                //1.Nachrichtentyp, 2.eCar Byte 3.Ladezustand 4.KM 5.Status
                
            }else {

            }

            return null;
        }

        public string checkReservation(string carId)
        {                        
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from c in p2e.Car select c;

            foreach (Car c in queryResults)
            {
                if (c.Car_ID.Equals(carId))
                {
                    //überprüfen ob Reservierung vorhanden für
                    //falls ja, reservierungsID, reservierungsstatus, KundenID, Start u End TS ID, Start u End Zeitpunkt
                    //falls nein, e 10
                }
            } 

            return null;
        }

        public string carStatus(string message)
        {
            //1. id 2. status 3. Alarm t/f 4. Standort 5. km 6.Ladezustand
            string[] parameter = message.Split(new Char[] { '$' });
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from c in p2e.Car select c;

            foreach (Car c in queryResults)
            {
                if (c.Car_ID.Equals(parameter[0]))
                {
                    //überprüfen ob status und alarm sich geändert haben
                }
            }

            return null;
        }

        public string carFahrtstatus(string message)
        {
            //1. id 2. startzeit 3. endzeit 4. Start Tankstellen ID 5. Ende 6.Start km 7. ende 8. kunde id 9. rese id
            string[] parameter = message.Split(new Char[] { '$' });
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from c in p2e.Car select c;

            foreach (Car c in queryResults)
            {
                if (c.Car_ID.Equals(parameter[0]))
                {
                    //neue daten übernehmen in db vor/nach fahrt
                    //bestätigung
                }
            }

            return null;
        }
    }
}
