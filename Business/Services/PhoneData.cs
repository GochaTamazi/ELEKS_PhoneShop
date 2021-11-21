using System.Collections.Generic;
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
            const string filePath = "./Templates/xlsx/Phones.xlsx";
            using var workBook = new XLWorkbook(filePath);
            var workSheet = workBook.Worksheet(1);

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

            await using var ms = new MemoryStream();
            workBook.SaveAs(ms);
            ms.Position = 0;
            var array = ms.ToArray();
            ms.Close();
            return array;
        }
    }
}