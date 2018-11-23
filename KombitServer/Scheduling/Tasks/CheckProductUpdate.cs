using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KombitServer.Controllers;
using KombitServer.Models;

using System;
using KombitServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Microsoft.AspNetCore.Http;

namespace KombitServer.ScheduleTask.Tasks
{
    public class CheckProductUpdate : IScheduledTask
    {
        public string Schedule => "*/1 * * * *";
        private readonly HttpClient _client;
        public CheckProductUpdate() {
            _client = new HttpClient();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            string url = "http://kombit.org//api/product/check_update_status/";
            // string url = "http://localhost:49205/api/product/check_update_status/";

            await _client.GetAsync(url);
        }

    }
}