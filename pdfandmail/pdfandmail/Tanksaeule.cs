//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pdfandmail
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tanksaeule
    {
        public Tanksaeule()
        {
            this.Fahrt = new HashSet<Fahrt>();
            this.Fahrt1 = new HashSet<Fahrt>();
        }
    
        public int Tanksaeule_ID { get; set; }
        public int Tankstelle_ID { get; set; }
        public int Tanksaeule_Nr { get; set; }
        public bool Belegt { get; set; }
    
        public virtual Tankstelle Tankstelle { get; set; }
        public virtual ICollection<Fahrt> Fahrt { get; set; }
        public virtual ICollection<Fahrt> Fahrt1 { get; set; }
    }
}
