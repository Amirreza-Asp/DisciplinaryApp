using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Presentation.Controllers.PrimaryVotes.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.PrimaryVotes
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryVoteApiController : ControllerBase
    {
        private readonly IPrimaryVoteService _pvService;
        private readonly IWebHostEnvironment _hostEnv;

        public PrimaryVoteApiController(IPrimaryVoteService pvService, IWebHostEnvironment hostEnv)
        {
            _pvService = pvService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetByCaseId/{caseId}")]
        public async Task<GetPrimaryVoteDto> GetByCaseId(long caseId)
        {
            var entity = await _pvService.GetByCaseIdAsync(caseId);
            if (entity == null)
                throw new Exception("Primary Vote NotFound");

            return GetPrimaryVoteDto.Create(entity);
        }


        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _pvService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.PrimaryVoteDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
