using DisciplinarySystem.Application.FinalVotes.Interfaces;
using DisciplinarySystem.Presentation.Controllers.FinalVotes.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalVoteApiController : ControllerBase
    {
        private readonly IFinalVoteService _fianlVoteService;
        private readonly IWebHostEnvironment _hostEnv;

        public FinalVoteApiController(IFinalVoteService fianlVoteService, IWebHostEnvironment hostEnv)
        {
            _fianlVoteService = fianlVoteService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetByCaseId/{caseId}")]
        public async Task<GetFinalVoteDto> GetByCaseId(long caseId)
        {
            var entity = await _fianlVoteService.GetByCaseIdAsync(caseId);
            return GetFinalVoteDto.Create(entity);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _fianlVoteService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.FinalVoteDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
