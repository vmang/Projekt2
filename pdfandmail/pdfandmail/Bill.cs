using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Net.Mail;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Globalization;

namespace pdfandmail
    {
    class Bill
        {

        public void getdata()
            {
            int lastKundenId = 0;
            string lastKundenMail = null;
            string fileName = null;

            Document doc = null;
            Projekt2Entities p2e = new Projekt2Entities();
            // query all contacts
            var queryResults =
                from f in p2e.Fahrt
                //where f.Endzeit == "London"
                orderby f.Kunde_ID ascending
                select f;


            foreach (Fahrt f in queryResults)
                {
                if (lastKundenId == f.Kunde_ID)
                    {
                    //write into next row of the current table
                    fillContent(f);
                    }
                else
                    {
                    if (doc != null)
                        {
                        fileName = lastKundenId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
                        saveDocument(fileName);
                        sendMail(fileName, lastKundenMail);
                        }
                    doc = createDocument(f);
                    fillContent(f);
                    //Start new PDF and send the old one via mail              
                    }
                lastKundenId = (int)f.Kunde_ID;
                lastKundenMail = f.Kunde.Email;
                }
            //Save the last PDF and Send the last mail
            fileName = lastKundenId + "_" + DateTime.Now.ToShortDateString() + ".pdf";
            saveDocument(fileName);
            sendMail(fileName, lastKundenMail);
            }
        void sendMail(String pdfPath, String emailAddress)
            {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ecar.Rechnung@gmail.com"); //Absender 
            mail.To.Add(emailAddress); //Empfänger 
            mail.Subject = "Ihre E-Car Rechnung vom " + DateTime.Now.Date.ToShortDateString();
            mail.Body = "Sehr geehrter E-Car Nutzer,\n\nIhre Rechnung für den letzten Monat befindet sich im Anhang. Der Betrag wird automatisch innerhalb der nächsten Wochen von Ihrem Konto abgebucht.\nFalls sich Ihre Kontodaten geändert haben oder Sie Fragen bezüglich der Rechnung haben, wenden Sie sich bitte an unsere Hotline unter der Nummer 555-1234-5678\n\nVielen Dank und freundliche Grüße\nIhre E-Car Team\n\n-----------------------------------\nDiese Email wurde elektronisch generiert und ist daher ohne Unterschrift gültig.";
            mail.Attachments.Add(new Attachment(@pdfPath));


            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("ecar.Rechnung@gmail.com", "123Pass123");//Anmeldedaten für den SMTP Server 


            client.Send(mail); //Senden 

            }

        Table table;
        Document document;
        double totalPrice;
        void saveDocument(String filename)
            {
            finishTable();
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;

            renderer.RenderDocument();

            renderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
            }

        void finishTable()
            {
            // Add an invisible row as a space line to the table
            Row row = this.table.AddRow();
            row.Borders.Visible = false;
            // Add the total price row
            row = this.table.AddRow();
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Netto Preis");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(totalPrice.ToString("0.00") + " €");

            // Add the VAT row
            row = this.table.AddRow();
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Mehrwertsteuer (19%)");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph((0.19 * totalPrice).ToString("0.00") + " €");


            // Add the total due row
            row = this.table.AddRow();
            row.Cells[0].AddParagraph("Brutto Preis");
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            totalPrice += 0.19 * totalPrice;
            row.Cells[5].AddParagraph(totalPrice.ToString("0.00") + " €");

            // Set the borders of the specified cell range
            this.table.SetEdge(5, this.table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);
            }

        public Document createDocument(Fahrt f)
            {
            totalPrice = 0;
            // Create a new MigraDoc document
            document = new Document();
            document.Info.Title = "Rechnung der eCar GmbH";
            document.Info.Subject = "Rechnung für " + f.Kunde.Vorname + " " + f.Kunde.Name;
            document.Info.Author = "Vincent Mang";

            defineStyles();

            createPage(f);

            return document;
            }
        void defineStyles()
            {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", MigraDoc.DocumentObjectModel.TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
            }
        void createPage(Fahrt f)
            {
            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();
            TextFrame addressFrame;
            TextFrame textFrame;
            TextFrame header;
            // Put a logo in the header
            Image image = section.Headers.Primary.AddImage("../../ecar_logo.png");
            image.Height = "2.0cm";
            image.Width = "5 cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;
            string footer = "eCar GmbH · Badstr. 42 · 77654 Offenburg · Deutschland";
            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText(footer);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            // Create Header 
            header = section.AddTextFrame();
            header.Width = "5 cm";
            Paragraph h = header.AddParagraph("eCar GmbH");
            h.Format.Font.Name = "Times New Roman";
            h.Format.Font.Size = 26;
            h.Format.SpaceAfter = 4;
            h.Format.Font.Bold = true;
            h.Format.Alignment = ParagraphAlignment.Center;

            //Create textframe
            textFrame = section.AddTextFrame();

            textFrame.Top = "20 cm";
            textFrame.Left = ShapePosition.Left;
            textFrame.RelativeHorizontal = RelativeHorizontal.Column;
            textFrame.RelativeVertical = RelativeVertical.Page;
            textFrame.Width = "20 cm";
            Paragraph rt = textFrame.AddParagraph("Der o.g. Gesamtbetrag wird Ihnen in den nächsten Tagen von folgendem Konto abgebucht:");
            rt.Format.Font.Name = "Times New Roman";
            rt.Format.Font.Size = 12;
            rt.Format.SpaceAfter = 4;
            rt.AddLineBreak();
            rt.AddLineBreak();
            rt.AddText("Kontoinhaber:\t" + f.Kunde.Vorname + " " + f.Kunde.Name);
            rt.AddLineBreak();
            rt.AddText("BIC:\t\t" + f.Kunde.Bank.BIC);
            rt.AddLineBreak();
            rt.AddText("IBAN:\t\t" + f.Kunde.Bank.IBAN);


            // Create the text frame for the address
            addressFrame = section.AddTextFrame();
            addressFrame.Height = "3.0cm";
            addressFrame.Width = "7.0cm";
            addressFrame.Left = ShapePosition.Left;
            addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            addressFrame.Top = "5.0cm";
            addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            paragraph = addressFrame.AddParagraph(footer);
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Fill address in address text frame
            Paragraph pg = addressFrame.AddParagraph();
            pg.AddText(f.Kunde.Vorname + " " + f.Kunde.Name);
            pg.AddLineBreak();
            pg.AddText(f.Kunde.Adresse.Strasse + " " + f.Kunde.Adresse.Hausnummer);
            pg.AddLineBreak();
            pg.AddText(f.Kunde.Adresse.PLZ + " " + f.Kunde.Adresse.Ort);

            // Add the print date field
            CultureInfo ci = new CultureInfo("de-DE");

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("eCar Rechnung für den Monat " + ci.DateTimeFormat.MonthNames[DateTime.Now.AddMonths(-2).Month], TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Offenburg, ");
            paragraph.AddDateField("dd.MM.yyyy");

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns 16cm 
            Column column = this.table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = this.table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Color.FromRgbColor((byte)255, Color.Parse("0xb2b9be"));
            row.Cells[0].AddParagraph("Fahrt-ID");
            row.Cells[1].AddParagraph("Kilometer");
            row.Cells[2].AddParagraph("Stunden");
            row.Cells[3].AddParagraph("Preis KM");
            row.Cells[4].AddParagraph("Preis Stunden");
            row.Cells[5].AddParagraph("Preis pro Fahrt");



            this.table.SetEdge(0, 0, 6, 1, Edge.Box, MigraDoc.DocumentObjectModel.BorderStyle.Single, 0.75, Color.Empty);
            }

        void fillContent(Fahrt f)
            {
            int km = (int)(f.End_KM - f.Start_KM);
            int stunden = (int)Math.Ceiling(((TimeSpan)(f.Endzeit - f.Startzeit)).TotalHours);
            double faktorKm = 0.3;
            double faktorStunde = 0.75;
            double preisKm = km * faktorKm;
            double preisStunde = stunden * faktorStunde;
            double preisFahrt = preisKm + preisStunde;


            // Each item fills two rows
            Row row1 = this.table.AddRow();
            row1.TopPadding = 1.5;
            row1.Format.Alignment = ParagraphAlignment.Center;
            row1.Cells[0].AddParagraph(f.Fahrt_ID.ToString());
            row1.Cells[1].AddParagraph(km.ToString());
            row1.Cells[2].AddParagraph(stunden.ToString());
            row1.Cells[3].AddParagraph(preisKm.ToString("0.00") + " €");
            row1.Cells[4].AddParagraph(preisStunde.ToString("0.00") + " €");
            row1.Cells[5].AddParagraph(preisFahrt.ToString("0.00") + " €");

            totalPrice = totalPrice + preisFahrt;
            this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);

            }

        }

    }