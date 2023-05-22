﻿using HealthFit.Object_Provider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit_Libs.InterfaceLibrary
{
    public interface IUser : IBaseInterfact
    {
        HttpResponseMessage CreateUser(User user);
        HttpResponseMessage GetUser(int UserId);
    }
}