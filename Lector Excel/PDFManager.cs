using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lector_Excel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;

namespace Reader_347
{
    /// <summary>
    /// Clase del gestor de archivos PDF.
    /// </summary>
    public class PDFManager
    {
        string DestinationPath;
        /// <summary>
        /// Inicializa una nueva instancia de <c>PDFManager</c>.
        /// </summary>
        /// <param name="destination"></param>
        public PDFManager(string destination)
        {
            this.DestinationPath = destination;
        }

        //Handles exporting up to 6 declareds to a pdf.
        //  * Due to Pdfsharp limitations, 6 is currently the max amount. (Change library in the future?)
        /// <summary>
        /// Exporta los datos recibidos a un PDF.
        /// </summary>
        /// <param name="type1">Los datos del registro de tipo 1.</param>
        /// <param name="declareds">Los datos de los declarados.</param>
        /// <returns>True si la exportación se produjo con éxito, de lo contrario false.</returns>
        public bool ExportToPDFDraft(List<string> type1, List<Declared> declareds)
        {
            try
            {
                if(declareds.Count < 1)
                {
                    throw new DeclaredAmountException(declareds.Count);
                }

                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                Debug.WriteLine("Working in " + Directory.GetCurrentDirectory());
                //File.WriteAllBytes(DestinationPath, );
                File.Copy(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources/Modelo347-Editable.pdf"), DestinationPath,true);

                PdfDocument pdf = PdfReader.Open(DestinationPath);
                PdfAcroForm forms = pdf.AcroForm;

                if (pdf.AcroForm.Elements.ContainsKey("/NeedAppearances") == false)
                    pdf.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));
                else
                    pdf.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);

                //Set registry type 1 fields
                {
                    SetPdfTextFieldValue("untitled68", type1[1], ref forms); //Declarant NIF
                    SetPdfTextFieldValue("untitled69", type1[4], ref forms); //Declarant phone
                    SetPdfTextFieldValue("untitled70", type1[2], ref forms); //Declarant Name
                    SetPdfTextFieldValue("untitled71", type1[14], ref forms); //NIF Legal Rep
                    SetPdfTextFieldValue("untitled72", type1[0], ref forms); //Exercise
                    SetPdfTextFieldValue("untitled73", type1[9], ref forms); //Previous declaration ID
                    SetPdfCheckBoxValue("untitled74", (type1[7].Equals("C") ? true : false), ref forms); //(Checkbox) Comp declaration 1
                    SetPdfCheckBoxValue("untitled75", (type1[7].Equals("C") ? true : false), ref forms); //(Checkbox) Comp declaration 2
                    SetPdfCheckBoxValue("untitled76", (type1[7].Equals("S") ? true : false), ref forms); //(Checkbox) Sust declaration
                    SetPdfTextFieldValue("untitled77", type1[10], ref forms); //Total entities
                    SetPdfTextFieldValue("untitled78", type1[11], ref forms); //Total anual money
                    SetPdfTextFieldValue("untitled79", type1[12], ref forms); //Total properties
                    SetPdfTextFieldValue("untitled80", type1[13], ref forms); //Total money rental
                    SetPdfTextFieldValue("untitled81", DateTime.Today.ToLongDateString(), ref forms); //Date
                    SetPdfTextFieldValue("untitled82", type1[5], ref forms); //Signature (Relation Name placeholder)

                    SetPdfTextFieldValue("untitled14", type1[1], ref forms);
                    SetPdfTextFieldValue("untitled15", type1[0], ref forms);
                    SetPdfTextFieldValue("untitled16", "1", ref forms);
                    SetPdfTextFieldValue("untitled17", declareds.Count <= 3 ? "1" : "2", ref forms);

                    if(declareds.Count > 3)
                    {
                        SetPdfTextFieldValue("untitled135", type1[1], ref forms);
                        SetPdfTextFieldValue("untitled136", type1[0], ref forms);
                        SetPdfTextFieldValue("untitled137", "2", ref forms);
                        SetPdfTextFieldValue("untitled138", "2", ref forms);
                    }
                }

                //Set registry type 2 fields

                int count = 0;
                foreach (Declared declared in declareds)
                {
                    switch (declareds.IndexOf(declared))
                    {
                        case 0:
                            SetPdfTextFieldValue("untitled18", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled20", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled21", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled22", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled23", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled24", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled32", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled25", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled26", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled30", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled33", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled31", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled35", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled36", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled37", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled40", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled38", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled41", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled39", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled42", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled19", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled27", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled28", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled29", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled34", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                        case 1:
                            SetPdfTextFieldValue("untitled84", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled86", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled87", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled88", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled89", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled90", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled98", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled91", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled92", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled96", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled99", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled97", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled101", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled102", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled103", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled106", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled104", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled107", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled105", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled108", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled85", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled93", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled94", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled95", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled100", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                        case 2:
                            SetPdfTextFieldValue("untitled109", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled111", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled112", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled113", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled114", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled115", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled123", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled116", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled117", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled121", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled124", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled122", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled126", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled127", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled128", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled131", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled129", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled132", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled130", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled133", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled110", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled118", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled119", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled120", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled125", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                        case 3:
                            SetPdfTextFieldValue("untitled43", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled45", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled46", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled47", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled48", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled49", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled57", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled50", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled51", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled55", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled58", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled56", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled60", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled65", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled61", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled66", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled62", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled67", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled63", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled64", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled44", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled52", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled53", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled54", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled59", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                        case 4:
                            SetPdfTextFieldValue("untitled139", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled141", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled142", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled143", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled144", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled145", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled153", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled146", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled147", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled151", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled154", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled152", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled156", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled157", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled158", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled161", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled159", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled162", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled160", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled163", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled170", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled148", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled149", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled150", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled155", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                        case 5:
                            SetPdfTextFieldValue("untitled164", declared.declaredData["DeclaredNIF"], ref forms);
                            SetPdfTextFieldValue("untitled166", declared.declaredData["LegalRepNIF"], ref forms);
                            SetPdfTextFieldValue("untitled167", declared.declaredData["DeclaredName"], ref forms);

                            SetPdfTextFieldValue("untitled168", declared.declaredData["ProvinceCode"], ref forms);
                            SetPdfTextFieldValue("untitled169", declared.declaredData["CountryCode"], ref forms);

                            SetPdfTextFieldValue("untitled170", declared.declaredData["OpKey"], ref forms);

                            SetPdfTextFieldValue("untitled178", declared.declaredData["AnualMoney"], ref forms);
                            SetPdfTextFieldValue("untitled171", declared.declaredData["OpInsurance"], ref forms);
                            SetPdfTextFieldValue("untitled172", declared.declaredData["LocalBusinessLease"], ref forms);

                            SetPdfTextFieldValue("untitled176", declared.declaredData["TotalMoney"], ref forms);
                            SetPdfTextFieldValue("untitled179", declared.declaredData["AnualPropertyMoney"], ref forms);

                            SetPdfTextFieldValue("untitled177", declared.declaredData["Exercise"], ref forms);

                            SetPdfTextFieldValue("untitled181", declared.declaredData["TrimestralOp1"], ref forms);
                            SetPdfTextFieldValue("untitled185", declared.declaredData["AnualPropertyIVAOp1"], ref forms);

                            SetPdfTextFieldValue("untitled182", declared.declaredData["TrimestralOp2"], ref forms);
                            SetPdfTextFieldValue("untitled186", declared.declaredData["AnualPropertyIVAOp2"], ref forms);

                            SetPdfTextFieldValue("untitled183", declared.declaredData["TrimestralOp3"], ref forms);
                            SetPdfTextFieldValue("untitled187", declared.declaredData["AnualPropertyIVAOp3"], ref forms);

                            SetPdfTextFieldValue("untitled184", declared.declaredData["TrimestralOp4"], ref forms);
                            SetPdfTextFieldValue("untitled188", declared.declaredData["AnualPropertyIVAOp4"], ref forms);

                            SetPdfTextFieldValue("untitled165", declared.declaredData["CommunityOpNIF"], ref forms);

                            SetPdfTextFieldValue("untitled173", declared.declaredData["OpIVA"], ref forms);
                            SetPdfTextFieldValue("untitled174", declared.declaredData["OpPassive"], ref forms);
                            SetPdfTextFieldValue("untitled175", declared.declaredData["OpCustoms"], ref forms);

                            SetPdfTextFieldValue("untitled180", declared.declaredData["AnualOpIVA"], ref forms);
                            break;
                    }

                    count++;
                    if(count == 6)
                    {
                        break;
                    }
                }

                pdf.Save(DestinationPath);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                MessageBoxResult msg = MessageBox.Show("Ha ocurrido un error. La exportación se interrumpirá.\nCódigo del error: " + e, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Accede al campo de texto del PDF y lo modifica.
        /// </summary>
        /// <param name="fieldName">El nombre del campo.</param>
        /// <param name="value">El valor que se quiere asignar al campo.</param>
        /// <param name="formList">La lista de campos del PDF.</param>
        private void SetPdfTextFieldValue(string fieldName, string value, ref PdfAcroForm formList)
        {
            if(formList.Fields[fieldName] != null)
            {
                PdfTextField txtfield = formList.Fields[fieldName] as PdfTextField;
                txtfield.Value = new PdfString(value);
            }
        }

        /// <summary>
        /// Accede a una casilla de verificación del PDF y la modifica.
        /// </summary>
        /// <param name="fieldName">El nombre de la casilla.</param>
        /// <param name="value">El valor que se quiere asignar a la casilla.</param>
        /// <param name="formList">La lista de campos del PDF.</param>
        private void SetPdfCheckBoxValue(string fieldName, bool value, ref PdfAcroForm formList)
        {
            if(formList.Fields[fieldName] != null)
            {
                PdfCheckBoxField chkfield = formList.Fields[fieldName] as PdfCheckBoxField;
                chkfield.Checked = value;
            }
        }

        //Creates PDF from scratch with graph picture and related text using Migradoc Library
        /// <summary>
        /// Crea un PDF desde cero, incluyendo una imagen del gráfico y sus datos empleando Migradoc.
        /// </summary>
        /// <param name="imageStream">El flujo de memoria que contiene la imagen del gráfico.</param>
        /// <param name="chartDataHolder">Los datos del gráfico.</param>
        /// <returns>True si la creación se produjo con éxito, de lo contrario false.</returns>
        /// Ver <see cref="ChartWindow.Menu_SaveGraphAs_Click(object, RoutedEventArgs)"/>
        public bool CreatePDFWithImage(MemoryStream imageStream, ChartDataHolder chartDataHolder)
        {
            //Create document and define fonts, sections, etc.
            Document pdf = new Document();
            Section section0 = pdf.AddSection();
            
            Font fntTitle = new Font("Calibri", 28);
            Font fntNormal = new Font("Calibri", 12);

            //Add title
            Paragraph paragraph0 = section0.AddParagraph();
            paragraph0.Format.Alignment = ParagraphAlignment.Center;
            paragraph0.AddFormattedText(chartDataHolder.ChartData[0], fntTitle);

            paragraph0.AddLineBreak();    //Add a couple of line breaks
            paragraph0.AddLineBreak();

            //Add image chart
            byte[] image_AsArray = imageStream.ToArray();
            string image_AsString = "base64:" + Convert.ToBase64String(image_AsArray);
            paragraph0.AddImage(image_AsString);

            paragraph0.AddLineBreak();    //Add line break for separation

            //Add ChartData as a bulleted list
            MigraDoc.DocumentObjectModel.Style listStyle = pdf.AddStyle("BulletedList", "Normal");
            listStyle.ParagraphFormat.LeftIndent = "0.5cm";
            Paragraph paragraph1;
            bool isFirstItem = true;
            foreach(string s in chartDataHolder.ChartData)
            {
                if (isFirstItem)
                {
                    isFirstItem = false;
                    continue;
                }
                ListInfo info = new ListInfo();
                info.ContinuePreviousList = !isFirstItem;
                info.ListType = ListType.BulletList1;
                paragraph1 = section0.AddParagraph();
                paragraph1.Style = "BulletedList";
                paragraph1.AddFormattedText(s, fntNormal);
                paragraph1.Format.ListInfo = info;
            }

            //Render document
            PdfDocumentRenderer pdfDocumentRenderer = new PdfDocumentRenderer(false);
            pdfDocumentRenderer.Document = pdf;
            pdfDocumentRenderer.RenderDocument();
            pdfDocumentRenderer.PdfDocument.Save(DestinationPath);
            return true;
        }

        //Creates PDF with graph picture using PDFSharp library
        /// <summary>
        /// Crea un PDF desde 0, con la imagen de un gráfico y sus datos empleando PDFSharp.
        /// </summary>
        /// <remarks>
        /// Este método está obsoleto y actualmente no se usa.
        /// </remarks>
        /// <param name="imageStream"></param>
        /// <param name="chartDataHolder"></param>
        /// Ver <see cref="PDFManager.CreatePDFWithImage(MemoryStream, ChartDataHolder)"/>
        public void CreatePDFWithImage_OLD(MemoryStream imageStream, ChartDataHolder chartDataHolder)
        {
            PdfDocument pdf = new PdfDocument(DestinationPath);
            PdfPage page = new PdfPage();
            pdf.AddPage(page);

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fntTitle = new XFont("Times New Roman", 28, XFontStyle.Bold);
            XFont fntText = new XFont("Arial", 14, XFontStyle.Regular);
            gfx.DrawString(chartDataHolder.ChartData[0], fntTitle, XBrushes.Black, new XPoint(page.Width / 2, 32), XStringFormats.Center);

            XImage image = XImage.FromStream(imageStream);
            gfx.DrawImage(image, new XPoint(32, 64));


            int index = 0;
            foreach (string s in chartDataHolder.ChartData)
            {
                if (index == 0)
                    continue;
                gfx.DrawString(s, fntText, XBrushes.Black, new XPoint(page.Width / 2, image.PixelHeight + 32 + index++ * 32), XStringFormats.Center);
            }

            pdf.Close();
        }
        /*
 *  DECLARANTE
    Field [untitled68] = (nif declarante) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled69] = (telefono declarant) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled70] = (nombre declarant) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled71] = (nif repr) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled72] = (ejer) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled73] = (declaracion anterior) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled74] = /Yes as PdfSharp.Pdf.AcroForms.PdfCheckBoxField
    Field [untitled75] =  as PdfSharp.Pdf.AcroForms.PdfCheckBoxField
    Field [untitled76] = /Yes as PdfSharp.Pdf.AcroForms.PdfCheckBoxField
    Field [untitled77] = (total personas y ent) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled78] = (importe anual op) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled79] = (total inmuebles) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled80] = (importe op arrend) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled81] = (fecha documento) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled82] = (firma documento) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled83] = (cargo declatante) as PdfSharp.Pdf.AcroForms.PdfTextField

    COPIA DECLARANTE
    Field [untitled14] = (nif declarante copia) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled15] = (ejer copia) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled16] = (n hoja) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled17] = (n hoja total) as PdfSharp.Pdf.AcroForms.PdfTextField

    DECLARADO 1
    Field [untitled18] = (nif declared1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled19] = (nif iva declared1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled20] = (nif repr 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled21] = (nombre declared 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled22] = (prov1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled23] = (pais1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled24] = (claveop1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled25] = (opseguro1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled26] = (arrend1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled27] = (ivacaja1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled28] = (pasivo1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled29] = (aduanero1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled30] = (imp metalico1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled31] = (ejer1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled32] = (imp anual 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled33] = (imp anual inmuebles 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled34] = (imp anual caja iva 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled35] = (importe 1t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled36] = (importe inmuebles 1t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled37] = (importe 2t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled38] = (importe 3t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled39] = (importe 4t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled40] = (importe inmuebles 2t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled41] = (importe inmuebles 3t 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled42] = (importe inmuebles 4t 1) as PdfSharp.Pdf.AcroForms.PdfTextField

    DECLARADO 2
    Field [untitled84] = (nif declared 2) as PdfSharp.Pdf.AcroForms.PdfTextField
        hasta untitled 108

    DECLARADO 3
    Field [untitled109] = (nif declared 3) as PdfSharp.Pdf.AcroForms.PdfTextField
        hasta untitled 133
    Field [untitled134] = (importe operaciones TOTAL) as PdfSharp.Pdf.AcroForms.PdfTextField

    DECLARADO 1 (PAG 2)
    [untitled43 hasta untitled67]

    Field [untitled135] = (comienza copia) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled189] = (termina copia) as PdfSharp.Pdf.AcroForms.PdfTextField

    INMUEBLES
    Field [untitled190] = (comienza inmuebles) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled191] = (ejer) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled192] = (hojanum) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled193] = (hojatotal) as PdfSharp.Pdf.AcroForms.PdfTextField

    ARRENDATARIO 1
    Field [untitled194] = (nif arrendatario1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled195] = (nif repr 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled196] = (nombre arrend 1) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled197] = (importe op) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled198] = (ref catastral) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled199] = (sit) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled200] = (tipo via) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled201] = (nombre via) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled202] = (tipo num) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled203] = (num casa) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled204] = (calif nu) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled205] = (bloque) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled206] = (portal) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled207] = (escalera) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled208] = (planta) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled209] = (puerta) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled210] = (complemento domicilio) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled211] = (localidad) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled212] = (nombre municipio) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled213] = (cod muni) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled214] = (provincia) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled215] = (cod provinc) as PdfSharp.Pdf.AcroForms.PdfTextField
    Field [untitled216] = (cod postal) as PdfSharp.Pdf.AcroForms.PdfTextField

    ARRENDATARIO 2
    Field [untitled217] = (nif arrend 2) as PdfSharp.Pdf.AcroForms.PdfTextField
        hasta untitled 239

    ARRENDATARIO 3
    Field [untitled240] = (nif arrend 3) as PdfSharp.Pdf.AcroForms.PdfTextField
        hasta untitled 262

    ARRENDATARIO 4
    Field [untitled263] = (nif arrend 4) as PdfSharp.Pdf.AcroForms.PdfTextField
        hasta untitled 285
 */

        //Testing method
        private void GetFieldNames(string path)
        {
            try
            {
                //File.Copy(".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "Modelo347-Editable.pdf", DestinationPath, true);
                //Debug.WriteLine("COPY GOES TO " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "copia.pdf");
                PdfDocument pdf = PdfReader.Open(path);

                //PdfPage pg = pdf.Pages[1];
                //PdfPage pgClone = pg.Clone() as PdfPage;
                //PdfPage pghdlr = pdf.AddPage(pgClone);
                PdfAcroForm forms = pdf.AcroForm;

                if (pdf.AcroForm.Elements.ContainsKey("/NeedAppearances") == false)
                   pdf.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));
                else
                   pdf.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);

                StreamWriter streamWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "pdfFields.log");
                if (forms != null)
                {
                    string[] fieldNames = forms.Fields.DescendantNames;
                    foreach(string s in fieldNames)
                    {
                        Debug.WriteLine("Field ["+s+"] = " + forms.Fields[s].Value + " as " + forms.Fields[s].GetType());
                        streamWriter.WriteLine("Field [" + s + "] = " + forms.Fields[s].Value + " as " + forms.Fields[s].GetType());
                    }
                    streamWriter.Close();
                }

                pdf.Save(DestinationPath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    /// <summary>
    /// Clase de excepción usada por <c>PDFManager</c>.
    /// </summary>
    [Serializable] class DeclaredAmountException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de <c>DeclaredAmountException</c>.
        /// </summary>
        /// <param name="amount">La cantidad de declarados recibidos.</param>
        public DeclaredAmountException (int amount) : base (string.Format("El número de declarados ({0}) es incompatible", amount))
        {

        }
    }
}
