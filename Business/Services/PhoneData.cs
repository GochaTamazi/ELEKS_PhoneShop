using Application.Interfaces;
using ClosedXML.Excel;
using Database.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Services
{
    public class PhoneData : IPhoneData
    {
        public async Task<byte[]> ExportToXlsxAsync(List<Phone> phones, CancellationToken token)
        {
            const string filePath = "./Templates/xlsx/Phones.xlsx";
            using var workBook = new XLWorkbook(filePath);
            var workSheet = workBook.Worksheet(1);

            var first = workSheet.FirstRow().Cells().ToDictionary(x => x.Address.ToString(), x => x.Value.ToString());
            workSheet.Clear();
            foreach (var (key, value) in first)
            {
                workSheet.Cell(key).Value = value;
            }

            var i = 2;
            foreach (var phone in phones)
            {
                workSheet.Cell("A" + i).Value = phone.Id;
                workSheet.Cell("B" + i).Value = phone.BrandSlug;
                workSheet.Cell("C" + i).Value = phone.PhoneSlug;
                workSheet.Cell("D" + i).Value = phone.PhoneName;
                workSheet.Cell("E" + i).Value = phone.Dimension;
                workSheet.Cell("F" + i).Value = phone.Os;
                workSheet.Cell("G" + i).Value = phone.Storage;
                workSheet.Cell("H" + i).Value = phone.ReleaseDate;
                workSheet.Cell("I" + i).Value = phone.Price;
                workSheet.Cell("J" + i).Value = phone.Stock;
                workSheet.Cell("K" + i).Value = phone.Hided;
                workSheet.Cell("L" + i).Value = phone.Thumbnail;
                workSheet.Cell("M" + i).Value = phone.Images;
                workSheet.Cell("N" + i).Value = phone.Specifications;
                i++;
            }

            await using var memoryStream = new MemoryStream();
            workBook.SaveAs(memoryStream);
            memoryStream.Position = 0;
            var array = memoryStream.ToArray();
            memoryStream.Close();
            return array;
        }

        public async Task<List<Phone>> ImportFromXlsxAsync(Stream stream, CancellationToken token)
        {
            var phones = new List<Phone>();

            using var workBook = new XLWorkbook(stream);
            var workSheet = workBook.Worksheet(1);

            var firstRow = workSheet.FirstRow();
            var rows = workSheet.Rows();
            foreach (var row in rows)
            {
                if (row.IsEmpty())
                {
                    break;
                }

                if (firstRow != row)
                {
                    var phone = new Phone();

                    int.TryParse(row.Cell(1).Value.ToString(), out int id);
                    phone.Id = id;

                    phone.BrandSlug = row.Cell(2).Value.ToString();
                    phone.PhoneSlug = row.Cell(3).Value.ToString();
                    phone.PhoneName = row.Cell(4).Value.ToString();
                    phone.Dimension = row.Cell(5).Value.ToString();
                    phone.Os = row.Cell(6).Value.ToString();
                    phone.Storage = row.Cell(7).Value.ToString();
                    phone.ReleaseDate = row.Cell(8).Value.ToString();

                    int.TryParse(row.Cell(9).Value.ToString(), out int price);
                    phone.Price = price;

                    int.TryParse(row.Cell(10).Value.ToString(), out int stock);
                    phone.Stock = stock;

                    bool.TryParse(row.Cell(11).Value.ToString(), out bool hided);
                    phone.Hided = hided;

                    phone.Thumbnail = row.Cell(12).Value.ToString();
                    phone.Images = row.Cell(13).Value.ToString();
                    phone.Specifications = row.Cell(14).Value.ToString();

                    phones.Add(phone);
                }
            }

            stream.Close();

            return phones;
        }
    }
}