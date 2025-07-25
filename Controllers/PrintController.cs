using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using bpac;
using System.Runtime.InteropServices;
using PrintAgent.Models; // Make sure to include this if your PrintRequest is in Models

namespace PrintAgent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Print([FromBody] PrintRequest request)
        {
            if (request == null || request.printQuantity < 1)
                return BadRequest("Invalid print request.");

            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "print_log.txt");
            await System.IO.File.AppendAllTextAsync(logPath,
                $"Print request: partNumber={request.partNumber}, drawingNumber={request.drawingNumber}, Quantity={request.printQuantity}\n");

            for (int i = 0; i < request.printQuantity; i++)
            {
                var result = PrintWithBrotherSDK(request.partNumber, request.drawingNumber);
                if (!result.Success)
                    return StatusCode(500, result.ErrorMessage);
            }

            return Ok(new { status = "Print job received", details = request });
        }

        // Enhanced error reporting
        private (bool Success, string ErrorMessage) PrintWithBrotherSDK(string partNumber, string drawingNumber)
        {
            string templatePath = @"C:\Users\devch\PrintAgent\Templates\Layout.lbx"; // Update as needed
            DocumentClass doc = null;
            try
            {
                doc = new bpac.DocumentClass();

                // Try to open the template
                if (!doc.Open(templatePath))
                    return (false, $"Failed to open template at \"{templatePath}\". Check if the file exists and is a valid bPAC template.");

                // Try to set label fields
                try
                {
                    doc.GetObject("partNumber").Text = partNumber;
                    doc.GetObject("drawingNumber").Text = drawingNumber;
                }
                catch (Exception fieldEx)
                {
                    return (false, $"Template \"{templatePath}\" does not have required objects (PartNumber/DrawingNumber). Error: {fieldEx.Message}");
                }

                // Try to print
                bool printResult = doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
                if (!printResult)
                {
                    // Further diagnose: check printer name
                    var printerName = doc.Printer?.Name ?? "(none)";
                    return (false, $"Print command failed. Printer in use: \"{printerName}\". Is the printer connected and online?");
                }

                doc.Close();
                return (true, null);
            }
            catch (COMException comEx)
            {
                return (false, $"bPAC COM error: {comEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Unexpected error during print: {ex.Message}");
            }
            finally
            {
                // Ensure document is closed on failure
                try { doc?.Close(); } catch { }
            }
        }
    }

    // If your PrintRequest is in a separate file under Models, remove this class here.
    public class PrintRequest
    {
        public string partNumber { get; set; }
        public string drawingNumber { get; set; }
        public int printQuantity { get; set; }
    }
}