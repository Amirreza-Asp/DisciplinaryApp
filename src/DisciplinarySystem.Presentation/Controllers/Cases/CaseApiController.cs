using DisciplinarySystem.Application.Cases.Helpers;
using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Cases.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Cases
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseApiController : ControllerBase
    {
        private readonly ICaseService _caseService;

        public CaseApiController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet("GetAll/{nationalCode}")]
        public async Task<IEnumerable<GetCases>> GetAll(long nationalCode)
        {
            return await _caseService.GetAllAsync(u=>u.Complaint.Complaining.NationalCode.Value.Equals(nationalCode.ToString()));
        }

        [HttpGet("GetById/{id}")]
       public async Task<GetCases> GetById(long id)
        {
            var entity = await _caseService.GetByIdAsync(id);
            return new GetCases
            {
                College = entity.Complaint.Complaining.College,
                EducationalGroup = entity.Complaint.Complaining.EducationalGroup,
                FullName = entity.Complaint.Complaining.FullName,
                StudentNumber = entity.Complaint.Complaining.StudentNumber.Value,
                Id = entity.Id,
                Grade = entity.Complaint.Complaining.Grade,
                Status = entity.Status.ToPersian(),
            };
        }
    }
}
