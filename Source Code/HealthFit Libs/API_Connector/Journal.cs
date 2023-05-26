using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using Object_Provider.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_Connector
{
    public class Journal : IJournal
    {
        public static readonly string Journal_Api_EndPoint_EditJouranal = "/Journal/EditJournal/";
        public static readonly string Journal_Api_EndPoint_DeleteJournal = "/Journal/DeleteJournal/";
        public static readonly string Journal_Api_EndPoint_GetAllJournal = "/Journal/GetAllJournal/";
        public static readonly string Journal_Api_EndPoint_GetJournal = "/Journal/GetJournal/";
        public static readonly string Journal_Api_EndPoint_GetAllCategoryList = "/Journal/GetAllCategoryList/";

        readonly ApiServerDetails _apiserver;
        readonly HTTPConnector apiConnector;
        /// <summary>
        /// Journal APi Connection
        /// </summary>
        /// <param name="apiserver"></param>
        public Journal(ApiServerDetails apiserver)
        {
            _apiserver = apiserver;
            apiConnector = new HTTPConnector(_apiserver);
        }
        /// <summary>
        /// Add or Edit Journal Information
        /// </summary>
        /// <param name="journal"></param>
        /// <returns></returns>
        public bool EditJournal(HealthFit.Object_Provider.Model.Journal journal)
        {
            return apiConnector.SendJsonRequest<bool>(Journal_Api_EndPoint_EditJouranal, HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(journal), string.Empty);
        }
        /// <summary>
        /// Delete Journal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteJournal(int id)
        {
            return apiConnector.SendJsonRequest<bool>(Journal_Api_EndPoint_DeleteJournal, HTTPConnector.RequestMethod.GET, string.Empty, "id=" + id);
        }
        /// <summary>
        /// Get All Jouranl TYpes
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <param name="pdfByteData"></param>
        /// <returns></returns>
        public List<HealthFit.Object_Provider.Model.Journal> GetAllJournal(UserType userType, int userId = 0, bool active = true, bool pdfByteData = false)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.Journal>>(Journal_Api_EndPoint_GetAllJournal, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userType={0}&userId={1}&active={2}&pdfByteData={3}", (int)userType, userId, active, pdfByteData));
        }
        /// <summary>
        /// Get Journal details by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pdfByteData"></param>
        /// <returns></returns>
        public HealthFit.Object_Provider.Model.Journal GetJournal(int id, bool pdfByteData = false)
        {
            return apiConnector.SendJsonRequest<HealthFit.Object_Provider.Model.Journal>(Journal_Api_EndPoint_GetJournal, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("id={0}&pdfByteData={1}", id, pdfByteData));
        }
        /// <summary>
        /// Get All Category List
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<string>?>(Journal_Api_EndPoint_GetAllCategoryList, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("publisherId={0}&active={1}", publisherId, true));
        }
        /// <summary>
        /// Upload Jornal Cover Photo and Journal File
        /// </summary>
        /// <param name="journalFileUpload"></param>
        /// <returns></returns>
        public string UploadJournalCoverPhotoAndJournalFile(JournalFileUpload journalFileUpload)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiserver.ServerBaseUrl.TrimEnd('/') + "/" + "/Journal/".TrimStart('/'));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiserver.Token);

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
