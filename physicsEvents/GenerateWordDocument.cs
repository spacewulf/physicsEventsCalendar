using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace physicsEvents
{
    internal class GenerateWordDocument
    {
        public static void Create(Events[] events, string path, DateTime startDate, DateTime endDate)
        {
            using (var document = WordprocessingDocument.Create(
                path, WordprocessingDocumentType.Document))
            {
                string stringHeaderDates;
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

                SpacingBetweenLines spacingHeader = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0"};
                Paragraph pHeader = new Paragraph();
                ParagraphProperties pPropertiesHeader = new ParagraphProperties();
                pPropertiesHeader.Append(spacingHeader);
                Run runHeader = new Run();
                RunProperties runPropertiesHeader = new RunProperties(
                    new RunFonts() { Ascii = "Arial"});
                runPropertiesHeader.Append(new Bold());
                string stringHeader = "Physics Seminars and Colloquia ~ ";
                if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
                {
                    stringHeaderDates = Methods.ReturnMonth(startDate.Month) + " " + startDate.Day.ToString() + " - " + endDate.Day.ToString() + ", " + startDate.Year.ToString();
                }  else if (startDate.Year == endDate.Year && startDate.Month != endDate.Month)
                {
                    stringHeaderDates = Methods.ReturnMonth(startDate.Month) + " " + startDate.Day.ToString() + " - " + Methods.ReturnMonth(endDate.Month) + " " + endDate.Day.ToString() + ", " + endDate.Year.ToString();
                }  else 
                {
                    stringHeaderDates = Methods.ReturnMonth(startDate.Month) + " " + startDate.Day.ToString() + ", " + startDate.Year.ToString() + " - " + Methods.ReturnMonth(endDate.Month) + " " + endDate.Day.ToString() + ", " + endDate.Year.ToString();
                }
                stringHeader = stringHeader + stringHeaderDates;
                Text textHeader = new Text(stringHeader);
                runHeader.Append(runPropertiesHeader);
                runHeader.Append(textHeader);
                pHeader.Append(pPropertiesHeader);
                pHeader.Append(runHeader);
                document.MainDocumentPart.Document.Body.AppendChild(pHeader);


                SpacingBetweenLines spacingHeaderSpace = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                Paragraph pHeaderSpace = new Paragraph(
                    new Run(
                        new Text(" ")));
                pHeaderSpace.Append(spacingHeaderSpace);
                document.MainDocumentPart.Document.Body.AppendChild(pHeaderSpace);


                foreach (Events e in events)
                {
                    if (days[e.Date.DayOfWeek.ToString()] == false) //This checks to only print out the date line once for each day
                    {
                        //This prints the date at the top and links it to the corresponding page on the website.
                        SpacingBetweenLines spacingDate = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                        Paragraph pDate = new Paragraph();
                        ParagraphProperties pPropertiesDate = new ParagraphProperties();
                        pPropertiesDate.Append(spacingDate);
                        Run runDate = new Run();
                        RunProperties runPropertiesDate = new RunProperties(new RunFonts() { Ascii = "Calibri" },
                                                                            new RunStyle { Val = "Hyperlink", },
                                                                            new Underline { Val = UnderlineValues.Single },
                                                                            new Color { Val = ConfigurationManager.AppSettings.Get("HyperlinkColor")});
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

                    days[e.Date.DayOfWeek.ToString()] = true; //Once the date has been printed once, this sets the day's value to true to be never printed again (until the next time it's run)

                    Dictionary<string, bool> times = new Dictionary<string, bool>();
                    times.Add(e.StartTime, false);
                    times.Add(e.EndTime, false);
                    string startTime = e.StartTime;
                    string endTime = e.EndTime;
                    string timeText;
                    Text textTime;

                    if (Int32.Parse(e.StartTime.Substring(0, 2)) >= 12 )
                    {
                        times[e.StartTime] = true;
                        if (Int32.Parse(e.StartTime.Substring(0,2)) > 12 )
                        {
                            startTime = (Int32.Parse(e.StartTime.Substring(0, 2)) - 12).ToString() + e.StartTime.Substring(2);
                        }
                    }

                    if (Int32.Parse(e.EndTime.Substring(0,2)) >= 12 )
                    {
                        times[e.EndTime] = true;
                        if (Int32.Parse(e.EndTime.Substring(0,2)) > 12)
                        {
                            endTime = (Int32.Parse(e.EndTime.Substring(0, 2)) - 12).ToString() + e.EndTime.Substring(2);
                        }
                    }



                    SpacingBetweenLines spacingTime = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pTime = new Paragraph();
                    ParagraphProperties pPropertiesTime = new ParagraphProperties();
                    pPropertiesTime.Append(spacingTime);
                    Run runTime = new Run();
                    RunProperties runPropertiesTime = new RunProperties(
                        new RunFonts() { Ascii = "Calibri"});

                    runTime.Append(runPropertiesTime);
                    if (times[e.StartTime] == times[e.EndTime] && times[e.StartTime] == false)
                    {

                        timeText = startTime + "-" + endTime + " AM";
                        textTime = new(timeText);
                        runTime.Append(textTime);
                    }
                    else if (times[e.StartTime] == times[e.EndTime] && times[e.StartTime] == true)
                    {
                        timeText = startTime + "-" + endTime + " PM";
                        textTime = new(timeText);
                        runTime.Append(textTime);
                    }
                    else if (times[e.StartTime] != times[e.EndTime])
                    {
                        timeText = startTime + " AM-" + endTime + " PM";
                        textTime = new(timeText);
                        runTime.Append(textTime);
                    }
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
                    Highlight highlightTitle = new Highlight() { Val = HighlightColorValues.Yellow };
                    if (e.Title.Contains("|") == false)
                    {
                        runPropertiesTitle.Append(highlightTitle);
                    }
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
                    Highlight highlightSpeaker = new Highlight() { Val = HighlightColorValues.Yellow };
                    if (e.Speaker.Contains("(") == false | e.Speaker.Contains(")") == false)
                    {
                        runPropertiesSpeaker.Append(highlightSpeaker);
                    }
                    runPropertiesSpeaker.Append(new Italic());
                    Text textSpeaker = new(e.Speaker);
                    runSpeaker.Append(runPropertiesSpeaker);
                    runSpeaker.Append(textSpeaker);
                    pSpeaker.Append(pPropertiesSpeaker);
                    pSpeaker.Append(runSpeaker);
                    document.MainDocumentPart.Document.Body.AppendChild(pSpeaker);

                    Text textLocation = new Text();
                    SpacingBetweenLines spacingLocation = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    Paragraph pLocation = new Paragraph();
                    ParagraphProperties pPropertiesLocation = new ParagraphProperties();
                    pPropertiesLocation.Append(spacingLocation);
                    Run runLocation = new Run();
                    string locationText = e.Location;
                    RunProperties runPropertiesLocation = new RunProperties(
                        new RunFonts() { Ascii = "Calibri" });
                    runPropertiesLocation.Append(new Bold());
                    if (e.Location.Contains("https"))
                    {
                        locationText = new(e.Location.Substring(0, e.Location.IndexOf("https")));
                    } else if (e.Location.Contains("http"))
                    {
                        locationText = new(e.Location.Substring(0, e.Location.IndexOf("http")));
                    } else
                    {
                        locationText = new(e.Location);
                    }

                    if (e.IsLivestreamed)
                    {
                        runPropertiesLocation.Append(new Highlight());
                        locationText = locationText + " Look for livestream link!";
                    }
                    textLocation = new(locationText);
                    runLocation.Append(runPropertiesLocation);
                    runLocation.Append(textLocation);
                    pLocation.Append(pPropertiesLocation);
                    pLocation.Append(runLocation);
                    document.MainDocumentPart.Document.Body.AppendChild(pLocation);

                    if (e.Location.Contains("https"))
                    {
                        SpacingBetweenLines spacingZoom = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                        Paragraph pZoom = new Paragraph();
                        ParagraphProperties pPropertiesZoom = new ParagraphProperties();
                        pPropertiesZoom.Append(spacingZoom);
                        Run runZoomText = new Run();
                        RunProperties runPropertiesZoom = new RunProperties(
                            new RunFonts() { Ascii = "Calibri" });
                        Text textZoomText = new("Event will be in-person and on Zoom: ");
                        runZoomText.Append(runPropertiesZoom);
                        runZoomText.Append(textZoomText);
                        pZoom.Append(runZoomText);


                        Hyperlink zoomHyperLink = Methods.HyperlinkManager(e.Location.Substring(e.Location.IndexOf("https")), document.MainDocumentPart);
                        Run runZoomHyperlink = new Run();
                        RunProperties runPropertiesZoomLink = new RunProperties(
                            new RunFonts() { Ascii = "Calibri" },
                            new Color() { Val = ConfigurationManager.AppSettings.Get("HyperlinkColor")});
                        Text textZoomLink = new(" " + e.Location.Substring(e.Location.IndexOf("https")));
                        textZoomLink.Space = SpaceProcessingModeValues.Preserve;
                        runZoomHyperlink.Append(runPropertiesZoomLink);
                        runZoomHyperlink.Append(textZoomLink);
                        zoomHyperLink.Append(runZoomHyperlink);
                        pZoom.Append(pPropertiesZoom);
                        pZoom.Append(zoomHyperLink);
                        document.MainDocumentPart.Document.Body.AppendChild(pZoom);

                    }

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
