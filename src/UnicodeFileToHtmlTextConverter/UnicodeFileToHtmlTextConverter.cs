using System.IO;
using System.Web;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    public class UnicodeFileToHtmlTextConverter
    {
        private readonly TextReader _textReader;

        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath)
        {
            _textReader = File.OpenText(fullFilenameWithPath);
        }

        public UnicodeFileToHtmlTextConverter(TextReader textReader)
        {
            _textReader = textReader;
        }

        public string ConvertToHtml()
        {
            using (_textReader)
            {
                string html = string.Empty;

                string line = _textReader.ReadLine();
                while (line != null)
                {
                    html += HttpUtility.HtmlEncode(line);
                    html += "<br />";
                    line = _textReader.ReadLine();
                }

                return html;
            }
        }
    }
}
