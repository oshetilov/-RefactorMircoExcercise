using System.IO;
using System.Text;
using NUnit.Framework;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter.Tests
{

    [TestFixture]
    public class UnicodeFileToHtmlTextConverterTests
    {

        [Test]
        public void UnicodeFileToHtmlTextConverter_Should_Convert_Empty_String()
        {
            var converter = new UnicodeFileToHtmlTextConverter(new StringReader(string.Empty));

            var result = converter.ConvertToHtml();

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void UnicodeFileToHtmlTextConverter_Should_Convert_Single_Line_String()
        {
            var converter = new UnicodeFileToHtmlTextConverter(new StringReader("test123!%&"));

            var result = converter.ConvertToHtml();

            Assert.AreEqual("test123!%&amp;<br />", result);
        }


        [Test]
        public void UnicodeFileToHtmlTextConverter_Should_Convert_Multiple_Line_String()
        {
            var sb = new StringBuilder()
                .AppendLine("first *")
                .AppendLine("second &");

            var converter = new UnicodeFileToHtmlTextConverter(new StringReader(sb.ToString()));

            var result = converter.ConvertToHtml();

            Assert.AreEqual("first *<br />second &amp;<br />", result);
        }
    }
}