using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using Object_Provider.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_Connector
{
    public class Journal : IJournal
    {
        readonly APIServer _apiserver;
        readonly HTTPConnector apiConnector;
        public Journal(APIServer apiserver)
        {
            _apiserver = apiserver;
            apiConnector = new HTTPConnector(_apiserver);
        }

        public bool EditJournal(HealthFit.Object_Provider.Model.Journal journal)
        {
            return apiConnector.SendJsonRequest<bool>("/Journal/EditJournal/", HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(journal), string.Empty);
        }

        public bool DeleteJournal(int id)
        {
            return apiConnector.SendJsonRequest<bool>("/Journal/DeleteJournal/", HTTPConnector.RequestMethod.GET, string.Empty, "id=" + id);
        }

        public List<HealthFit.Object_Provider.Model.Journal> GetAllJournal(UserType userType, int userId = 0, bool active = true, bool pdfByteData = false)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.Journal>>("/Journal/GetAllJournal/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userType={0}&userId={1}&active={2}&pdfByteData={3}", (int)userType, userId, active, pdfByteData));
        }

        public HealthFit.Object_Provider.Model.Journal GetJournal(int id, bool pdfByteData = false)
        {
            return apiConnector.SendJsonRequest<HealthFit.Object_Provider.Model.Journal>("/Journal/GetJournal/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("id={0}&pdfByteData={1}", id, pdfByteData));
        }

        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<string>?>("/Journal/GetAllCategoryList/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("publisherId={0}&active={1}", publisherId, true));
        }
        public string UploadJournalCoverPhotoAndJournalFile(JournalFileUpload journalFileUpload)
        {

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiserver.ServerBaseUrl.TrimEnd('/') + "/" + "/Journal/".TrimStart('/'));

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "UploadJournalCoverPhotoAndJournalFile");
            using var multipartContent = new MultipartFormDataContent {
                { new StreamContent(journalFileUpload.CoverPhotofile.OpenReadStream()), "CoverPhotofile", journalFileUpload.CoverPhotofile.FileName },
                { new StreamContent(journalFileUpload.JournalFile.OpenReadStream()), "JournalFile", journalFileUpload.JournalFile.FileName } ,
                { new StringContent(journalFileUpload.JournalId.ToString()), "JournalId"}
            };

            requestMessage.Content = multipartContent;

            var responseStatus = httpClient.Send(requestMessage);

            return responseStatus.ReasonPhrase;

        }
    }
}
