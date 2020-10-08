using BookStore.Data.Entities;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;

namespace BookStore.Helpers
{
    public class PdfGenerator
    {
        public byte[] CreatePdf(Order order, string email)
        {
            var html = BuildEmailContent(order, email);

            byte[] bytes = null;

            StringReader sr = new StringReader(html.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            return bytes;
        }

        protected StringBuilder BuildEmailContent(Order order, string email)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<header class='clearfix'>");
            sb.Append("<h2>BookStore Receipt</h2>");
            sb.Append("<br/>");
            sb.Append("<div id='company' class='clearfix'>");
            sb.Append("<div></div>");
            sb.Append("<div></div>");
            sb.Append("<br/>");
            sb.Append("<div><a href='mailto:highflyairline@gmail.com'>highflyairline@gmail.com</a></div>");
            sb.Append("<br/>");
            sb.Append("</div>");
            sb.Append("<div id='project'>");
            sb.Append($"<div><span>Client: </span>{order.User.FullName}</div>");
            sb.Append($"<div><span>Address: </span>{order.User.Address}</div>");
            sb.Append($"<div><span>Date: </span>{DateTime.UtcNow.ToLocalTime().ToLongDateString()}</div>");
            sb.Append("</div>");
            sb.Append("</header>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<main>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th class='service'>Book</th>");
            sb.Append("<th>Price</th>");
            sb.Append("<th>Quantity</th>");
            sb.Append("<th>Total</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in order.Items)
            {
                sb.Append("<tr>");
                sb.Append($"<td class='service'>{item.Item.Title}</td>");
                sb.Append($"<td class='unit'>{item.Item.Price}</td>");
                sb.Append($"<td class='qty'>{item.Quantity}</td>");
                sb.Append($"<td class='total'>{item.Value}</td>");
                sb.Append("</tr>");
            }

            sb.Append("<tr>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append($"<td><strong>TOTAL: {order.Value} €</strong></td>");
            sb.Append($"<td></td>");
            sb.Append("</tr>");


            sb.Append("<tr>");
            sb.Append("<td></td>");
            sb.Append($"<td><strong></strong></td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<div id='notices'>");

            sb.Append("</main>");
            sb.Append("<footer>");
            sb.Append("</footer>");

            return sb;
        }
    }
}
