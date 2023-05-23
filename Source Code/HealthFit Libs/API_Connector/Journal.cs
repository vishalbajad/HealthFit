﻿using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
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

        public List<HealthFit.Object_Provider.Model.Journal> GetAllJournal(int publisherId)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.Journal>>("/Journal/GetAllJournal/", HTTPConnector.RequestMethod.GET, string.Empty, "publisherId=" + publisherId);
        }

        public HealthFit.Object_Provider.Model.Journal GetJournal(int id)
        {
            return apiConnector.SendJsonRequest<HealthFit.Object_Provider.Model.Journal>("/Journal/GetJournal/", HTTPConnector.RequestMethod.GET, string.Empty, "id=" + id);
        }
    }
}
