using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using ClosedXML.Excel;
using Database.Models;

namespace Application.Services
{
    public class PhoneData : IPhoneData
    {
        public async Task<byte[]> ExportToXlsxAsync(List<Phone> phones, CancellationToken token)
        {
            var dt = new DataTable();
            dt.TableName = "PhonesData";

            //Add Columns
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("BrandSlug", typeof(string));
            dt.Columns.Add("PhoneSlug", typeof(string));
            dt.Columns.Add("PhoneName", typeof(string));
            dt.Columns.Add("Dimension", typeof(string));
            dt.Columns.Add("Os", typeof(string));
            dt.Columns.Add("Storage", typeof(string));
            dt.Columns.Add("ReleaseDate", typeof(string));
            dt.Columns.Add("Price", typeof(int));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Hided", typeof(string));
            dt.Columns.Add("Thumbnail", typeof(string));
            dt.Columns.Add("Images", typeof(string));
            dt.Columns.Add("Specifications", typeof(string));

            foreach (var phone in phones)
            {
                dt.Rows.Add(phone.Id,
                    phone.BrandSlug,
                    phone.PhoneSlug,
                    phone.PhoneName,
                    phone.Dimension,
                    phone.Os,
                    phone.Storage,
                    phone.ReleaseDate,
                    phone.Price,
                    phone.Stock,
                    phone.Hided,
                    phone.Thumbnail,
                    phone.Images,
                    phone.Specifications);
            }

            dt.AcceptChanges();

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(dt);
            sheet.RowHeight = 20;
            sheet.ColumnWidth = 23;

            await using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;
            return ms.ToArray();
        }
    }
}