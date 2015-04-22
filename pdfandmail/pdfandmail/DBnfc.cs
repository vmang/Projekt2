using pdfandmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSpezifikation
{
    class DBnfc
    {
        //Grunddaten zur Initialisierung
        public string checkNFC(string message)
        {
            //vorziehen mit allgemein und unterscheidung anhand ersten parameter
            //Extrahieren Der Parameter, an dritter Stelle
            string[] split = message.Split(new Char [] {'%'});
            string[] parameter = split[2].Split(new Char[] {'$'});

            //erster eCar ID
            //zweiter nfc Serial
            Projekt2Entities p2e = new Projekt2Entities();

            var queryResults = from c in p2e.Car select c;

            string messageType = "e";
            foreach(Car c in queryResults){                
                if(c.Car_ID.Equals(parameter[0])){
                    var queRes = from k in p2e.Karte select k where k.Kunde_ID = c.Reservierung.Kunde_ID;
                    foreach (Karte karte in queRes){
                        if(){
                            messageType = "v";
                        }
                    }
                    
                    
                }
            } 


            string[] paraReturn;
            if(messageType.Equals("s")){
                //1.Nachrichtentyp, 2.eCar Byte 3.Ladezustand 4.KM 5.Status
                
            }else {

            }

            string returnMessage = "%";
            return null;
        }
    }
}
