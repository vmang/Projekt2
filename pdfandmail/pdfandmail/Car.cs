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
    
    public partial class Car
    {
        public Car()
        {
            this.Reservierung = new HashSet<Reservierung>();
            this.Status1 = new HashSet<Status>();
            this.Fahrt = new HashSet<Fahrt>();
        }
    
        public int Car_ID { get; set; }
        public string Seriennummer { get; set; }
        public Nullable<int> Status_ID { get; set; }
        public Nullable<bool> Gesperrt { get; set; }
        public Nullable<bool> SpontaneNutzungGesperrt { get; set; }
        public Nullable<bool> ReservierungGesperrt { get; set; }
    
        public virtual Status Status { get; set; }
        public virtual ICollection<Reservierung> Reservierung { get; set; }
        public virtual ICollection<Status> Status1 { get; set; }
        public virtual ICollection<Fahrt> Fahrt { get; set; }
    }
}
