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
using Microsoft.Extensions.Logging;

namespace RetailAppWPF.Services
{
    public class PrintServices : IDisposable
    {
        private static string BARCODE_TXT = "{BARCODE}";
        private Dymo.DymoAddIn dymo;
        private readonly ILogger _log;

        public PrintServices(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<PrintServices>();
        }

        public void PrintBarcodeLabel2(ProductItem item, int quantity)
        {
            IFontInfo font = new FontInfo("Arial", 8d, FontStyle.None);
            _log.LogInformation($"Request to print '{item.Name}' (SKU: {item.SKU}); quantity: {quantity}");

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
            string xmlTemplate = String.Empty;
            try
            {
                xmlTemplate = File.ReadAllText(labelXmlFilePath, Encoding.UTF8);
            }
            catch (FileNotFoundException e)
            {
                _log.LogError($"Failed to load Dymo template: {labelXmlFilePath}", e);
                return;
            }
            catch (Exception e)
            {
                _log.LogError($"Exception caught in loading Dymo template: {labelXmlFilePath}", e);
                return;
            }

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
