﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using DeadDog;
using System.Text.RegularExpressions;

namespace IKEAPDFPArser
{
    public class PDFParser
    {
        public PDFParser()
        {
        }

        public string GetString(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                    pdfReader.Close();
                }
            }
            else
                throw new FileNotFoundException();

            return text.ToString();
        }
    }
}
