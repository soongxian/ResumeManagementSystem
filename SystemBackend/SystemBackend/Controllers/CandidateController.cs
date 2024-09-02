using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBackend.Core.Context;
using SystemBackend.Core.Dtos.Candidate;
using SystemBackend.Core.Entities;

namespace SystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private ApplicationDbContext _context { get; }
        private IMapper _mapper { get; }

        public CandidateController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD

        // Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto dto, IFormFile pdfFile)
        {
            // First => Save PDF to server
            // Then => Save URL into entity
            var fiveMegaByte = 5 * 1024 * 1024;
            var pdfMimeType = "application/pdf";

            if (pdfFile.Length > fiveMegaByte || pdfFile.ContentType != pdfMimeType)
            {
                return BadRequest("File is not valid.");
            }

            var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "pdf", resumeUrl);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }
            var newCandidate = _mapper.Map<Candidate>(dto);
            newCandidate.ResumeUrl = resumeUrl;
            await _context.Candidates.AddAsync(newCandidate);
            await _context.SaveChangesAsync();

            return Ok("Candidate Saved Successfully");
        }


        // Read
        [HttpGet]
        [Route("Get")]

        public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetCandidates()
        {
            var candidates = await _context.Candidates.Include(c => c.Job).ToListAsync();
            var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);

            return Ok(convertedCandidates);
        }

        // Read (Download PDF file)
        [HttpGet]
        [Route("download/{url}")]
        public IActionResult DownloadPdfFile(string url)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "pdf", url);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            var file = File(pdfBytes, "application/pdf", url);
            return file;
        }
    }
}
