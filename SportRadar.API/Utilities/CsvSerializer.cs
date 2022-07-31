using CsvHelper;
using SportRadar.API.Utilities.Interfaces;
using System.Globalization;
using System.Text;

namespace SportRadar.API.Utilities
{
    public class CsvSerializer : ISerializer
    {
        public async Task<string> SerializeAsync<T>(T input)
        {
            var csv = new StringBuilder();

            await using var textWriter = new StringWriter(csv);
            using var csvWriter = new CsvWriter(textWriter, CultureInfo.InvariantCulture);

            // automatically map the properties of T
            csvWriter.Context.AutoMap<T>();

            // we want to have a header row (optional)
            csvWriter.WriteHeader<T>();
            await csvWriter.NextRecordAsync();

            // write our record
            csvWriter.WriteRecord(input);

            // make sure all records are flushed to stream
            await csvWriter.FlushAsync();

            return csv.ToString();
        }
    }
}
