using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using ESCommon;
using ESCommon.Rtf;
using HtmlAgilityPack;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.WebUtil
{
    public static class LetterReportUtil
    {
        //Constant Variables
        private const string RtfFontType = "Verdana";
        private const string RtfImageNodeName = "img";
        private const string RtfTextNodeName = "#text";
        private const string RtfImageSrcName = "src";
        private const string RtfHeightAttribute = "height";
        private const string RtfWidthAttribute = "width";

        //Default variables for preview
        private static readonly DateTime LetterBillDate = Convert.ToDateTime("01/14/2013");
        private const string LetterBillType = "111";
        private const string LetterContractName = "Professional Contract";
        private const string LetterDrg = "313";
        private const double LetterExpectedAllowed = 5000;
        private const string LetterFtn = "12-3456789";

        private const string PrimaryGroupNumber = "PGRP1234";
        private const string SecondaryGroupNumber = "SGRP1234";
        private const string TertiaryGroupNumber = "TGRP1234";

        private const string PrimaryMemberId = "PID12345678";
        private const string SecondaryMemberId = "SID12345678";
        private const string TertiaryMemberId = "TID12345678";

        private const string LetterMedRecNumber = "MR12345678";
        private const string LetterNpi = "1003814971";
        private const string LetterPatientAccountNumber = "ACCT123456";
        private static readonly DateTime LetterPatientDateOfBirth = Convert.ToDateTime("06/10/1959");
        private const string LetterPatientFirstName = "John";
        private const string LetterPatientLastName = "Dean";
        private const string LetterPatientMiddleName = "M";
        private const double LetterPatientResponsibility = 500;
        private const string LetterPayerName = "Medicare";
        private const string LetterProviderName = "General Medical Center";
        private static readonly DateTime LetterRemitCheckDate = Convert.ToDateTime("05/23/2013");
        private const double LetterRemitPayment = 5000;
        private static readonly DateTime LetterStatementFrom = Convert.ToDateTime("01/01/2013");
        private static readonly DateTime LetterStatementThru = Convert.ToDateTime("01/05/2013");
        private const double LetterClaimTotal = 5500;
        private const string LetterIcn = "1426116263";
        private const int LetterLos = 4;
        private const int LetterAge = 54;

        private static HtmlDocument _htmlDocument;
        //Other local Variables
        private static AppealLetter _appealLetter;
        private static string _filePath;

        private const float FontSize = 10;

        /// <summary>
        /// Gets the name of the exported file.
        /// </summary>
        /// <param name="appealLetter">The appeal letter container.</param>
        /// <param name="reportVirtualPath">The report virtual path.</param>
        /// <param name="fileBaseName">Name of the file base.</param>
        /// <returns></returns>
        public static string[] GetExportedFileName(AppealLetter appealLetter, string reportVirtualPath, string fileBaseName)
        {
            string fileName;

            if (appealLetter.ReportThreshold == Constants.ReportThreshold) // Exceeded threshold limit
            {
                fileName = Convert.ToString(Constants.ReportThreshold);
            }
            else if (appealLetter.AppealLetterClaims.Count == 0) //No Records Found
            {
                fileName = Constants.EmptyReportResult;
            }
            else
            {
                _appealLetter = appealLetter;
                string dateTimeStamp = DateTime.Now.ToString(Constants.DateTimeExtendedFormat);
                fileName = string.Format("{0}{1}.{2}", fileBaseName, dateTimeStamp, Constants.AppealLetterFileExtension);
                _filePath = Path.Combine(reportVirtualPath, fileName);
                //Build rtf document object
                RtfDocument rtfDocument = GetRtfDocument();

                //Get rtf content String
                RtfWriter rtfWriter = new RtfWriter();
                string templateRtfContent = rtfWriter.GetRtfContent(rtfDocument);
                //loop through each claim data and replace claim text and merge rtf content 
                string finalRtfContent = _appealLetter.AppealLetterClaims.Select(
                    appealLetterClaim => ReplaceClaimData(templateRtfContent, appealLetterClaim))
                    .Aggregate(string.Empty,
                        (current, rtfClaimText) =>
                            !string.IsNullOrEmpty(current)
                                ? MergeRtfContent(current, rtfClaimText)
                                : rtfClaimText);

                //Write rtf content into file
                using (TextWriter writer = new StreamWriter(_filePath))
                {
                    rtfWriter.Write(writer, finalRtfContent);
                }
                if (appealLetter.IsPreview)
                {
                    return new[] { appealLetter.LetterTemplaterText, fileName };
                }
            }
            return new[] { fileName };

        }

        /// <summary>
        /// Merges the content of the RTF.
        /// </summary>
        /// <param name="sourceContent">Content of the source.</param>
        /// <param name="destinationContent">Content of the destination.</param>
        /// <returns></returns>
        private static string MergeRtfContent(string sourceContent, string destinationContent)
        {
            try
            {
                // in the source file, we obtain indexces of the first and the last paragraphs
                int startContent = destinationContent.IndexOf(Constants.RtfParagraphTag, StringComparison.Ordinal);
                int endContent = destinationContent.LastIndexOf(Constants.RtfParagraphTag, StringComparison.Ordinal) - 1;
                if ((startContent < 0) || (endContent < 0))
                    return sourceContent;

                string migratingContent = destinationContent.Substring(startContent, endContent - startContent + 1);

                // processing the destination file to get the index at which we insert the migrating content
                int srcClose = sourceContent.LastIndexOf(Constants.RtfDocumentEndTag, StringComparison.Ordinal);
                if (srcClose < 0)
                    return sourceContent;

                // ensures that there's a page break between the old content and the new one
                if (migratingContent.StartsWith(Constants.RtfParagraphResetTag))
                    migratingContent = Constants.RtfPageBreak + migratingContent.Remove(0, Constants.RtfParagraphResetTag.Length);
                sourceContent = sourceContent.Insert(srcClose, migratingContent);

                return sourceContent;
            }
            catch
            {
                return sourceContent;
            }
        }

        /// <summary>
        /// Replaces the image path.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        /// <returns></returns>
        private static string ReplaceImagePath(string templateText)
        {
            MatchCollection matchCollection = Regex.Matches(templateText, Constants.SourceRegex);
            for (int matchCollectionCount = 0; matchCollectionCount < matchCollection.Count; matchCollectionCount++)
            {
                if (Convert.ToString(matchCollection[matchCollectionCount]).Contains(Constants.Imagepath))
                {
                    string imageSource = string.Format("{0}{1}{2}", Constants.ImageSourceBasePath,
                        GlobalConfigVariable.LetterTemplateImagePath + Constants.FileNameSeperator,
                        Convert.ToString(matchCollection[matchCollectionCount])
                            .Substring(
                                matchCollection[matchCollectionCount].ToString()
                                    .IndexOf(Constants.Path, StringComparison.Ordinal) + Constants.IndexOfHtmlImageSourceValue));

                    templateText = templateText.Replace(matchCollection[matchCollectionCount].ToString(), imageSource);
                }
            }
            return templateText;
        }

        /// <summary>
        /// Replaces the style.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        /// <returns></returns>
        private static string ReplaceStyle(string templateText)
        {
            templateText = templateText.Replace("<div><br /></div>", "<br />");
            templateText = templateText.Replace(Constants.HtmlLineBreakTag, Constants.HtmlParagraphEndTag + Constants.HtmlParagraphStartTag);

            if (templateText.StartsWith(Constants.HtmlParagraphEnclosure))
            {
                Regex replaceDiv = new Regex(Regex.Escape(Constants.HtmlParagraphEnclosure));
                templateText = replaceDiv.Replace(templateText, Constants.HtmlParagraphStartTag, 1);
            }
            templateText = templateText.Replace(Constants.HtmlParagraphStartEnclosure, Constants.HtmlParagraphEndTag);
            templateText = templateText.Replace(Constants.HtmlParagraphEndEnclosure, Constants.HtmlParagraphStartTag);

            if (!templateText.EndsWith(Constants.HtmlParagraphEndTag))
            {
                templateText = templateText + Constants.HtmlParagraphEndTag;
            }

            //Replace Letter Style attribute in template text
            templateText = Constants.LetterStylePairs.Aggregate(templateText, (result, s) => result.Replace(s.Key, s.Value));
            return templateText;
        }


        /// <summary>
        /// Replaces the claim data.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        /// <param name="appealLetterClaim">The appeal letter claim.</param>
        /// <returns></returns>
        private static string ReplaceClaimData(string templateText, AppealLetterClaim appealLetterClaim)
        {
            //Creating Field and Value pair for ClaimData
            Dictionary<string, string> claimColumns =
                new Dictionary<string, string>
                {
                    {
                        Constants.BillDate,
                        appealLetterClaim.BillDate.HasValue
                        ? (appealLetterClaim.BillDate.Value.ToString("MM/dd/yyyy") == "01/01/1900" ? string.Empty : appealLetterClaim.BillDate.Value.ToString("MM/dd/yyyy"))
                            : string.Empty
                    },
                    {Constants.BillType, appealLetterClaim.BillType},
                    {Constants.ContractName, appealLetterClaim.ContractName},
                    {Constants.Drg, appealLetterClaim.Drg},
                    {
                        Constants.ExpectedAllowed,
                        appealLetterClaim.ExpectedAllowed.HasValue
                            ? "$" + appealLetterClaim.ExpectedAllowed.Value.ToString("0.00")
                            : string.Empty
                    },
                    {Constants.Ftn, appealLetterClaim.Ftn},
                    {Constants.PrimaryGroupNumber, appealLetterClaim.PrimaryGroupNumber},
                    {Constants.SecondaryGroupNumber, appealLetterClaim.SecondaryGroupNumber},
                    {Constants.TertiaryGroupNumber, appealLetterClaim.TertiaryGroupNumber},

                    {Constants.PrimaryMemberId, appealLetterClaim.PrimaryMemberId},
                    //{Constants.SecondaryMemberId, appealLetterClaim.SecondaryMemberId},
                    //{Constants.TertiaryMemberId, appealLetterClaim.TertiaryMemberId},

                    {Constants.MedRecNumber, appealLetterClaim.MedRecNumber},
                    {Constants.Npi, appealLetterClaim.Npi},
                    {Constants.PatientAccountNumber, appealLetterClaim.PatientAccountNumber},
                    {
                        Constants.PatientDob,
                        appealLetterClaim.PatientDob.HasValue
                        ? (appealLetterClaim.PatientDob.Value.ToString("MM/dd/yyyy") == "01/01/1900" ? string.Empty : appealLetterClaim.PatientDob.Value.ToString("MM/dd/yyyy"))
                            : string.Empty
                    },
                    {Constants.PatientFirstName, appealLetterClaim.PatientFirstName},
                    {Constants.PatientLastName, appealLetterClaim.PatientLastName},
                    {Constants.PatientMiddleName, appealLetterClaim.PatientMiddleName},
                    {
                        Constants.PatientResponsibility,
                        appealLetterClaim.PatientResponsibility.HasValue
                            ? "$" + appealLetterClaim.PatientResponsibility.Value.ToString("0.00")
                            : string.Empty
                    },
                    {Constants.PayerName, appealLetterClaim.PayerName},
                    {Constants.ProviderName, appealLetterClaim.ProviderName},

                    {
                        Constants.RemitCheckDate,
                        appealLetterClaim.RemitCheckDate.HasValue
                        ? (appealLetterClaim.RemitCheckDate.Value.ToString("MM/dd/yyyy") == "01/01/1900" ? string.Empty : appealLetterClaim.RemitCheckDate.Value.ToString("MM/dd/yyyy"))
                            : string.Empty
                    },
                    {
                        Constants.RemitPayment,
                        appealLetterClaim.RemitPayment.HasValue
                            ? "$" + appealLetterClaim.RemitPayment.Value.ToString("0.00")
                            : string.Empty
                    },

                    {
                        Constants.StatementFrom,
                        appealLetterClaim.StatementFrom.HasValue
                        ? (appealLetterClaim.StatementFrom.Value.ToString("MM/dd/yyyy") == "01/01/1900" ? string.Empty : appealLetterClaim.StatementFrom.Value.ToString("MM/dd/yyyy"))
                            : string.Empty
                    },
                    {
                        Constants.StatementThru,
                        appealLetterClaim.StatementThru.HasValue
                            ? (appealLetterClaim.StatementThru.Value.ToString("MM/dd/yyyy") == "01/01/1900" ? string.Empty : appealLetterClaim.StatementThru.Value.ToString("MM/dd/yyyy"))
                            : string.Empty
                    },
                    {
                        Constants.ClaimTotal,
                        appealLetterClaim.ClaimTotal.HasValue
                            ? "$" + appealLetterClaim.ClaimTotal.Value.ToString("0.00")
                            : string.Empty
                    },
                    {
                    Constants.CurrentDate,
                    DateTime.Now.ToString("MM/dd/yyyy")
                    },
                    {Constants.Icn, appealLetterClaim.Icn},
                    {Constants.Los, Convert.ToString(appealLetterClaim.Los)},
                    {Constants.Age, Convert.ToString(appealLetterClaim.Age)}
                };

            //Replace claim data in template text
            string templateClaimText = claimColumns.Aggregate(templateText,
                (result, s) => result.Replace(s.Key, s.Value));

            return templateClaimText;
        }

        /// <summary>
        /// Gets the RTF document.
        /// </summary>
        /// <returns></returns>
        private static RtfDocument GetRtfDocument()
        {
            string templateText = HttpUtility.HtmlDecode(_appealLetter.LetterTemplaterText);

            //Add Content into Paragraph (if letter template is save without pressing enter than no paragraph is coming and for generating rtf we need each content into paragraph)
            if (!templateText.Contains(Constants.HtmlStartTag + Constants.HtmlParagraphTagName))
                templateText = string.Format("{0}{1}{2}", Constants.HtmlParagraphStartTag, templateText, Constants.HtmlParagraphEndTag);

            //Replace Image Path
            templateText = ReplaceImagePath(templateText);

            _htmlDocument = new HtmlDocument();
            
            //Replace some style attributes for rtf conversion
            templateText = ReplaceStyle(templateText);
            
            //HTML Div element's attributes will be copied to child paragraph nodes.
            templateText = CloneParentDivHtmlAttributesToChildNodes(templateText);
            templateText = PropagateImageStyleToParagraph(templateText);

            //Get HtmlDocument from templateText
            _htmlDocument.LoadHtml(templateText);
            
            //Get List of RtfHtmlNode from Document node
            List<RtfHtmlNode> rtfHtmlNodes = GetRtfHtmlNodes(_htmlDocument.DocumentNode);

            //Create rtfDocument object and add default properties
            RtfDocument rtfDocument = new RtfDocument();
            rtfDocument.FontTable.Add(new RtfFont(RtfFontType));

            //Get RtfDocumentContentBase array from list of RtfHtmlNode 
            RtfDocumentContentBase[] rtfDocumentContents = GetRtfDocumentContents(rtfHtmlNodes);
            rtfDocument.Contents.AddRange(rtfDocumentContents);
            return rtfDocument;
        }

        /// <summary>
        ///  Gets html based on template text
        /// </summary>
        /// <param name="templateText">Template text</param>
        /// <returns>html content</returns>
        private static string PropagateImageStyleToParagraph(string templateText)
        {
            _htmlDocument.LoadHtml(templateText);

            var imageNodes = _htmlDocument.DocumentNode.SelectNodes(Constants.HtmlImageXPath);
            if (imageNodes != null)
            {
                foreach (HtmlNode imageNode in imageNodes)
                {
                    KeyValuePair<string, string> matchedImageStyle = new KeyValuePair<string, string>();
                    var imageStyle = imageNode.Attributes.FirstOrDefault(attribute => attribute.Name == Constants.HtmlStyleAttribute);
                    var immediateParagraph = imageNode.Ancestors(Constants.HtmlParagraphTagName).FirstOrDefault();
                    if(imageStyle != null)
                        matchedImageStyle = Constants.ImageStylePairs.FirstOrDefault(x => imageStyle.Value.Contains(x.Key));

                    if (imageStyle != null && immediateParagraph != null && !immediateParagraph.Attributes.Contains(Constants.HtmlAlignAttribute))
                    {
                        immediateParagraph.Attributes.Add(Constants.HtmlAlignAttribute, matchedImageStyle.Value);
                    }
                }
            }
            return _htmlDocument.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// HTML Div element's attributes will be copied to child paragraph nodes.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        /// <returns></returns>
        private static string CloneParentDivHtmlAttributesToChildNodes(string templateText)
        {
            //Get HtmlDocument from templateText
            _htmlDocument.LoadHtml(templateText);
            //Get all HTML div elements
            var divNodes = _htmlDocument.DocumentNode.SelectNodes(Constants.HtmlDivXPath);

            if (divNodes != null)
            {
                foreach (var divNode in divNodes)
                {   
                    // Check if div node contains attributes and child nodes with paragraph elements
                    if (divNode.Attributes.Any() && (divNode.Descendants(Constants.HtmlParagraphTagName).Any() || divNode.Descendants(Constants.HtmlImageTagName).Any()))
                    {
                        foreach (var attribute in divNode.Attributes)
                        {
                            //For each Paragraph Tag copy parent div attributes.
                            foreach (var paragraphNodes in divNode.Descendants(Constants.HtmlParagraphTagName).Where(paragraphNodes => !paragraphNodes.Attributes.Contains(Constants.HtmlAlignAttribute)))
                            {
                                paragraphNodes.Attributes.Add(attribute);
                            }

                            //For each Image Tag copy parent div attributes.
                            foreach (var imageNode in divNode.Descendants(Constants.HtmlImageTagName).Where(imageNode => !imageNode.Attributes.Contains(Constants.HtmlAlignAttribute)))
                            {
                                imageNode.Attributes.Add(Constants.HtmlStyleAttribute, Constants.ImageStylePairs.FirstOrDefault(attrib =>  attrib.Value == attribute.Value).Key);
                            }
                        }
                    }
                    else
                    {
                        //Div nodes without attributes and paragraph child nodes.
                        //Create span HTML node
                        HtmlNode spanHtmlNode = _htmlDocument.CreateElement(Constants.HtmlSpanTagName);
                        //copy div tag attributes to newly created span node.
                        foreach (var attribute in divNode.Attributes)
                        {
                            spanHtmlNode.Attributes.Add(attribute);
                        }
                        //wrap inner html of div node to newly created span node
                        spanHtmlNode.InnerHtml = divNode.InnerHtml;
                        divNode.RemoveAll();
                        //Append span node to current div node
                        divNode.ChildNodes.Add(spanHtmlNode);
                    }
                    //Remove all attributes from div node element.
                    divNode.Attributes.RemoveAll();
                }
            }
            
            templateText = _htmlDocument.DocumentNode.OuterHtml;
            
            // Remove opening and closing div tags
            if (templateText.Contains(Constants.HtmlDivClosingTag))
            {
                templateText = templateText.Replace(Constants.HtmlDivClosingTag, Constants.HtmlParagraphEndTag + Constants.HtmlParagraphStartTag);
                templateText = templateText.Replace(Constants.HtmlDivOpeningTag, Constants.HtmlParagraphEndTag + Constants.HtmlParagraphStartTag);
            }
            return templateText; 
        }
        /// <summary>
        /// Gets the RTF document content base list.
        /// </summary>
        /// <param name="rtfHtmlNodeList">The RTF HTML node list.</param>
        /// <returns></returns>
        private static RtfDocumentContentBase[] GetRtfDocumentContents(List<RtfHtmlNode> rtfHtmlNodeList)
        {
            RtfDocumentContentBase[] rtfDocumentContents = new RtfDocumentContentBase[rtfHtmlNodeList.Count];
            for (int nodeCount = 0; nodeCount < rtfHtmlNodeList.Count; nodeCount++)
            {
                RtfHtmlNode rtfHtmlNode = rtfHtmlNodeList[nodeCount];
                RtfParagraphFormat rtfParagraphFormat = new RtfParagraphFormat(FontSize,
                    string.IsNullOrEmpty(rtfHtmlNode.RtfParagraphFormat)
                        ? RtfTextAlign.Justified
                        : GetRtfTextAlign(rtfHtmlNode.RtfParagraphFormat));
                RtfFormattedParagraph rtfFormattedParagraph = new RtfFormattedParagraph(rtfParagraphFormat);
                foreach (var rtfHtmlItem in rtfHtmlNode.RtfHtmlItem)
                {

                    //Add text in paragraph
                    if (!string.IsNullOrEmpty(rtfHtmlItem.HtmlText))
                        rtfFormattedParagraph.AppendText(new RtfFormattedText(rtfHtmlItem.HtmlText, rtfHtmlItem.RtfFormattedText));
                    //Add image in paragraph
                    if (rtfHtmlItem.HtmlImage != null)
                    {
                        RtfImage rtfImage = new RtfImage(rtfHtmlItem.HtmlImage, RtfImageFormat.Wmf, rtfHtmlItem.ImageHeight, rtfHtmlItem.ImageWidth);
                        rtfFormattedParagraph.AppendText(rtfImage);
                    }

                }
                rtfFormattedParagraph.Formatting.SpaceAfter = TwipConverter.ToTwip(10F, MetricUnit.Point);
                rtfDocumentContents[nodeCount] = rtfFormattedParagraph;
            }
            return rtfDocumentContents;
        }

        /// <summary>
        /// Gets the RTF HTML node list.
        /// </summary>
        /// <param name="htmlNode">The document node.</param>
        /// <returns></returns>
        private static List<RtfHtmlNode> GetRtfHtmlNodes(HtmlNode htmlNode)
        {
            List<RtfHtmlNode> rtfHtmlNodes = new List<RtfHtmlNode>();
            foreach (var node in htmlNode.ChildNodes)
            {
                IEnumerable<HtmlNode> descendantNodes = node.Descendants();
                RtfHtmlNode rtfHtmlNode = new RtfHtmlNode();
                if (node.Attributes.Count > 0)
                    rtfHtmlNode.RtfParagraphFormat = node.Attributes[0].Value;
                RtfHtmlItem rtfHtmlItem = new RtfHtmlItem();
                foreach (var currentNode in descendantNodes)
                {
                    if (currentNode.Name.Equals(RtfImageNodeName) && currentNode.Attributes.Count > 0) //Image
                    {
                        rtfHtmlItem = GetImage(currentNode);
                        rtfHtmlNode.RtfHtmlItem.Add(rtfHtmlItem);
                        rtfHtmlItem = new RtfHtmlItem();
                    }
                    else if (currentNode.Name.Equals(RtfTextNodeName)) //Text
                    {
                        rtfHtmlItem.HtmlText = currentNode.InnerText;
                        rtfHtmlNode.RtfHtmlItem.Add(rtfHtmlItem);
                        rtfHtmlItem = new RtfHtmlItem();
                    }
                    else //Format text
                    {
                        rtfHtmlItem.RtfFormattedText.Add(currentNode.Name);
                    }
                }
                rtfHtmlNodes.Add(rtfHtmlNode);
            }
            return rtfHtmlNodes;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="htmlNode">The HTML node.</param>
        /// <returns></returns>
        private static RtfHtmlItem GetImage(HtmlNode htmlNode)
        {
            RtfHtmlItem rtfHtmlItem = new RtfHtmlItem();

            if (htmlNode.Attributes.Count > 0)
            {
                string imageUrl = HttpUtility.UrlDecode(htmlNode.Attributes.Where(y => y.Name.Equals(RtfImageSrcName)).Select(x => x.Value).First());

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    //to get the image height from the specified path
                    int imageHeight = Image.FromFile(imageUrl).Height;
                    //to get the image width from the specified path
                    int imageWidth = Image.FromFile(imageUrl).Width;
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        //declare default height and width as 0 (As per Rtf dll if its 0 than it will take original height/width else specified height/width)

                        //Get height/width if html node (Image) contains those attribute
                        var heightAttribute = htmlNode.Attributes.FirstOrDefault(y => y.Name.Equals(RtfHeightAttribute));
                        var widthAttribute = htmlNode.Attributes.FirstOrDefault(y => y.Name.Equals(RtfWidthAttribute));
                        int height;
                        int width;
                        //checking heightAttribute value
                        if (heightAttribute != null)
                            //setting maximum height for the image
                            height = Convert.ToInt32(heightAttribute.Value) > Constants.ImageHeight
                                ? Constants.ImageHeight
                                : Convert.ToInt32(heightAttribute.Value);
                        else
                        {
                            //setting maximum height for the image
                            height = imageHeight > Constants.ImageHeight ? Constants.ImageHeight : imageHeight;
                        }

                        if (widthAttribute != null)
                            //setting maximum width for the image
                            width = Convert.ToInt32(widthAttribute.Value) > Constants.ImageWidth
                                ? Constants.ImageWidth
                                : Convert.ToInt32(widthAttribute.Value);
                        else
                        {
                            //setting maximum width for the image
                            width = imageWidth > Constants.ImageWidth ? Constants.ImageWidth : imageWidth;
                        }
                        //Decode image url
                        imageUrl = Uri.UnescapeDataString(imageUrl);

                        Regex regEx = new Regex(Constants.UrlSourceRegex, RegexOptions.Compiled);
                        //Check added image is from URL or uploaded folder
                        if (regEx.Match(imageUrl).Success)
                        {
                            //Getting image from url and convert it into bitmap
                            WebRequest request = WebRequest.Create(imageUrl);
                            WebResponse response = request.GetResponse();
                            Stream stream = response.GetResponseStream();
                            if (stream != null)
                            {
                                Bitmap bitmap = new Bitmap(stream);
                                rtfHtmlItem.HtmlImage = bitmap;
                            }
                        }
                        else
                        {
                            if (File.Exists(imageUrl))
                            {
                                //getting image from uploaded folder
                                using (FileStream fileStream = File.Open(imageUrl, FileMode.Open, FileAccess.Read,
                                    FileShare.ReadWrite))
                                {
                                    Bitmap bitmap = new Bitmap(fileStream);
                                    rtfHtmlItem.HtmlImage = bitmap;
                                }
                            }
                        }

                        //Assign Image width/height
                        rtfHtmlItem.ImageHeight = height;
                        rtfHtmlItem.ImageWidth = width;
                    }
                }
            }

            return rtfHtmlItem;
        }

        /// <summary>
        /// RTFs the preview.
        /// </summary>
        /// <param name="letterTemplateViewModel">The letter template view model.</param>
        /// <param name="reportVirtualPath">The report virtual path.</param>
        /// <returns></returns>
        public static string[] RtfPreview(LetterTemplateViewModel letterTemplateViewModel, string reportVirtualPath)
        {
            AppealLetter letterReportContainer = new AppealLetter
            {
                LetterTemplaterText = letterTemplateViewModel.TemplateText,
                AppealLetterClaims = new List<AppealLetterClaim>(),
                IsPreview = true
            };

            //Create default letter data / claim data
            AppealLetterClaim appealLetterClaim = new AppealLetterClaim
            {
                BillDate = LetterBillDate,
                BillType = LetterBillType,
                ContractName = LetterContractName,
                Drg = LetterDrg,
                ExpectedAllowed = LetterExpectedAllowed,
                Ftn = LetterFtn,
                MedRecNumber = LetterMedRecNumber,
                PrimaryGroupNumber = PrimaryGroupNumber,
                SecondaryGroupNumber = SecondaryGroupNumber,
                TertiaryGroupNumber = TertiaryGroupNumber,

                PrimaryMemberId = PrimaryMemberId,
                SecondaryMemberId = SecondaryMemberId,
                TertiaryMemberId = TertiaryMemberId,

                Npi = LetterNpi,
                PatientAccountNumber = LetterPatientAccountNumber,
                PatientDob = LetterPatientDateOfBirth,
                PatientFirstName = LetterPatientFirstName,
                PatientLastName = LetterPatientLastName,
                PatientMiddleName = LetterPatientMiddleName,
                PatientResponsibility = LetterPatientResponsibility,
                PayerName = LetterPayerName,
                ProviderName = LetterProviderName,
                RemitCheckDate = LetterRemitCheckDate,
                RemitPayment = LetterRemitPayment,
                StatementFrom = LetterStatementFrom,
                StatementThru = LetterStatementThru,
                ClaimTotal = LetterClaimTotal,
                Icn = LetterIcn,
                Los = LetterLos,
                Age = LetterAge
            };
            letterReportContainer.AppealLetterClaims.Add(appealLetterClaim);
            string[] test = GetExportedFileName(letterReportContainer, reportVirtualPath, Constants.AppealLetterPreviewFileBaseName);
            return test;

        }

        /// <summary>
        /// Updates the image URL.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <param name="applicationPath">The application path.</param>
        /// <returns></returns>
        public static LetterTemplate UpdateImageUrl(LetterTemplate letterTemplate, string applicationPath)
        {
            //Replace Image Path
            MatchCollection matchCollection =
                Regex.Matches(letterTemplate.TemplateText, Constants.SourceRegex);
            for (int iloop = 0; iloop < matchCollection.Count; iloop++)
            {
                if (matchCollection[iloop].ToString().Contains(Constants.Imagepath))
                {
                    var paths = matchCollection[iloop].ToString().Split('/');
                    if (applicationPath != null && paths.Length > 1)
                        paths[1] = applicationPath.Remove(0, 1);
                    string path = string.Join(@"/", paths);
                    letterTemplate.TemplateText = letterTemplate.TemplateText.Replace(matchCollection[iloop].ToString(),
                        path);
                }
            }
            return letterTemplate;
        }
        
        /// <summary>
        /// Gets the RTF text align.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <returns></returns>
        private static RtfTextAlign GetRtfTextAlign(string align)
        {
            RtfTextAlign rtfTextAlign;
            switch (align.ToLower())
            {
                case Constants.AlignLeft:
                    rtfTextAlign = RtfTextAlign.Left;
                    break;
                case Constants.AlignRight:
                    rtfTextAlign = RtfTextAlign.Right;
                    break;
                case Constants.AlignCenter:
                    rtfTextAlign = RtfTextAlign.Center;
                    break;
                default:
                    rtfTextAlign = RtfTextAlign.Justified;
                    break;
            }
            return rtfTextAlign;
        }
       
    }
}