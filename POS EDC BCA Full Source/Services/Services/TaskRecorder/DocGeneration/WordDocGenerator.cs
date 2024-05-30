/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.IO;
using LSRetailPosis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Models;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Properties;
using Microsoft.Office.Interop.Word;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.DocGeneration
{
    /// <summary>
    /// Creates "Recording.doc" document containing steps/instructions and screenshots
    /// </summary>
    public static class WordDocGenerator
    {
        #region private fields
        private static object oEndOfDoc = "\\endofdoc";
        private const string RecordingDocFilename = "Recording.doc";
        private static object oMissing = System.Reflection.Missing.Value;
        private static object rangeObj;
        private static Range range;
        #endregion

        #region public methods
        /// <summary>
        /// Creates the recording word document.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="recordingFolderPath">The recording folder path.</param>
        /// <param name="templatePath">The template path.</param>
        /// <returns>
        /// Created doc path.
        /// </returns>
        public static async Task<string> CreateRecordingWordDocAsync(string taskName, IEnumerable<MessageRecord> messages, string recordingFolderPath, string templatePath)
        {
            string returnFilePath = null;

            string screenshotsFolder = Path.Combine(recordingFolderPath, Utils.ScreenshotsWorkingFolder);
            string filePath = Path.Combine(recordingFolderPath, RecordingDocFilename);
            object template = string.IsNullOrWhiteSpace(templatePath) ? oMissing : templatePath;

            if (messages != null)
            {
                if (!Directory.Exists(screenshotsFolder))
                {
                    Directory.CreateDirectory(screenshotsFolder);
                }

                try
                {
                    await System.Threading.Tasks.Task.Factory.StartNew(() => CreateDocument(taskName, messages, screenshotsFolder, filePath, template));
                }
                finally
                {
                    Directory.Delete(screenshotsFolder, true);
                }

                returnFilePath = filePath;
            }

            return returnFilePath;
        }

        #endregion

        #region private methods

        private static void CreateDocument(string taskName, IEnumerable<MessageRecord> messages, string screenshotsFolder, string filePath, object template)
        {
            Application wordApp = new Application() { Visible = false };
            Document doc = wordApp.Documents.Add(Template: ref template, Visible: false);
            doc.Activate();

            AppendHeading1Para(doc, taskName);
            AppendEmptyPara(doc);
            AppendTextPara(doc, ApplicationLocalizer.Language.Translate(58433)); // Document generation for POS Task Recorder
            AppendAuthorsTable(doc);
            AppendEmptyPara(doc);
            AppendEmptyPara(doc);

            TableOfContents toc = AppendTableOfContents(doc);
            AppendInstructions(doc, messages, screenshotsFolder);
            toc.Update();

            try
            {
                doc.SaveAs(filePath);
            }
            finally
            {
                // Close and Quit methods should be called but compiler gives a warning as it is not able to match with a specific member and FXCop gives warnings as errors when calling it.
                // So converting to dynamic to avoid that error. 
                dynamic dynamicDoc = doc;
                dynamicDoc.Close(SaveChanges: false);
                dynamic dynamicApp = wordApp;
                dynamicApp.Quit();
            }

        }

        private static void AppendInstructions(Document doc, IEnumerable<MessageRecord> messages, string screenshotsFolder)
        {
            Paragraph instructionPara = null;
            List<int> indexes = new List<int>();
            int currentRange = 0;

            int i = 1;
            foreach (var item in messages)
            {
                string currentScreenshot = Path.Combine(screenshotsFolder, string.Format("{0}.png", item.RecordId));
                bool screenShotExists = File.Exists(currentScreenshot);

                if (screenShotExists)
                {
                    AppendPageBreak(doc);
                    string screenShotTitle = string.Format(ApplicationLocalizer.Language.Translate(58432), // Form name: {0}
                        item.WindowTitle);
                    AppendHeading1Para(doc, screenShotTitle);
                    AppendEmptyPara(doc);
                    AppendEmptyPara(doc);

                    Paragraph picPara = AppendPara(doc);
                    InlineShape shape = picPara.Range.InlineShapes.AddPicture(currentScreenshot);
                    File.Delete(currentScreenshot);
                }

                if ((screenShotExists) || (instructionPara == null))
                {
                    MakeInstructionPartsBold(doc, instructionPara, ref indexes);
                    instructionPara = AppendPara(doc);
                    currentRange = instructionPara.Range.Start + 1;
                }

                var text = MessageTextUtil.FormatMessageTextWithSequenceNumber(i++, item.HelpText);
                currentRange = PrepareInstructionParts(instructionPara, indexes, currentRange, text);
            }

            AppendEmptyPara(doc);
            MakeInstructionPartsBold(doc, instructionPara, ref indexes);
        }

        private static int PrepareInstructionParts(Paragraph instructionPara, ICollection<int> indexes, int currentRange, string text)
        {
            // Find the indexes of the single quotes. This is to make the words enclosed in quotes as bold later.
            for (int j = 0; j < text.Length; j++)
            {
                if (text[j] == '\'')
                {
                    indexes.Add(currentRange + j - indexes.Count);
                }
            }

            string reducedText = text.Replace("'", string.Empty);
            instructionPara.Range.Text += reducedText;
            currentRange = instructionPara.Range.End + indexes.Count;
            return currentRange;
        }

        private static void MakeInstructionPartsBold(Document doc, Paragraph instructionPara, ref List<int> indexes)
        {
            // Find the positions in doc which are enclosed in quotes for the current para and make them bold.
            if (instructionPara != null)
            {
                var indexesEnumerator = indexes.GetEnumerator();
                while (indexesEnumerator.MoveNext())
                {
                    int start = indexesEnumerator.Current;
                    if (indexesEnumerator.MoveNext())
                    {
                        int end = Math.Min(indexesEnumerator.Current, doc.Content.End);
                        doc.Range(start, end).Bold = 1;
                    }
                }
                indexes = new List<int>();
            }
        }

        private static TableOfContents AppendTableOfContents(Document doc)
        {
            Paragraph tocPara = AppendPara(doc);
            TableOfContents toc = doc.TablesOfContents.Add(range);
            return toc;
        }

        private static void AppendAuthorsTable(Document doc)
        {
            Paragraph tablePara = AppendPara(doc);
            Table table = tablePara.Range.Tables.Add(range, 4, 2, ref oMissing, ref oMissing);
            AddBoldCell(table, 1, 1, ApplicationLocalizer.Language.Translate(58434));// Recorded by:
            AddBoldCell(table, 2, 1, ApplicationLocalizer.Language.Translate(58435));// Recorded date:
            AddBoldCell(table, 3, 1, ApplicationLocalizer.Language.Translate(58436));// Document created:
            AddBoldCell(table, 4, 1, ApplicationLocalizer.Language.Translate(58437));// Task notes:
            object tableStyle = WdBuiltinStyle.wdStyleTableLightGrid;
            table.set_Style(ref tableStyle);
        }

        private static Paragraph AppendHeading1Para(Document doc, string text)
        {
            Paragraph heading1Para = AppendTextPara(doc, text);
            object heading1Style = WdBuiltinStyle.wdStyleHeading1;
            heading1Para.Format.set_Style(ref heading1Style);
            return heading1Para;
        }

        private static Paragraph AppendTextPara(Document doc, string text)
        {
            Paragraph para = AppendPara(doc);
            para.Range.Text = text;
            range = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            return para;
        }

        private static void AppendEmptyPara(Document doc)
        {
            range = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            range.InsertParagraphAfter();
        }

        private static void AppendPageBreak(Document doc)
        {
            range = doc.Bookmarks[oEndOfDoc].Range;
            object pageBreak = WdBreakType.wdPageBreak;
            range.InsertBreak(ref pageBreak);
        }

        private static Paragraph AppendPara(Document doc)
        {
            rangeObj = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            Paragraph para = doc.Content.Paragraphs.Add(ref rangeObj);
            return para;
        }

        private static void AddBoldCell(Table table, int row, int column, string text)
        {
            Cell cell = table.Cell(row, column);
            cell.Range.Text = text;
            cell.Range.FormattedText.Bold = 1;
        }
        #endregion
    }
}
