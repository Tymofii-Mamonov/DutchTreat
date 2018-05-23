﻿using Microsoft.Extensions.Logging;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }

        public void SendMail(string to, string subject, string message)
        {
            //log
            _logger.LogInformation($"To: {to} Subject: {subject}, Body: {message}");
        }
    }
}
