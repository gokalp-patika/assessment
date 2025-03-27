using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Domain.Interfaces.Services;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.API.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<Report>> RequestReport([FromBody] LocationRequest request)
        {
            var report = await _reportService.RequestReportAsync(request.Location);
            return AcceptedAtAction(nameof(GetReport), new { id = report.Id }, report);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(Guid id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            return Ok(report);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        // Implement other endpoints...
    }
} 