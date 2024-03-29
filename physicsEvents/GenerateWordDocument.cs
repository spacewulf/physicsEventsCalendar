﻿using DocumentFormat.OpenXml;
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

                //StyleDefinitionsPart part = document.MainDocumentPart.StyleDefinitionsPart;

                //Style defaultStyle = StyleDefinitionsPart.Styles;
                //if 

                foreach (Events e in events)
                {
                    Paragraph pDate = new Paragraph();
                    Run runDate = pDate.AppendChild(new Run());
                    Text date = new Text(e.Date.ToString("D", CultureInfo.CreateSpecificCulture("en")));
                    runDate.AppendChild(date);
                    document.MainDocumentPart.Document.Body.AppendChild(pDate);

                    Paragraph pTime = new Paragraph();
                    Run runTime = pTime.AppendChild(new Run());
                    Text time = new Text(e.StartTime.ToString());
                    runTime.AppendChild(time);
                    document.MainDocumentPart.Document.Body.AppendChild(pTime);

                    Paragraph pTitle = new Paragraph();
                    Run runTitle = pTitle.AppendChild(new Run());
                    Text title = new Text(e.Title);
                    runTitle.AppendChild(title);
                    document.MainDocumentPart.Document.Body.AppendChild(pTitle);

                    Paragraph pSpeaker = new Paragraph();
                    Run runSpeaker = pSpeaker.AppendChild(new Run());
                    Text speaker = new Text(e.Speaker);
                    runSpeaker.AppendChild(speaker);
                    document.MainDocumentPart.Document.Body.AppendChild(pSpeaker);

                    Paragraph pLocation = new Paragraph();
                    Run runLocation = pLocation.AppendChild(new Run());
                    Text location = new Text(e.Location);
                    runLocation.AppendChild(location);
                    document.MainDocumentPart.Document.Body.AppendChild(pLocation);

                    Paragraph pSpace =
                        new Paragraph(
                            new Run(
                                new Text("")));
                    document.MainDocumentPart.Document.Body.AppendChild(pSpace);

                }

            }
        }
    }
}
