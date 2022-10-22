using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Invitations.Interfaces;
using DisciplinarySystem.Application.Invitations.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Invitations.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Invitations
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class InvitationController : Controller
    {
        private readonly IInvitationService _invService;
        private readonly IWebHostEnvironment _hostEnv;

        private static InvitationFilter _filters = new InvitationFilter();

        public InvitationController ( IWebHostEnvironment hostEnv , IInvitationService invService )
        {
            _hostEnv = hostEnv;
            _invService = invService;
        }

        public async Task<IActionResult> Index ( InvitationFilter filters )
        {
            _filters = filters;
            filters.CreateDate = filters.CreateDate.ToMiladi();

            var vm = new GetAllInvitations
            {
                Invitations = await GetFilteredInvitations(filters) ,
                Filters = filters ,
                TotalCount = GetFilteredCount(filters) ,
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _invService.GetByIdAsync(id);
            return View(entity);
        }

        public async Task<IActionResult> Create ( long caseId )
        {
            var dto = new CreateInvitation
            {
                CaseId = caseId ,
                Persons = await _invService.GetPersonsAsync(caseId) ,
                Subject = "دعوت به حراست دانشگاه" ,
                Description = "جناب آقای / سرکار خانم ....\n\nموضوع : دعوت به جلسه\n\nبا سلام و احترام\n\n" +
                $"بدین وسیله به استحضار می رساند جلسه ای با موضوعات ذیل فردا مورخه {DateTime.Now.ToShamsi()} راس ساعت .......... در سالن جلسات ساختمان معاونت دانشجویی با حضور مدیریت محترم عامل تشکیل خواهد شد.\n\n" +
                $"خواهشمند است راس ساعت مقرر حضور به هم رسانید.\n\n" +
                $"بررسی موضوع و ..........."
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateInvitation command )
        {
            if ( !ModelState.IsValid )
            {
                if ( !String.IsNullOrEmpty(command.InviteDate) && command.InviteDate.Split(' ').Length >= 9 )
                    command.InviteDate = DateTimeConvertor.GetDateFromString(command.InviteDate).GetWebToolKitString();

                command.Persons = await _invService.GetPersonsAsync(command.CaseId);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _invService.CreateAsync(command , files);
            TempData[SD.Success] = "دعوتنامه با موفقیت اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Update ( Guid id )
        {
            var inv = await _invService.GetByIdAsync(id);
            if ( inv == null )
            {
                TempData[SD.Error] = "دعوتنامه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var command = UpdateInvitation.Create(inv);
            command.Persons = await _invService.GetPersonsAsync(command.CaseId);

            var selectedPersonsId = inv.InvitationUsers.Select(u => u.UserId).ToList();
            if ( inv.ComplainingId.HasValue )
                selectedPersonsId.Add(inv.ComplainingId.Value);
            if ( inv.PlaintiffId.HasValue )
                selectedPersonsId.Add(inv.PlaintiffId.Value);

            var existsPersons = command.Persons.Where(u => selectedPersonsId.Contains(Guid.Parse(u.Value))).ToList();
            foreach ( var person in existsPersons )
                command.Persons.Remove(person);


            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateInvitation command )
        {
            if ( !ModelState.IsValid )
            {
                command.Persons = await _invService.GetPersonsAsync(command.CaseId);
                command.SelectedPersons = await _invService.GetInvitePersonsAsync(command.Id);

                if ( !String.IsNullOrEmpty(command.InviteDate) && command.InviteDate.Split(' ').Length >= 9 )
                    command.InviteDate = DateTimeConvertor.GetDateFromString(command.InviteDate).GetWebToolKitString();


                if ( command.SelectedPersons != null )
                    foreach ( var item in command.SelectedPersons )
                    {
                        var selectItem = command.Persons.Where(u => Guid.Parse(u.Value) == item.Id).First();
                        command.Persons.Remove(selectItem);
                    }


                command.CurrentDocuments = await _invService.GetDocumentsAsync(command.Id);

                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _invService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            return Json(new { Success = await _invService.RemoveAsync(id) });
        }

        public async Task<IActionResult> RemovePerson ( Guid invId , Guid personId , String group )
        {
            await _invService.RemovePersonAsync(invId , personId , group);
            return RedirectToAction(nameof(Update) , new { Id = invId });
        }

        public async Task<JsonResult> RemoveFile ( Guid id )
        {
            return Json(new { Success = await _invService.RemoveFileAsync(id) });
        }

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.InvitationDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }

        private async Task<IEnumerable<InvitationDetails>> GetFilteredInvitations ( InvitationFilter filters )
        {
            return await _invService.ListAsync(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId ,
                            skip: filters.Skip ,
                            take: filters.Take);
        }

        private int GetFilteredCount ( InvitationFilter filters )
        {
            return _invService.GetCount(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId);
        }
    }
}
