using DisciplinarySystem.Domain.Epistles;

namespace DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels
{
    public class GetEpistleApi
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Reciver { get; set; }
        public String Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? CaseId { get; private set; }
        public long? ComplaintId { get; set; }
        public IEnumerable<GetEpistleDocumentApi> Documents { get; set; }


        public static GetEpistleApi Create ( Epistle entity ) =>
            new GetEpistleApi
            {
                Id = entity.Id ,
                Type = entity.Type ,
                Subject = entity.Subject ,
                Sender = entity.Sender ,
                Description = entity.Description ,
                Reciver = entity.Reciver ,
                UpdateDate = entity.UpdateDate ,
                CaseId = entity.CaseId ,
                ComplaintId = entity.ComplaintId ,
                CreateDate = entity.CreateDate ,
                Documents = GetEpistleDocumentApi.Create(entity.Documents)
            };
    }

    public class GetEpistleDocumentApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetEpistleDocumentApi> Create ( IEnumerable<EpistleDocument> entities ) =>
            entities.Select(entity => new GetEpistleDocumentApi
            {
                Id = entity.Id ,
                Name = entity.Name ,
                SendTime = entity.SendTime
            });
    }

}
