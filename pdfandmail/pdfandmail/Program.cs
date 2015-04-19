using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Net.Mail;

namespace pdfandmail
{
    static class Program
    {

        static void Main()
        {
            //Bill b = new Bill();
            //b.getdata();

            Statistik s = new Statistik();
            s.setGeoSpace();
            s.calcTankstationsInSpace();

        }
       
    }
}
