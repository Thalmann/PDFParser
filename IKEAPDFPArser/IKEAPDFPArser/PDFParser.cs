using System;
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

namespace IKEAPDFWorkingScheduleParser
{
    public class PDFParser
    {
        public PDFParser()
        {
        }

        public string[] ReadPdfFile(string fileName)
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
            return Regex.Split(text.ToString(), "(?=Mandag)|(?=Tirsdag)|(?=Onsdag)|(?=Torsdag)|(?=Fredag)|(?=Lørdag)|(?=Søndag)"); 
        }

        private int startYear, startMonth, startDay, startHour, startMinute, endYear, endMonth, endDay, endHour, endMinute;

        public List<EventEntry> GetEvents(string[] source)
        {
            List<EventEntry> list = new List<EventEntry>();

            bool yearChanged = false;
            //bool IKEADoc = false;

            int lineNumber = 0;

            foreach (string line in source)
            {
                string temp;
                string[] tempA;
                lineNumber++;
                EventEntry entry = new EventEntry();
                When eventTime = new When();

                entry.Title.Text = "Arbejde IKEA";

                //Tjek om det er en IKEA arbejdsplan
                /*if (IKEADoc == false)
                {
                    if (line.Contains("Arbejdsplan"))
                    {
                        IKEADoc = true;
                    }
                    else
                    {
                        MessageBox.Show("Wrong PDF, it has to be a IKEA schedueling PDF", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }*/
                
                
                /*
                if (yearChanged)
                {
                    if (line.Contains("for perioden"))
                    {
                        temp = "20" + line.CutToSection("-20", " - ", true);
                        if (int.TryParse(temp, out startYear) == false)
                        {
                            MessageBox.Show("Wrong Info From PDF, contact mr.thalmann@gmail.com.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        temp = "20" + line.CutToLast("20", CutDirection.Left, true);
                        if (int.TryParse(temp, out endYear) == false)
                        {
                            MessageBox.Show("Wrong Info From PDF, contact mr.thalmann@gmail.com.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    yearChanged = false;
                    tempStartYear = startYear;
                    tempEndYear = startYear;
                }*/


                //Get next string line, because there is no space for the whole message on the actual line
                if (!(lineNumber == source.Count()))
                {
                    string s = source[lineNumber];
                    if (line.Contains("ikke fysisk tilstede"))
                    {
                        entry.Title.Text = "Arbejde IKEA - IKKE FYSISK TILSTEDE";
                    }
                    if (s.Contains("møde"))
                    {
                        entry.Title.Text = "Arbejde IKEA - INTERNT MØDE";
                    }
                    if (s.Contains("udviklingsam"))
                    {
                        entry.Title.Text = "Arbejde IKEA - UDVIKLINGSSAMTALE";
                    }
                }
                

                if (!line.Contains("Fri") && (line.Contains("Mandag") || line.Contains("Tirsdag") || line.Contains("Onsdag") || line.Contains("Torsdag") || line.Contains("Fredag") || line.Contains("Lørdag") || line.Contains("Søndag")))
                {
                    bool clockCheckedStart = false, clockCheckedEnd = false, dateChecked = false, test = true;
                    tempA = Regex.Replace(line, "(- )|(: )", "").Split(new string[] { }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in tempA)
                    {
                        test = true;
                        if (clockCheckedStart == false)
                        {
                            if (s.Contains(":"))
                            {
                                temp = s.CutToFirst(":", CutDirection.Right, true);
                                if (int.TryParse(temp, out startHour) == false)
                                {
                                    return null;
                                }

                                temp = s.CutToFirst(":", CutDirection.Left, true);
                                if (int.TryParse(temp, out startMinute) == false)
                                {
                                    return null;
                                }
                                clockCheckedStart = true;
                                test = false;
                            }
                        }
                        if (clockCheckedEnd == false && test == true)
                        {
                            if (s.Contains(":"))
                            {
                                temp = s.CutToFirst(":", CutDirection.Right, true);
                                if (int.TryParse(temp, out endHour) == false)
                                {
                                    return null;
                                }

                                temp = s.CutToFirst(":", CutDirection.Left, true);
                                if (int.TryParse(temp, out endMinute) == false)
                                {
                                    return null;
                                }
                                clockCheckedEnd = true;
                            }
                        }

                        if (dateChecked == false)
                        {
                            if (s.Contains("-"))
                            {
                                temp = s.CutToFirst("-", CutDirection.Right, true);
                                if (int.TryParse(temp, out startDay) == false)
                                {
                                    return null;
                                }

                                temp = s.CutToFirst("-", CutDirection.Left, true);
                                if (int.TryParse(temp, out startMonth) == false)
                                {
                                    return null;
                                }
                                dateChecked = true;
                            }
                        }

                    }
                    if (startMonth == 1)
                    {
                        startYear = 2013;
                        endYear = 2013;
                        yearChanged = true;
                    }
                    else if (yearChanged == false)
                    {
                        startYear = 2012;
                        endYear = 2012;
                    }

                    

                    endDay = startDay;
                    endMonth = startMonth;

                    DateTime start = new DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
                    DateTime end = new DateTime(endYear, endMonth, endDay, endHour, endMinute, 0);

                    
                    eventTime.StartTime = start;
                    eventTime.EndTime = end;

                    entry.Times.Add(eventTime);
                    list.Add(entry);
                }

                }
            return list;
        }



        public int tempStartYear { get; set; }

        public int tempEndYear { get; set; }
    }
}
