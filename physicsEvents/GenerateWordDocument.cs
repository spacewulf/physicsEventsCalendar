using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace physicsEvents
{
    internal class GenerateWordDocument
    {
        public static void Create(Events[] events)
        {
            using (var document = WordprocessingDocument.Create(
                @"c:\Users\Kees Wolterstorff\Desktop\test.docx", WordprocessingDocumentType.Document))
            {
                document.AddMainDocumentPart();
                document.MainDocumentPart.Document = new Document(new Body());
                Dictionary<string, bool> days = new Dictionary<string, bool>();
                days.Add("Sunday", false);
                days.Add("Monday", false);
                days.Add("Tuesday", false);
                days.Add("Wednesday", false);
                days.Add("Thursday", false);
                days.Add("Friday", false);
                days.Add("Saturday", false);

                foreach (Events e in events)
                {
                    if (days[e.Date.DayOfWeek.ToString()] == false)
                    {
                        SpacingBetweenLines spacingDate = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                        Paragraph pDate = new Paragraph();
                        ParagraphProperties pPropertiesDate = new ParagraphProperties();
                        pPropertiesDate.Append(spacingDate);
                        Run runDate = new Run();
                        RunProperties runPropertiesDate = new RunProperties(new RunFonts() { Ascii = "Calibri" },
                                                                            new RunStyle { Val = "Hyperlink", },
                                                                            new Underline { Val = UnderlineValues.Single },
                                                                            new Color { Val = "76ABEE" });
                        runPropertiesDate.Append(new Bold());
                        Hyperlink dateHyperlink = Methods.HyperlinkManager(e.DateUri.ToString(), document.MainDocumentPart);
                        Text textDate = new Text(e.Date.ToString("D", CultureInfo.CreateSpecificCulture("en")));
                        runDate.Append(runPropertiesDate);
                        runDate.Append(textDate);
                        dateHyperlink.Append(runDate);
                        pDate.Append(pPropertiesDate);
                        pDate.Append(dateHyperlink);
                        document.MainDocumentPart.Document.Body.AppendChild(pDate);
                    }

                    days[e.Date.DayOfWeek.ToString()] = true;

                    SpacingBetweenLines spacingTime = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pTime = new Paragraph();
                    ParagraphProperties pPropertiesTime = new ParagraphProperties();
                    pPropertiesTime.Append(spacingTime);
                    Run runTime = new Run();
                    RunProperties runPropertiesTime = new RunProperties(
                        new RunFonts() { Ascii = "Calibri"});
                    string timeText = e.StartTime + "-" + e.EndTime;
                    Text textTime = new(timeText);
                    runTime.Append(runPropertiesTime);
                    runTime.Append(textTime);
                    pTime.Append(pPropertiesTime);
                    pTime.Append(runTime);
                    document.MainDocumentPart.Document.Body.AppendChild(pTime);

                    SpacingBetweenLines spacingTitle = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pTitle = new Paragraph();
                    ParagraphProperties pPropertiesTitle = new ParagraphProperties();
                    pPropertiesTitle.Append(spacingTitle);
                    Run runTitle = new Run();
                    RunProperties runPropertiesTitle = new RunProperties(
                        new RunFonts() { Ascii = "Calibri" },
                        new RunStyle { Val = "Hyperlink", },
                        new Underline { Val = UnderlineValues.Single });
                    runPropertiesTitle.Append(new Bold());
                    Hyperlink titleHyperlink = Methods.HyperlinkManager(e.Uri.ToString(), document.MainDocumentPart);

                    Text textTitleHyperlink = new(e.Title);
                    runTitle.Append(runPropertiesTitle);
                    runTitle.Append(textTitleHyperlink);
                    titleHyperlink.Append(runTitle);
                    pTitle.Append(pPropertiesTitle);
                    pTitle.Append(titleHyperlink);
                    document.MainDocumentPart.Document.Body.AppendChild(pTitle);

                    SpacingBetweenLines spacingSpeaker = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pSpeaker = new Paragraph();
                    ParagraphProperties pPropertiesSpeaker = new ParagraphProperties();
                    pPropertiesSpeaker.Append(spacingSpeaker);
                    Run runSpeaker = new Run();
                    RunProperties runPropertiesSpeaker = new RunProperties(
                        new RunFonts() { Ascii = "Calibri" });
                    Text textSpeaker = new(e.Speaker);
                    runSpeaker.Append(runPropertiesSpeaker);
                    runSpeaker.Append(textSpeaker);
                    pSpeaker.Append(pPropertiesSpeaker);
                    pSpeaker.Append(runSpeaker);
                    document.MainDocumentPart.Document.Body.AppendChild(pSpeaker);

                    SpacingBetweenLines spacingLocation = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pLocation = new Paragraph();
                    ParagraphProperties pPropertiesLocation = new ParagraphProperties();
                    pPropertiesLocation.Append(spacingLocation);
                    Run runLocation = new Run();
                    RunProperties runPropertiesLocation = new RunProperties(
                        new RunFonts() { Ascii = "Calibri" });
                    runPropertiesLocation.Append(new Bold());
                    Text textLocation = new(e.Location);
                    runLocation.Append(runPropertiesLocation);
                    runLocation.Append(textLocation);
                    pLocation.Append(pPropertiesLocation);
                    pLocation.Append(runLocation);
                    document.MainDocumentPart.Document.Body.AppendChild(pLocation);

                    SpacingBetweenLines spacingSpace = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pSpace = new Paragraph();
                    ParagraphProperties pPropertiesSpace = new ParagraphProperties();
                    pPropertiesSpace.Append(spacingSpace);
                    Run runSpace = new Run();
                    RunProperties runPropertiesSpace = new RunProperties(
                        new RunFonts() { Ascii = "Calibri" });
                    Text textSpace = new Text("");
                    runSpace.Append(runPropertiesSpace);
                    runSpace.Append(textSpace);
                    pSpace.Append(pPropertiesSpace);
                    pSpace.Append(runSpace);
                    document.MainDocumentPart.Document.Body.AppendChild(pSpace);

                }

            }
        }
    }
}
