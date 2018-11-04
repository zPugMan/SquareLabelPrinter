using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DYMO.Common;
//using DYMO.DLS.Runtime;
using DYMO.Label.Framework;
using RetailAppWPF.Models;

namespace RetailAppWPF.Services
{
    public class PrintServices : IDisposable
    {
        private static string BARCODE_TXT = "{BARCODE}";
        private Dymo.DymoAddIn dymo;

        public void PrintBarcodeLabel(ProductItem item, int quantity)
        {
            var labelXmlFilePath = Path.Combine(Environment.CurrentDirectory, @"Assets\jbrooker.label");
            string xmlTemplate = File.ReadAllText(labelXmlFilePath, Encoding.UTF8);
            string xmlData = xmlTemplate.Replace(BARCODE_TXT, item.SKU);
            File.WriteAllText(@"c:\temp\test.xml", xmlData);
            string cachePath = Path.Combine(Environment.CurrentDirectory, @"Cache\");

            dymo = new Dymo.DymoAddIn();
            dymo.StartPrintJob();
            dymo.Open(@"c:\temp\test.xml");
            dymo.Print(quantity, false);
            dymo.EndPrintJob();

        }

        public void PrintBarcodeLabel2(ProductItem item, int quantity)
        {
            IFontInfo font = new FontInfo("Arial", 8d, FontStyle.None);

            BarcodeObject barcode = new BarcodeObject();
            barcode.BarcodeSize = BarcodeSize.Small;
            barcode.BarcodeType = BarcodeType.Code39;
            barcode.BarcodeText = item.SKU;
            barcode.HorizontalAlignment = HorizontalAlignment.Center;
            barcode.Name = "BARCODE";
            barcode.Rotation = ObjectRotation.Rotation0;
            barcode.TextPosition = BarcodeTextPosition.Bottom;
            barcode.TextFont = font;
            BarcodeError barError = barcode.Validate();

            var labelXmlFilePath = Path.Combine(Environment.CurrentDirectory, @"Assets\jbrooker-portrait-1.label");
            string xmlTemplate = File.ReadAllText(labelXmlFilePath, Encoding.UTF8);
            ILabel label = Label.OpenXml(xmlTemplate);
            label.SetObjectText("BARCODE", item.SKU);
            label.SetObjectText("TEXT", item.Name);
            label.SetObjectText("COST", item.Price);

            var printers = Framework.GetPrinters();
            if(printers!=null && printers.Count() >0)
            {
                var printer = printers.First();

                ILabelWriterPrintParams parms = new LabelWriterPrintParams();
                parms.Copies = quantity;
                parms.FlowDirection = FlowDirection.LeftToRight;
                parms.PrintQuality = LabelWriterPrintQuality.BarcodeAndGraphics;
                parms.RollSelection = RollSelection.Auto;
                parms.JobTitle = String.Format("{0}-BARCODE", item.SKU);

                PrintJob job = new PrintJob(printer, parms);
                job.AddLabel(label);
                job.Print();

                //label.Print(printer, parms);
                //label.SaveToFile(@"c:\temp\test.xml");
            }
            
        }

        private void SaveFile(string sku, string xml)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Cache\", sku, @"\.xml"), xml);
        }

        public void Dispose()
        {
            //dymo.EndPrintJob();
            //dymo.Save();
            dymo = null;

            //DYMO.Label.Framework.IPrinters printers = DYMO.Label.Framework.Framework.GetPrinters();
            //if(printers!=null && printers.Count() > 0)
            //{
            //    DYMO.Label.Framework.IPrinter printer = printers.First();
            //    printer.CreatePrintJob(null);
            //}

            //if (File.Exists(@"c:\temp\test.xml"))
            //{
            //    File.Delete(@"c:\temp\test.xml");
            //}

        }
    }
}
